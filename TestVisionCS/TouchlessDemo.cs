//****************************************************************************************
//  File:       Marker.cs
//  Project:    TouchlessDemo
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//              Gary Caldwell (gacald@microsoft.com)
//
//  Demo application to show the rudimentary functionality of the Touchless library.
//
//  TODO: Add calculated props, name editing, representative color, color avg...
//  TODO: Fix demo mode so that running demos stop when you switch to another mode
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using TouchlessLib;
using System.Diagnostics;

namespace TouchlessDemo
{
    public partial class TouchlessDemo : Form
    {
        #region Touchless Demo Management

        public TouchlessDemo()
        {
            InitializeComponent();
        }

        private void TouchlessDemo_Load(object sender, EventArgs e)
        {
            // Make a new TouchlessMgr for library interaction
            _touchlessMgr = new TouchlessMgr();

            // Initialize some members
            _dtFrameLast = DateTime.Now;
            _fAddingMarker = false;
            _fChangingMode = false;
            _markerSelected = null;
            _addedMarkerCount = 0;

            // Put the app in camera mode and select the first camera by default
            radioButtonCamera.Checked = true;
            foreach (Camera cam in _touchlessMgr.Cameras)
                comboBoxCameras.Items.Add(cam);

            if (comboBoxCameras.Items.Count > 0)
            {
                comboBoxCameras.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Oops, this requires a Webcam. Please connect a Webcam and try again.");
                Environment.Exit(0);
            }
        }

