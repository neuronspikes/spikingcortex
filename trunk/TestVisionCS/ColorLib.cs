//*****************************************************************************************
//  File:       ColorLib.cs
//  Project:    TouchlessLib
//  Author(s):  Michael Wasserman (Michael.Wasserman@microsoft.com)
//
//  These helper classes represent colors.
//
//  TODO: Improve HSV colorspace binning/partitioning? Replace with grouping/clustering?
//  TODO: Replace RGB->HSV with a lookup table?
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace TouchlessLib
{
    /// <summary>
    /// Represents a color in RGB (Red, Green, Blue)
    /// byte Red: [0-255]
    /// byte Grn: [0-255]
    /// byte Blu: [0-255]
    /// </summary>
    internal struct RGB
    {
        public static byte RGB_MAX_RED = 255;
        public static byte RGB_MAX_GRN = 255;
        public static byte RGB_MAX_BLU = 255;

        /// <summary>
        /// Create a new RGB (Red, Green, Blue) struct
        /// </summary>
        /// <param name="r">RGB Red [0-255]</param>
        /// <param name="g">RGB Green [0-255]</param>
        /// <param name="b">RGB Blue [0-255]</param>
        public RGB(byte r, byte g, byte b)
        {
            this.r = r; this.g = g; this.b = b;
        }

        /// <summary>
        /// Create a new RGB (Red, Green, Blue) struct from an ARGB int
        /// </summary>
        /// <param name="argb">ARGB integer in the format 0xAARRGGBB</param>
        public RGB(int argb)
        {
            this.b = (byte)(argb & 0xff);
            this.g = (byte)((argb >>= 8) & 0xff);
            this.r = (byte)((argb >> 8) & 0xff);
        }

        #region Accessors

        /// <summary>
        /// RGB Red [0-255]
        /// </summary>
        public byte Red
        {
            get { return r; }
            set { r = value; }
        }

        /// <summary>
        /// RGB Green [0-255]
        /// </summary>
        public byte Grn
        {
            get { return g; }
            set { g = value; }
        }

        /// <summary>
        /// RGB Blue [0-255]
        /// </summary>
        public byte Blu
        {
            get { return b; }
            set { b = value; }
        }

        #endregion Accessors

        #region Static Conversion Methods

        /// <summary>
        /// Convert a System.Drawing.Color to its corresponding RGB
        /// </summary>
        /// <param name="c">A System.Drawing.Color</param>
        /// <returns>A corresponding RGB structure</returns>
        public static RGB ConvertFromColor(System.Drawing.Color c)
        {
            return new RGB(c.R, c.G, c.B);
        }

        /// <summary>
        /// Convert an RGB structure to its corresponding System.Drawing.Color
        /// </summary>
        /// <param name="rgb">An RGB structure</param>
        /// <returns>A corresponding System.Drawing.Color</returns>
        public static System.Drawing.Color ConvertToColor(RGB rgb)
        {
            return System.Drawing.Color.FromArgb(rgb.r, rgb.g, rgb.b);
        }

        /// <summary>
        /// Convert an RGB structure to its corresponding HSV structure
        /// </summary>
        /// <param name="rgb">An RGB structure</param>
        /// <returns>A corresponding HSV structure</returns>
        public static HSV ConvertToHSV( RGB rgb )
        {
            HSV hsv = new HSV();
            byte min, max;

            // Find the min / max for r, g, b
            if (rgb.r <= rgb.g)
            {
                if (rgb.r >= rgb.b) { min = rgb.b; max = rgb.g; }
                else { min = rgb.r; max = (rgb.g >= rgb.b)? rgb.g : rgb.b; }
            }
            else if (rgb.r <= rgb.b) { min = rgb.g; max = rgb.b; }
            else { max = rgb.r; min = (rgb.g <= rgb.b) ? rgb.g : rgb.b; }

            // HSV value and saturation
            hsv.Val = max;
            hsv.Sat = (byte)((max == 0) ? 0 : 255* (max - min) / max);

            // HSV hue
            if (max == min) hsv.Hue = 0;
            else
            {
                if (max == rgb.r) hsv.Hue = (short)((60 * (rgb.g - rgb.b) / (max - min) + 360) % 360);
                else if (max == rgb.g) hsv.Hue = (short)(60 * (rgb.b - rgb.r) / (max - min) + 120);
                else if (max == rgb.b) hsv.Hue = (short)(60 * (rgb.r - rgb.g) / (max - min) + 240);
            }

            return hsv;
        }

        #endregion

        private byte r;
        private byte g;
        private byte b;
    }


    /// <summary>
    /// Represents a color in HSV (Hue, Saturation, Value)
    /// byte Hue: [0-360]
    /// byte Sat: [0-255]
    /// byte Val: [0-255]
    /// </summary>
    internal struct HSV
    {
        public static short HSV_MAX_HUE = 360;
        public static byte HSV_MAX_SAT = 255;
        public static byte HSV_MAX_VAL = 255;

        /// <summary>
        /// Create a new HSV (Hue, Saturation, Value) struct
        /// </summary>
        /// <param name="h">HSV Hue [0-360]</param>
        /// <param name="s">HSV Saturation [0-255]</param>
        /// <param name="v">HSV Value [0-255]</param>
        public HSV(short h, byte s, byte v)
        {
            this.h = h; this.s = s; this.v = v;
        }

        #region Accessors

        /// <summary>
        /// HSV Hue [0-360]
        /// </summary>
        public short Hue
        {
            get { return h; }
            set { h = value; }
        }

        /// <summary>
        /// HSV Saturation [0-255]
        /// </summary>
        public byte Sat
        {
            get { return s; }
            set { s = value; }
        }

        /// <summary>
        /// HSV Value [0-255]
        /// </summary>
        public byte Val
        {
            get { return v; }
            set { v = value; }
        }

        #endregion Accessors

        #region Static Conversion Methods

        /// <summary>
        /// Convert a System.Drawing.Color to its corresponding HSV
        /// </summary>
        /// <param name="c">A System.Drawing.Color</param>
        /// <returns>A corresponding HSV structure</returns>
        public static HSV ConvertFromColor(System.Drawing.Color c)
        {
            return RGB.ConvertToHSV(new RGB(c.R, c.G, c.B));
        }

        /// <summary>
        /// Convert an HSV structure to its corresponding System.Drawing.Color
        /// </summary>
        /// <param name="hsv">An HSV structure</param>
        /// <returns>A corresponding System.Drawing.Color</returns>
        public static System.Drawing.Color ConvertToColor(HSV hsv)
        {
            RGB rgb = HSV.ConvertToRGB(hsv);
            return System.Drawing.Color.FromArgb(rgb.Red, rgb.Grn, rgb.Blu);
        }

        /// <summary>
        /// Convert an HSV structure to its corresponding RGB structure
        /// </summary>
        /// <param name="hsv">An HSV structure</param>
        /// <returns>A corresponding RGB structure</returns>
        public static RGB ConvertToRGB(HSV hsv)
        {
            int h = (hsv.h / 60) % 6;
            float f = hsv.h / 60.0F - hsv.h / 60;
            byte p = (byte)((hsv.v * (HSV.HSV_MAX_SAT - hsv.s)) / HSV.HSV_MAX_SAT);
            byte q = (byte)((hsv.v * (HSV.HSV_MAX_SAT - f * hsv.s)) / HSV.HSV_MAX_SAT);
            byte t = (byte)((hsv.v * (HSV.HSV_MAX_SAT - (1 - f) * hsv.s)) / HSV.HSV_MAX_SAT);
            RGB rgb = new RGB();
            if (h == 0) rgb = new RGB(hsv.v, t, p);
            else if (h == 1) rgb = new RGB(q, hsv.v, p);
            else if (h == 2) rgb = new RGB(p, hsv.v, t);
            else if (h == 3) rgb = new RGB(p, q, hsv.v);
            else if (h == 4) rgb = new RGB(t, p, hsv.v);
            else if (h == 5) rgb = new RGB(hsv.v, p, q);
            return rgb;
        }

        /// <summary>
        /// Returns an HSV representing the binned version of the argument HSV color
        /// </summary>
        /// <param name="hsv">The HSV color to be binned and then used to generate the hash key</param>
        /// <param name="binCounts">The count of bins per Hue, Sat, Val space</param>
        public static HSV GetBinnedHSV(HSV hsv, HSV binCounts)
        {
            HSV binnedHSV = new HSV();
            binnedHSV.Hue = (short)(hsv.Hue * (binCounts.Hue - 1) / HSV.HSV_MAX_HUE);
            binnedHSV.Sat = (byte)(hsv.Sat * (binCounts.Sat - 1) / HSV.HSV_MAX_SAT);
            binnedHSV.Val = (byte)(hsv.Val * (binCounts.Val - 1) / HSV.HSV_MAX_VAL);
            return binnedHSV;
        }

        #endregion

        private short h;
        private byte s;
        private byte v;
    }

    /// <summary>
    /// A hash key that represents an HSV color
    /// </summary>
    internal struct ColorKey
    {
        /// <summary>
        /// Constructor that takes an HSV color
        /// </summary>
        /// <param name="h">The HSV color used to generate the hash key</param>
        public ColorKey(HSV h)
        {
            hsv = h;
            key = h.Hue + h.Sat * HSV.HSV_MAX_HUE + h.Val * HSV.HSV_MAX_SAT * HSV.HSV_MAX_HUE;
        }

        /// <summary>
        /// The HSV Value used to generate the hash key
        /// </summary>
        public HSV Hsv
        {
            get { return hsv; }
            set
            {
                hsv = value;
                key = value.Hue + value.Sat * HSV.HSV_MAX_HUE + value.Val * HSV.HSV_MAX_SAT * HSV.HSV_MAX_HUE;
            }
        }

        /// <summary>
        /// Provides a hash key from the HSV value
        /// </summary>
        /// <returns>A unique int used as a hash key for this color (depends on HSV colorspace partitioning model)</returns>
        public override int GetHashCode()
        {
            return key;
        }

        /// <summary>
        /// Hashtable equality tester
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Returns true if the color components (h,s,v) are the same</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ColorKey))
                return false;
            ColorKey other = (ColorKey)obj;
            return other.hsv.Hue == hsv.Hue
                && other.hsv.Sat == hsv.Sat
                && other.hsv.Val == hsv.Val;
        }

        private HSV hsv;
        private int key;
    }
}
