using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TestVisionCS
{
    static class Vision
    {

        public static List<int> Spikes(PictureBox src, PictureBox monitor, int treshold)
        // Returns the list of neurons(=pixels) that exceeds the treshold
        {
            List<int> spikes = new List<int>();

            lock (src)
            {
                lock (monitor)
                {
                    Rectangle srcRect = new Rectangle(0, 0, src.Width, src.Height);
                    BitmapData sight = ((Bitmap)src.Image).LockBits(srcRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    BitmapData see = ((Bitmap)monitor.Image).LockBits(srcRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    // process
                    int PixelSize = 4;
                    int x = 0, y = 0;
                    int w = sight.Width, h = sight.Height;
                    int identity = 0; // neuron identity = pixel number in scan order
                    int luma;


                    unsafe
                    {
                        for (int yScan = y; yScan < (y + h); yScan++)
                        {
                            byte* row = (byte*)sight.Scan0 + yScan * sight.Stride;
                            byte* rowOut = (byte*)see.Scan0 + yScan * sight.Stride;
                            for (int xScan = x; xScan < (x + w); xScan++)
                            {
                                int xRef = xScan * PixelSize;
                                byte r, g, b, a;

                                b = row[xRef];
                                g = row[xRef + 1];
                                r = row[xRef + 2];
                                a = row[xRef + 3];
                                luma = (r + g + b) / 3;
                                byte l = 0;
                                if (luma > treshold)
                                {
                                    spikes.Add(identity);
                                    l = 255;

                                }
                                rowOut[xRef] = (byte)(rowOut[xRef] -1 + l / 255);
                                rowOut[xRef + 1] = (byte)(rowOut[xRef + 1] -1 + l / 255);
                                rowOut[xRef + 2] = (byte)(rowOut[xRef + 2] -1 + l / 255);
                                a = 255;
                                identity++;
                            }
                        }
                    }

                    // close
                    ((Bitmap)src.Image).UnlockBits(sight);
                    ((Bitmap)monitor.Image).UnlockBits(sight);
                    monitor.Invalidate();
                }
            }
            return spikes;
        }

        public static Bitmap Transform(Bitmap src, pixop op)
        {
            Bitmap dst = (Bitmap)src.Clone();
            Rectangle srcRect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData sight = dst.LockBits(srcRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            // process
            int PixelSize = 4;
            int x = 0, y = 0;
            int w = sight.Width, h = sight.Height;
            byte[] i = { 0, 0, 0, 0 };
            byte[] o = { 0, 0, 0, 0 };


            unsafe
            {
                for (int yScan = y; yScan < (y + h); yScan++)
                {
                    byte* row = (byte*)sight.Scan0 + yScan * sight.Stride;
                    for (int xScan = x; xScan < (x + w); xScan++)
                    {
                        int xRef = xScan * PixelSize;

                        i[0] = row[xRef];
                        i[1] = row[xRef + 1];
                        i[2] = row[xRef + 2];
                        i[3] = row[xRef + 3];

                        o = op(i);

                        row[xRef] = o[0];
                        row[xRef + 1] = o[1];
                        row[xRef + 2] = o[2];
                        row[xRef + 3] = o[3];
                    }
                }
            }

            // close
            dst.UnlockBits(sight);
            return dst;
        }

        // pixel operations
        public delegate byte[] pixop(byte[] pix); //pixel in bgra format

        public static byte[] bright(byte[] pix)
        {
            byte[] o = { 0, 0, 0, 0 };
            byte luma = (byte)((pix[0] + pix[1] + pix[2]) / 3);
            o[0] = luma;
            o[1] = luma;
            o[2] = luma;
            o[3] = 255;
            return o;
        }

        public static byte[] hue(byte[] pix)
        {
            byte[] o = { 0, 0, 0, 0 };
            Pix p = new Pix(pix[0], pix[1], pix[2], pix[3]);
            o = p.getHue();
            return o;
        }

        public static byte[] sat(byte[] pix)
        {
            byte[] o = { 0, 0, 0, 0 };
            byte luma = (byte)(Math.Abs(pix[0] - pix[1]) + Math.Abs(pix[0] - pix[2]) + Math.Abs(pix[2] - pix[1]) / 2);
            o[0] = luma;
            o[1] = luma;
            o[2] = luma;
            o[3] = pix[3];
            return o;
        }

        // Negative
        public static byte[] neg(byte[] pix)
        {
            byte[] o = { 0, 0, 0, 0 };
            o[0] = (byte)(255 - pix[0]);
            o[1] = (byte)(255 - pix[1]);
            o[2] = (byte)(255 - pix[2]);
            o[3] = pix[3];
            return o;
        }

        public static Bitmap See(Bitmap src, int cx, int cy, int cz, double theta)
        {
            Bitmap dst = (Bitmap)src.Clone();
            Rectangle srcRect = new Rectangle(0, 0, dst.Width, dst.Height);
            BitmapData bitmapData = dst.LockBits(srcRect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int divisions = 16;
            double r = Math.PI / (divisions/2); // ray width

            int z = cz;

            Triangle[,] triangle = new Triangle[100,16];

            int maxS=0;
            int cyl = 1;
            for (int s = 0; cyl <= z; s++,cyl += s)
            {
                Point a = new Point((int)(cx + cyl * Math.Cos(theta)), (int)(cy + cyl * Math.Sin(theta))); ;
                
                for (int d = 0;d<divisions ;d++ )
                {
                    double t = theta + (r+d * r);
                    Point b = new Point((int)(cx + cyl * Math.Cos(t)), (int)(cy + cyl * Math.Sin(t)));
                    Point c = new Point(cx, cy);

                    triangle[s,d] = new Triangle(a, b, c);
                    triangle[s,d].pickColorFromBitmap(bitmapData);
                    a = b; // for next triangle
                    }
                maxS=s;
            }

            for (int s = maxS; s >= 0; s--)
            {

                for (int d = 0; d < divisions; d++)
                {
                    triangle[s, d].paintOnBitmap(bitmapData);
                }
            }
            dst.UnlockBits(bitmapData);
            return dst;
        }



    }

    class ROI
    {
        private Rectangle region; //region of interest
        BitmapData bd;
        Bitmap original;
        Pix min, max, avg;

        public ROI(Bitmap original, BitmapData bd, Rectangle region)
        {
            this.original = original;
            this.bd = bd;
            this.region = region;
        }
    }
    class Pix
    {
        public int a, r, g, b;
        public int scale;   //divisor e.g. 255 for rgba32, cumulative for epsillon calculus
        public int hue, sat, bright;
        private double s; // = sat in [0,1] interval
        public int max, min;
        public double phase; // scale /6

        public Pix(byte b, byte g, byte r, byte a)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
            scale = 255;
            phase = (double)scale / 6;

            if (r == g && g == b)
            {
                hue = 0;
                sat = 0;
                bright = r;
                max = r;
                min = r;
            }
            else
            {
                max = (r > g ? r : g);
                max = (max > b ? max : b);
                min = (r < g ? r : g);
                min = (min < b ? min : b);

                bright = max;
                sat = max - min;
                s = (double)sat / scale; // float precision required for transformation

                if (r == max)
                {
                    hue = (int)(phase * ((double)(g - b) / scale) / s);
                }
                else
                {
                    if (max == g)
                        hue = (int)(2 * phase + phase * ((double)(b - r) / scale) / s);
                    else //max == b
                        hue = (int)(4 * phase + phase * ((double)(r - g) / scale) / s);
                }
                if (hue < 0) hue = (int)(hue + 6 * phase); // rephase, was negative...
            }
        }
        public byte[] getBGRA()
        {
            byte[] argb = getARGB();
            byte[] bgra = { 0, 0, 0, 0 };
            bgra[0] = argb[3];
            bgra[1] = argb[2];
            bgra[2] = argb[1];
            bgra[3] = argb[0];
            return bgra;
        }
        public byte[] getSat()
        {
            byte[] bgra = { 0, 0, 0, 0 };
            bgra[0] = (byte)sat;
            bgra[1] = (byte)sat;
            bgra[2] = (byte)sat;
            bgra[3] = 255;
            return bgra;
        }
        public byte[] getHue()
        {
            int satbak = sat;
            //sat = 255;
            s = (double)sat / 255; // float precision required for transformation

            int brightbak = bright;
            //bright = 255;

            byte[] bgra = { 0, 0, 0, 0 };
            byte[] argb = getARGB();
            bgra[0] = argb[3];
            bgra[1] = argb[2];
            bgra[2] = argb[1];
            bgra[3] = argb[0];

            sat = satbak;
            s = (double)sat / scale; // float precision required for transformation
            bright = brightbak;

            return bgra;
        }
        public byte[] getARGB()
        {
            byte[] o = { 0, 0, 0, 0 };
            int h;
            double f;
            byte p, t, q, v;

            h = (hue / (int)phase) % 6;
            f = (hue / phase) - h;

            p = (byte)(bright * (1 - s));
            t = (byte)(bright * (1 - f * s));
            q = (byte)(bright * (1 - (1 - f) * s));

            v = (byte)bright;

            if (h == 0)
            {
                o[1] = (byte)v;
                o[2] = (byte)t;
                o[3] = (byte)p;
            }
            else if (h == 1)
            {
                o[1] = (byte)q;
                o[2] = (byte)v;
                o[3] = (byte)p;
            }
            else if (h == 2)
            {
                o[1] = (byte)p;
                o[2] = (byte)v;
                o[3] = (byte)t;
            }
            else if (h == 3)
            {
                o[1] = (byte)p;
                o[2] = (byte)q;
                o[3] = (byte)v;
            }
            else if (h == 4)
            {
                o[1] = (byte)t;
                o[2] = (byte)p;
                o[3] = (byte)v;
            }
            else //if (h == 5)
            {
                o[1] = (byte)v;
                o[2] = (byte)p;
                o[3] = (byte)q;
            }

            o[0] = 255;// opaque
            return o;
        }

        public Pix(BitmapData bd, int x, int y)
        {
            int PixelSize = 4;
            unsafe
            {
                byte* row = (byte*)bd.Scan0 + y * bd.Stride;
                int xRef = x * PixelSize;

                b = row[xRef];
                g = row[xRef + 1];
                r = row[xRef + 2];
                a = row[xRef + 3];
            }
            scale = 255;
        }
        public Pix(BitmapData bd, int x, int y, int w, int h)
        {
            int PixelSize = 4;
            unsafe
            {
                for (int yScan = y; yScan < (y + h); yScan++)
                {
                    byte* row = (byte*)bd.Scan0 + yScan * bd.Stride;
                    for (int xScan = x; xScan < (x + w); xScan++)
                    {
                        int xRef = xScan * PixelSize;

                        b += row[xRef];
                        g += row[xRef + 1];
                        r += row[xRef + 2];
                        a += row[xRef + 3];
                    }
                }
            }
            scale = w * h;
        }

        public static bool operator <(Pix p1, Pix p2)
        {
            return (p1.r + p1.g + p1.b) < (p2.r + p2.g + p2.b);
        }

        public static bool operator >(Pix p1, Pix p2)
        {
            return (p1.r + p1.g + p1.b) > (p2.r + p2.g + p2.b);
        }

        public static bool operator ==(Pix p1, Pix p2)
        {
            return p1.r == p2.r && p1.g == p2.g && p1.b == p2.b;
        }

        public static bool operator !=(Pix p1, Pix p2)
        {
            return !(p1 == p2);
        }

        public static bool Equals(Pix p1, Pix p2)
        {
            // should be modified to consider scale
            return p1.r == p2.r && p1.g == p2.g && p1.b == p2.b;
        }

        public int brightness()
        {
            return r + g + b;
        }

        public int saturation()
        {
            return Math.Abs(r - g) + Math.Abs(r - b) + Math.Abs(b - g);
        }

    }
}