        private void TouchlessDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose of the TouchlessMgr object
            if (_touchlessMgr != null)
            {
                _touchlessMgr.Dispose();
                _touchlessMgr = null;
            }
        }

        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_fChangingMode)
                return;

            _fChangingMode = true;
            groupBoxCamera.Visible = radioButtonCamera.Checked;
            _fChangingMode = false;

        }

        private TouchlessMgr _touchlessMgr;
        private static DateTime _dtFrameLast;
        private static int _nFrameCount;
        private static Point _markerCenter;
        private static float _markerRadius;
        private static Marker _markerSelected;
        private static bool _fAddingMarker;
        private static int _addedMarkerCount;
        private static bool _fChangingMode;
        private static bool _fUpdatingMarkerUI;
        private static Image _latestFrame;
        private static Bitmap _lastFrame;

        private static int _nFrameDropped;
        private static int _nFrameProcessed=1; // avoids div/0

        private static bool _mutex=false;
        private static bool _drawSelectionAdornment;

        private static DrawDemo _drawDemo;
        private static ImageDemo _imageDemo;

        #endregion Touchless Demo Management

        #region Event Handling

        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                lock (_latestFrame)
                {
                    // Draw the latest image from the active camera
                    e.Graphics.DrawImage(_latestFrame, 0, 0, pictureBoxDisplay.Width, pictureBoxDisplay.Height);

                    // Draw the selection adornment
                    if (_drawSelectionAdornment)
                    {
                        Pen pen = new Pen(Brushes.Red, 1);
                        e.Graphics.DrawEllipse(pen, _markerCenter.X - _markerRadius, _markerCenter.Y - _markerRadius, 2 * _markerRadius, 2 * _markerRadius);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler from the active camera
        /// </summary>
        public void OnImageCaptured(object sender, CameraEventArgs args)
        {
            // Calculate FPS (only update the display once every second)
            _nFrameCount++;
            double milliseconds = (DateTime.Now.Ticks - _dtFrameLast.Ticks) / TimeSpan.TicksPerMillisecond;
            if (milliseconds >= 1000)
            {
                this.BeginInvoke(new Action<double>(UpdateFPSInUI), new object[] { _nFrameCount * 1000.0 / milliseconds });
                _nFrameCount = 0;
                _dtFrameLast = DateTime.Now;
                this.BeginInvoke(new Action<double>(UpdateFDropsInUI), new object[] { _nFrameDropped});
                //_nFrameDropped = 0;
                _nFrameProcessed = 1; // resets and avoids div/0
            }
            
            if (_mutex)
            {
                _nFrameDropped++;
            }
            else
            {
                _mutex = true;
                if (_latestFrame == null) _latestFrame = (Bitmap)args.Image.Clone();
                lock (_latestFrame)
                {
                    _nFrameProcessed++;
                    // used to start remanance only
                    if (_lastFrame == null) _lastFrame = (Bitmap)args.Image.Clone();

                    // Save the latest image for drawing

                    //if (_lastFrame.PixelFormat == PixelFormat.Format32bppArgb)
                    //{
                    //Source: http://www.bobpowell.net/lockingbits.htm
                    //Format32BppArgb Given X and Y coordinates,  the address of the first element in the 
                    // pixel is Scan0+(y * stride)+(x*4). This Points to the blue byte. The following 
                    // three bytes contain the green, red and alpha bytes. 

                        Rectangle rc = new Rectangle(0, 0, _lastFrame.Width, _lastFrame.Height);
                        BitmapData bmd = _lastFrame.LockBits(rc, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                        BitmapData bmd2 = args.Image.LockBits(rc, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                        /*
                            The BitmapData class contains the following important properties;

                            * Scan0         The address in memory of the fixed data array 
                            * Stride        The width, in bytes, of a single row of pixel data. This width is 
                                            a multiple, or possiblysub-multiple, of the pixel dimensions of 
                                            the image and may be padded out to include a few more bytes. 
                            * PixelFormat   The actual pixel format of the data. This is important for finding the right bytes 
                            * Width         The width of the locked image 
                            * Height        The height of the locked image 
                         */

                        //--- now set the alpha
                        unsafe
                        {
                            //NOTE: For this to work in VS2005, in Project > Properties > Build
                            // you must check "Allow Unsafe Code"
                            int PixelSize = 4;

                            for (int y = 0; y < bmd.Height; y++)
                            {
                                byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                                byte* row2 = (byte*)bmd2.Scan0 + (y * bmd2.Stride);
                                for (int x = 0; x < bmd.Width; x++)
                                {
                                    byte byB = row[x * PixelSize];
                                    byte byR = row[x * PixelSize + 1];
                                    byte byG = row[x * PixelSize + 2];
                                    byte byA = row[x * PixelSize + 3];

                                    byte byB2 = row2[x * PixelSize];
                                    byte byR2 = row2[x * PixelSize + 1];
                                    byte byG2 = row2[x * PixelSize + 2];
                                    byte byA2 = row2[x * PixelSize + 3];

                                    float ratio = 4; // 1/ratio for remanance

                                    //alpha
                                    int a=(Math.Abs(byR2 - byR) + Math.Abs(byG2 - byG) + Math.Abs(byB2 - byB))/3;
                                    row[x * PixelSize + 3] = (byte)(255-(a>255?255:a));
                                    //blue
                                    row[x * PixelSize] = (byte)((byB * ratio + byB2) / (ratio + 1));
                                    //green
                                    row[x * PixelSize + 1] = (byte)((byG * ratio + byG2) / (ratio + 1));
                                    //red
                                    row[x * PixelSize + 2] = (byte)((byR * ratio + byR2) / (ratio + 1));

                                    
                                }
                            }
                        }

                        //now unlock
                        _lastFrame.UnlockBits(bmd);
                        args.Image.UnlockBits(bmd2);
                        //           }
                   _latestFrame = _lastFrame;
                }
                _mutex = false;

                pictureBoxDisplay.Invalidate();
            }
        }

        // Thread safe FPS label update for UI
        private void UpdateFPSInUI(double fps)
        {
            labelCameraFPSValue.Text = "" + Math.Round(fps, 2);
        }

        // Thread safe FPS label update for UI
        private void UpdateFDropsInUI(double fps)
        {
            FDrops.Text = "" + Math.Round(fps, 2);
        }

        /// <summary>
        /// Event Handler from the selected marker in the Marker Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnSelectedMarkerUpdate(object sender, MarkerEventArgs args)
        {
           this.BeginInvoke(new Action<MarkerEventData>(UpdateMarkerDataInUI), new object[] { args.EventData });
        }

        private void UpdateMarkerDataInUI(MarkerEventData data)
        {
            if (data.Present)
            {
                labelMarkerData.Text =
                      "Center X:  " + data.X + "\n"
                    + "Center Y:  " + data.Y + "\n"
                    + "DX:        " + data.DX + "\n"
                    + "DY:        " + data.DY + "\n"
                    + "Area:      " + data.Area + "\n"
                    + "Left:      " + data.Bounds.Left + "\n"
                    + "Right:     " + data.Bounds.Right + "\n"
                    + "Top:       " + data.Bounds.Top + "\n"
                    + "Bottom:    " + data.Bounds.Bottom + "\n";
            }
            else
                labelMarkerData.Text = "Marker not present";
        }

        #endregion Event Handling

        #region Demo Mode

        /// <summary>
        /// Toggle the draw demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDrawDemo_Click(object sender, EventArgs e)
        {
            if (_drawDemo == null)
            {
                _drawDemo = new DrawDemo(_touchlessMgr, pictureBoxDisplay.Bounds);
                buttonDrawDemo.Text = "Stop Draw Demo";
                buttonSnakeDemo.Enabled = buttonImageDemo.Enabled = buttonDefendDemo.Enabled = false;
                labelDemoInstructions.Enabled = true;
                labelDemoInstructions.Text = "Drawing Demo Instructions:\n\n"
                    + "Use one or more markers to draw on a canvas.\n"
                    + "Change a marker's visible size to change its drawing width.\n"
                    + "   Bring a marker closer to or farther from the camera.\n"
                    + "   Hide or expose parts of a marker.\n\n"
                    + "Hide the entire marker to prevent it from drawing:\n"
                    + "   With a marker on your finger, 'click' to hide.\n"
                    + "   Use a marker on your thumb and hide it with your fingers.\n\n"
                    + "Can you extend this demo to make a small version of paint?\n"
                    + "Can you think of better ways to 'click'?\n\n"
                    + "Give feedback, submit code, join the community, and more:\n"
                    + "http://www.codeplex.com/touchless";
            }
            else
            {
                _drawDemo.Dispose();
                _drawDemo = null;
                buttonDrawDemo.Text = "Start Draw Demo";
                buttonSnakeDemo.Enabled = buttonImageDemo.Enabled = buttonDefendDemo.Enabled = true;
                labelDemoInstructions.Enabled = false;
                labelDemoInstructions.Text = "";
            }
        }



        /// <summary>
        /// Toggle the image demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImageDemo_Click(object sender, EventArgs e)
        {
            if (_imageDemo == null)
            {
                _imageDemo = new ImageDemo(_touchlessMgr, pictureBoxDisplay.Bounds);
                buttonImageDemo.Text = "Stop Image Demo";
                buttonDrawDemo.Enabled = buttonSnakeDemo.Enabled = buttonDefendDemo.Enabled = false;
                labelDemoInstructions.Enabled = true;
                labelDemoInstructions.Text = "Image Demo Instructions:\n\n"
                    + "Use one or two markers to control an image.\n\n"
                    + "With one marker:\n"
                    + "   Control zoom in the center area with the marker size.\n"
                    + "   Move the marker outside the center area to pan.\n\n"
                    + "With two markers:\n"
                    + "   The first marker is the lower left corner of the image.\n"
                    + "   The second marker is the upper right corner of the image.\n"
                    + "   Move the markers around together or independently.\n\n"
                    + "Can you make a more intuitive way to control an image?\n"
                    + "Can you implment arbitrary two-point transformations?\n"
                    + "Can you extend the controls to work with multiple images?\n"
                    + "Give feedback, submit code, join the community, and more:\n"
                    + "http://www.codeplex.com/touchless";
            }
            else
            {
                _imageDemo.Dispose();
                _imageDemo = null;
                buttonImageDemo.Text = "Start Image Demo";
                buttonDrawDemo.Enabled = buttonSnakeDemo.Enabled = buttonDefendDemo.Enabled = true;
                labelDemoInstructions.Enabled = false;
                labelDemoInstructions.Text = "";
            }
        }

        private void buttonMarkerAdd_Click(object sender, EventArgs e)
        {
            _fAddingMarker = !_fAddingMarker;
            buttonMarkerAdd.Text = _fAddingMarker ? "Cancel Adding Marker" : "Add A New Marker";
        }

        private void comboBoxMarkers_DropDown(object sender, EventArgs e)
        {
            // Refresh the marker dropdown list.
            comboBoxMarkers.Items.Clear();
            foreach (Marker marker in _touchlessMgr.Markers)
                comboBoxMarkers.Items.Add(marker);
        }

        private void comboBoxMarkers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_markerSelected != null)
                _markerSelected.OnChange -= new EventHandler<MarkerEventArgs>(OnSelectedMarkerUpdate);

            if (comboBoxMarkers.SelectedIndex < 0)
            {
                comboBoxMarkers.Text = "Edit An Existing Marker";
                groupBoxMarkerControl.Enabled = false;
                groupBoxMarkerControl.Text = "No Marker Selected";
                return;
            }

            _markerSelected = (Marker)comboBoxMarkers.SelectedItem;
            _markerSelected.OnChange += new EventHandler<MarkerEventArgs>(OnSelectedMarkerUpdate);

            groupBoxMarkerControl.Text = _markerSelected.Name;
            groupBoxMarkerControl.Enabled = true;
            _fUpdatingMarkerUI = true;
            checkBoxMarkerHighlight.Checked = _markerSelected.Highlight;
            checkBoxMarkerSmoothing.Checked = _markerSelected.SmoothingEnabled;
            numericUpDownMarkerThresh.Value = _markerSelected.Threshold;
            _fUpdatingMarkerUI = false;
        }

        #region UI Marker Editing

        private void checkBoxMarkerHighlight_CheckedChanged(object sender, EventArgs e)
        {
            if (_fUpdatingMarkerUI)
                return;

            ((Marker)comboBoxMarkers.SelectedItem).Highlight = checkBoxMarkerHighlight.Checked;
        }

        private void checkBoxMarkerSmoothing_CheckedChanged(object sender, EventArgs e)
        {
            if (_fUpdatingMarkerUI)
                return;

            ((Marker)comboBoxMarkers.SelectedItem).SmoothingEnabled = checkBoxMarkerSmoothing.Checked;
        }

        private void numericUpDownMarkerThresh_ValueChanged(object sender, EventArgs e)
        {
            ((Marker)comboBoxMarkers.SelectedItem).Threshold = (int)numericUpDownMarkerThresh.Value;
        }

        private void buttonMarkerRemove_Click(object sender, EventArgs e)
        {
            _touchlessMgr.RemoveMarker(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.Items.RemoveAt(comboBoxMarkers.SelectedIndex);
            comboBoxMarkers.SelectedIndex = -1;
            comboBoxMarkers.Text = "Edit An Existing Marker";
            groupBoxMarkerControl.Enabled = false;
            groupBoxMarkerControl.Text = "No Marker Selected";
            if (comboBoxMarkers.Items.Count == 0)
            {
                comboBoxMarkers.Enabled = false;
            }
        }

        #endregion UI Marker Editing

        #region Display Interaction

        private void pictureBoxDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            // If we are adding a marker - get the marker center on mouse down
            if (_fAddingMarker)
            {
                _markerCenter = e.Location;
                _markerRadius = 0;

                // Begin drawing the selection adornment
                _drawSelectionAdornment = true;
            }
        }

        private void pictureBoxDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            // If we are adding a marker - get the marker radius on mouse up, add the marker
            if (_fAddingMarker)
            {
                int dx = e.X - _markerCenter.X;
                int dy = e.Y - _markerCenter.Y;
                _markerRadius = (float)Math.Sqrt(dx * dx + dy * dy);
                // Adjust for the image/display scaling (assumes proportional scaling)
                _markerCenter.X = (_markerCenter.X * _latestFrame.Width) / pictureBoxDisplay.Width;
                _markerCenter.Y = (_markerCenter.Y * _latestFrame.Height) / pictureBoxDisplay.Height;
                _markerRadius = (_markerRadius * _latestFrame.Height) / pictureBoxDisplay.Height;
                // Add the marker
                Marker newMarker = _touchlessMgr.AddMarker("Marker #" + ++_addedMarkerCount, (Bitmap)_latestFrame, _markerCenter, _markerRadius);
                comboBoxMarkers.Items.Add(newMarker);

                // Restore the app to its normal state and clear the selection area adorment
                _fAddingMarker = false;
                buttonMarkerAdd.Text = "Add A New Marker";
                _markerCenter = new Point();
                _drawSelectionAdornment = false;
                pictureBoxDisplay.Image = new Bitmap(pictureBoxDisplay.Width, pictureBoxDisplay.Height);

                // Enable the demo and marker editing
                comboBoxMarkers.Enabled = true;
                comboBoxMarkers.SelectedIndex = comboBoxMarkers.Items.Count-1;
            }
        }

        private void pictureBoxDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // If the user is selecting a marker, draw a circle of their selection as a selection adornment
            if (_fAddingMarker && !_markerCenter.IsEmpty)
            {
                // Get the current radius
                int dx = e.X - _markerCenter.X;
                int dy = e.Y - _markerCenter.Y;
                _markerRadius = (float)Math.Sqrt(dx * dx + dy * dy);

                // Cause display update
                pictureBoxDisplay.Invalidate();
            }
        }

        #endregion Display Interaction

        #endregion Marker Mode

        #region Camera Mode

        private void comboBoxCameras_DropDown(object sender, EventArgs e)
        {
            // Refresh the list of available cameras
            comboBoxCameras.Items.Clear();
            foreach (Camera cam in _touchlessMgr.Cameras)
                comboBoxCameras.Items.Add(cam);
        }

        private void comboBoxCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Early return if we've selected the current camera
            if (_touchlessMgr.CurrentCamera == (Camera)comboBoxCameras.SelectedItem)
                return;

            // Trash the old camera
            if (_touchlessMgr.CurrentCamera != null)
            {
                _touchlessMgr.CurrentCamera.OnImageCaptured -= new EventHandler<CameraEventArgs>(OnImageCaptured);
                _touchlessMgr.CurrentCamera.Dispose();
                _touchlessMgr.CurrentCamera = null;
                comboBoxCameras.Text = "Select A Camera";
                groupBoxCameraInfo.Enabled = false;
                groupBoxCameraInfo.Text = "No Camera Selected";
                labelCameraFPSValue.Text = "0.00";
                pictureBoxDisplay.Paint -= new PaintEventHandler(drawLatestImage);
            }

            if (comboBoxCameras.SelectedIndex < 0)
            {
                pictureBoxDisplay.Paint -= new PaintEventHandler(drawLatestImage);
                comboBoxCameras.Text = "Select A Camera";
                return;
            }

            try
            {
                Camera c = (Camera)comboBoxCameras.SelectedItem;
                c.OnImageCaptured += new EventHandler<CameraEventArgs>(OnImageCaptured);
                _touchlessMgr.CurrentCamera = c;
                _dtFrameLast = DateTime.Now;

                groupBoxCameraInfo.Enabled = true;
                groupBoxCameraInfo.Text = c.ToString();

                pictureBoxDisplay.Paint += new PaintEventHandler(drawLatestImage);
            }
            catch (Exception ex)
            {
                comboBoxCameras.Text = "Select A Camera";
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCameraProperties_Click(object sender, EventArgs e)
        {
            if (comboBoxCameras.SelectedIndex < 0)
                return;

            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.ShowPropertiesDialog(this.Handle);
        }

        private void checkBoxCameraFPSLimit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownCameraFPSLimit.Visible = numericUpDownCameraFPSLimit.Enabled = checkBoxCameraFPSLimit.Checked;
            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.FpsLimit = checkBoxCameraFPSLimit.Checked ? (int)numericUpDownCameraFPSLimit.Value : -1;
        }

        private void numericUpDownCameraFPSLimit_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxCameras.SelectedIndex < 0)
                return;

            Camera c = (Camera)comboBoxCameras.SelectedItem;
            c.FpsLimit = (int)numericUpDownCameraFPSLimit.Value;
        }

        private void checkBoxCameraFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCameraFlipX.Checked)
            {
                if(checkBoxCameraFlipY.Checked)
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipXY;
                else
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipX;
            }
            else
            {
                if(checkBoxCameraFlipY.Checked)
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipY;
                else
                    _touchlessMgr.CurrentCamera.RotateFlip = RotateFlipType.RotateNoneFlipNone;
            }
        }

        #endregion Camera Mode

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}