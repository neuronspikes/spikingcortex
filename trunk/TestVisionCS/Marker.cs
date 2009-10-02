//*****************************************************************************************
//  File:       Marker.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//
//  Classes to represent markers and their associated data.
//  NOTE: Scanline shape rasterization proposed by Natan Zohar.
//
//  TODO: Get higher degree moments of inertia (primary/secondary axes, etc.)
//  TODO: Extend alpha smoothing with exponential decay...
//  TODO: Optimize threshold, replace threshhold concept with partial matching?
//  TODO: Expose smoothing factor?
//*****************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TouchlessLib
{
    /// <summary>
    /// Represents a marker being tracked
    /// </summary>
    public class Marker
    {
        #region Public Interface

        /// <summary>
        /// Name of the marker provided when it was created
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Representative color value of the marker
        /// </summary>
        public Color RepresentativeColor
        {
            get { return representativeColor; }
            set { representativeColor = value; }
        }

        /// <summary>
        /// Color frequency threshold used for detection
        /// </summary>
        public int Threshold
        {
            get { return frequencyThreshhold; }
            set { frequencyThreshhold = value; }
        }

        /// <summary>
        /// Enable marker motion smoothing
        /// </summary>
        public bool SmoothingEnabled
        {
            get { return smoothingEnabled; }
            set { smoothingEnabled = value; }
        }

        /// <summary>
        /// Determines if the MarkerEventData provides additional calculated properties or not
        /// </summary>
        public bool ProvideCalculatedProperties
        {
            get { return provideCalculatedProperties; }
            set { provideCalculatedProperties = value; }
        }

        /// <summary>
        /// Determines if the marker will be highlighted in images
        /// </summary>
        public bool Highlight
        {
            get { return highlight; }
            set { highlight = value; }
        }

        /// <summary>
        /// Returns the current status of the marker, useful for polling operations
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's current state</returns>
        public MarkerEventData CurrentData
        {
            get { return currData; }
            internal set { currData = value; }
        }

        /// <summary>
        /// Returns the previous status of the marker
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's previous state</returns>
        public MarkerEventData PreviousData
        {
            get { return prevData; }
            internal set { prevData = value; }
        }

        /// <summary>
        /// Returns the last known status of the marker
        /// </summary>
        /// <returns>An instance of the MarkerEventData structure that represents the marker's last state wherein it was present</returns>
        public MarkerEventData LastGoodData
        {
            get { return lastGoodData; }
            internal set { lastGoodData = value; }
        }

        /// <summary>
        /// Event fired when a marker's MarkerEventData is updated
        /// </summary>
        /// <example>
        /// The following is a code snippet that shows how to setup the <c>OnChange</c> event handler
        /// <code>
        ///     TouchlessMgr _touch = new TouchlessMgr();
        ///     ...
        /// 
        ///     // Create a new marker using the current image at a fixed location and size
        ///     Marker m = _touch.AddMarker("marker", _touch.CurrentCamera.GetCurrentImage(), new Point(100, 100), 10);
        ///     m.OnChange += new EventHandler&lt;MarkerEventArgs&gt;(Marker_OnChange);
        /// 
        ///     ...
        ///     void Marker_OnChange(object sender, MarkerEventArgs args)
        ///     {
        ///         if (args.EventData.IsPresent)
        ///         {
        ///             // Do something with args.EventData
        ///         }
        ///     }
        ///     
        /// </code>
        /// </example>
        public event EventHandler<MarkerEventArgs> OnChange;

        /// <summary>
        /// ToString override that returns the name of the marker
        /// </summary>
        /// <returns>The name of the marker</returns>
        public override String ToString()
        {
            return name;
        }

        #endregion Public Interface

        private string name;
        private Color representativeColor;
        private int frequencyThreshhold;
        private bool smoothingEnabled;
        private bool provideCalculatedProperties;
        private bool highlight;

        // Color frequency hashtable (ColorKey hashkey -> Int frequency)
        internal Hashtable hsvFreq;
        internal MarkerEventData currData;
        internal MarkerEventData prevData;
        internal MarkerEventData lastGoodData;
        internal float smoothingFactor;

        #region Internal Implementation

        // Number of bins used for color partitioning
        internal byte binsHue, binsSat, binsVal;
        // Scanline shape rasterization commands used to limit the marker search area in the image scan loop
        internal MarkerScanCommand searchMinX, searchMaxX, searchMinY, searchMaxY;

        /// <summary>
        /// Set the appearance of the marker, given its color frequencies
        /// </summary>
        /// <param name="rawHsvFreq">A 3D array of HSV frequencies</param>
        /// <returns>Success</returns>
        internal bool SetMarkerAppearance(int[, ,] rawHsvFreq)
        {
            // Get the dimensions of the cube
            binsHue = (byte)rawHsvFreq.GetLength(0);
            binsSat = (byte)rawHsvFreq.GetLength(1);
            binsVal = (byte)rawHsvFreq.GetLength(2);

            // Reset stats and hash
            representativeColor = Color.Black;
            hsvFreq = new Hashtable((int)(0.01 * binsHue * binsSat * binsVal));

            // Populate the hash, calculate threshold, and find the weighted average color
            uint thresh = 0;
            uint colors = 0;
            uint freq = 0;
            uint hue = 0, sat = 0, val = 0;
            for (byte h = 0; h < binsHue; h++)
                for (byte s = 0; s < binsSat; s++)
                    for (byte v = 0; v < binsVal; v++)
                        if (rawHsvFreq[h, s, v] > 0)
                        {
                            freq = (uint)rawHsvFreq[h, s, v];
                            hsvFreq.Add(new ColorKey(new HSV(h, s, v)), freq);
                            thresh += freq;
                            hue += freq * h;
                            sat += freq * s;
                            val += freq * v;
                            colors++;
                        }

            if (colors == 0)
                return false;

            // Set the threshold to 2*(the average color frequency)
            Threshold = (int)(2 * thresh / colors);

            // Find the representative color as a normalized weighted average
            float repH = hue * HSV.HSV_MAX_HUE / (float)(thresh * binsHue);
            float repS = sat * HSV.HSV_MAX_SAT / (float)(thresh * binsSat);
            float repV = val * HSV.HSV_MAX_VAL / (float)(thresh * binsVal);
            representativeColor = HSV.ConvertToColor(new HSV((byte)repH, (byte)repS, (byte)repV));
            return true;
        }

        /// <summary>
        /// Fire an event to notify handlers that the marker data was updated
        /// </summary>
        internal void FireMarkerEventData()
        {
            if(OnChange != null)
                OnChange(this, new MarkerEventArgs(currData, this));
        }

        internal Marker(string name)
        {
            this.name = name;
            highlight = true;
            smoothingEnabled = true;
            smoothingFactor = 0.55F;

            // Initialize the scanline search bounds
            searchMinX = new MarkerScanCommand(this, ScanCommand.addMarker, 0);
            searchMaxX = new MarkerScanCommand(this, ScanCommand.remMarker, 0);
            searchMinY = new MarkerScanCommand(this, ScanCommand.addMarker, 0);
            searchMaxY = new MarkerScanCommand(this, ScanCommand.remMarker, 0);
        }

        #endregion
    }

    /// <summary>
    /// Defines data associated with a Marker
    /// </summary>
    public struct MarkerEventData
    {
        /// <summary>
        /// Current X position of the marker
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Current Y position of the marker
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Relative distance in the X axis from the last processed location
        /// </summary>
        public int DX
        {
            get { return dx; }
            set { dx = value; }
        }

        /// <summary>
        /// Relative distance in the Y axis from the last processed location
        /// </summary>
        public int DY
        {
            get { return dy; }
            set { dy = value; }
        }

        /// <summary>
        /// If the marker is currently found
        /// </summary>
        public bool Present
        {
            get { return present; }
            set { present = value; }
        }

        /// <summary>
        /// current average color value of the marker
        /// </summary>
        public Color ColorAvg
        {
            get { return colorAvg; }
            set { colorAvg = value; }
        }

        /// <summary>
        /// The 2D volume of the marker
        /// </summary>
        public int Area
        {
            get { return area; }
            set { area = value; }
        }

        /// <summary>
        /// Bounding box of the marker area
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        /// <summary>
        /// A DateTime that reflects when this data was collected
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        private int x;
        private int y;
        private int dx;
        private int dy;
        private bool present;
        private int area;
        private Rectangle bounds;
        private Color colorAvg;
        private System.DateTime timestamp;

        // Used for bounds calculations - System.Drawing.Rectangle doesn't allow you to set top, bottom, left, right...
        internal int top, bottom, left, right;
    }

    /// <summary>
    /// Marker specific EventArgs that provides the MarkerEventData
    /// </summary>
    public class MarkerEventArgs : EventArgs
    {
        /// <summary>
        /// Marker state data
        /// </summary>
        public MarkerEventData EventData
        {
            get { return _eventData; }
        }

        /// <summary>
        /// The source marker
        /// </summary>
        public Marker EventMarker
        {
            get { return _eventMarker; }
        }

        #region Internal Implementation

        /// <summary>
        /// A constructor that takes the event data as an argument
        /// </summary>
        /// <param name="med">The marker event data for this event</param>
        /// <param name="m">The marker updated with this event</param>
        internal MarkerEventArgs(MarkerEventData med, Marker m)
        {
            _eventData = med;
            _eventMarker = m;
        }

        private MarkerEventData _eventData;
        private Marker _eventMarker;

        #endregion
    }

    #region Scanline Rasterization
    /// <summary>
    /// An enumeration used to distinguish between add/rem commands during the image scan loop
    /// </summary>
    internal enum ScanCommand
    {
        addMarker = 0,
        remMarker = 1
    }

    /// <summary>
    /// Represents a command to add or remove a marker from consideration in part of the image scan loop
    /// </summary>
    internal struct MarkerScanCommand
    {
        internal MarkerScanCommand(Marker m, ScanCommand c, int i)
        {
            marker = m;
            command = c;
            coordinate = i;
        }

        // The marker associated with this command
        public Marker marker;
        // The instrcution to add/rem the marker for entering/exiting the search region
        public ScanCommand command;
        // The coordinate of the command
        public int coordinate;
    }
    #endregion Scanline Rasterization
}
