using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace TestVisionCS
{
    class Triangle
    {
        private Point a, b, c; // always in order A.y <= B.y <= C.y
        public Point C
        {
            get { return c; }
            set
            {
                insertABC(value);
            }
        }
        public Point B
        {
            get { return b; }
            set
            {
                insertABC(value);
            }
        }
        public Point A
        {
            get { return a; }
            set
            {
                insertABC(value);
            }
        }

        private void insertABC(Point abc)
        {
            // insertion sort
            if (abc.Y > c.Y)
            {
                a = b;
                b = c;
                c = abc;
            }
            else
            {
                if (abc.Y > b.Y)
                {
                    a = b;
                    b = abc;
                }
                else
                {
                    a = abc;
                }
            }
        }

        private rawColor fillColor;

        private int rawSurfaceSize = 0;  // triangle surface in pixels

        public int RawSurfaceSize
        {
            get { return rawSurfaceSize; }
            set { rawSurfaceSize = value; }
        }

        private int rawSurfaceKnown = 0; // surface coverage of this triangle on the last bitmap used, in pixels

        public int RawSurfaceKnown
        {
            get { return rawSurfaceKnown; }
            set { rawSurfaceKnown = value; }
        }

        public rawColor FillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        public Triangle(Point A, Point B, Point C)
        {

            // avoids ordering mess
            a = new Point(-1, -1);
            b = a;
            c = a;

            // assignation with automatic ordering...

            this.A = A;
            this.B = B;
            this.C = C;
        }

        public void blendColor(rawColor c)
        {

            if (fillColor == null)
            {
                fillColor = c;
            }
            else
            {
                fillColor.R += c.R;
                //fillColor.R /= 2;

                fillColor.G += c.G;
                //fillColor.G /= 2;

                fillColor.B += c.B;
                //fillColor.B /= 2;
            }
        }

        public void paintOnBitmap(BitmapData bitmap)
        {
            unsafe
            {
                RawBitmapBGRA bmp = new RawBitmapBGRA(bitmap);
                float dx1, dx2, dx3;

                if (B.Y - A.Y > 0)
                {
                    dx1 = (float)(B.X - A.X) / (B.Y - A.Y);
                }
                else
                {
                    dx1 = 0;
                }

                if ((C.Y - A.Y) > 0)
                {
                    dx2 = (float)(C.X - A.X) / (C.Y - A.Y);
                }
                else
                {
                    dx2 = 0;
                }

                if ((C.Y - B.Y) > 0)
                {
                    dx3 = (float)(C.X - B.X) / (C.Y - B.Y);
                }
                else { dx3 = 0; }

                fPoint S = new fPoint(A.X, A.Y);
                fPoint E = new fPoint(A.X, A.Y);

                if (dx1 > dx2)
                {
                    for (; S.Y <= B.Y; S.Y++, E.Y++, S.X += dx2, E.X += dx1)
                        bmp.setLine((int)S.X, (int)E.X, (int)S.Y, fillColor);

                    E = new fPoint(B.X, B.Y);
                    for (; S.Y <= C.Y; S.Y++, E.Y++, S.X += dx2, E.X += dx3)
                        bmp.setLine((int)S.X, (int)E.X, (int)S.Y, fillColor);
                }
                else
                {
                    for (; S.Y <= B.Y; S.Y++, E.Y++, S.X += dx1, E.X += dx2)
                        bmp.setLine((int)S.X, (int)E.X, (int)S.Y, fillColor);
                    S = new fPoint(B.X, b.Y);
                    for (; S.Y <= C.Y; S.Y++, E.Y++, S.X += dx3, E.X += dx2)
                        bmp.setLine((int)S.X, (int)E.X, (int)S.Y, fillColor);
                }
            }
        }

        public void pickColorFromBitmap(BitmapData bitmap)
        {
            // Prepare for evaluation of triangle coverage on the bitmap
            rawSurfaceSize = 0;
            rawSurfaceKnown = 0;

            unsafe
            {
                RawBitmapBGRA bmp = new RawBitmapBGRA(bitmap);
                float dx1, dx2, dx3;

                if (B.Y - A.Y > 0)
                {
                    dx1 = (float)(B.X - A.X) / (B.Y - A.Y);
                }
                else
                {
                    dx1 = 0;
                }

                if ((C.Y - A.Y) > 0)
                {
                    dx2 = (float)(C.X - A.X) / (C.Y - A.Y);
                }
                else
                {
                    dx2 = 0;
                }

                if ((C.Y - B.Y) > 0)
                {
                    dx3 = (float)(C.X - B.X) / (C.Y - B.Y);
                }
                else { dx3 = 0; }

                fPoint S = new fPoint(A.X, A.Y);
                fPoint E = new fPoint(A.X, A.Y);

                if (dx1 > dx2)
                {
                    for (; S.Y <= B.Y; S.Y++, E.Y++, S.X += dx2, E.X += dx1)
                        bmp.blendFromColorLine((int)S.X, (int)E.X, (int)S.Y, this);

                    E = new fPoint(B.X, B.Y);
                    for (; S.Y <= C.Y; S.Y++, E.Y++, S.X += dx2, E.X += dx3)
                        bmp.blendFromColorLine((int)S.X, (int)E.X, (int)S.Y, this);
                }
                else
                {
                    for (; S.Y <= B.Y; S.Y++, E.Y++, S.X += dx1, E.X += dx2)
                        bmp.blendFromColorLine((int)S.X, (int)E.X, (int)S.Y, this);
                    S = new fPoint(B.X, b.Y);
                    for (; S.Y <= C.Y; S.Y++, E.Y++, S.X += dx3, E.X += dx2)
                        bmp.blendFromColorLine((int)S.X, (int)E.X, (int)S.Y, this);
                }
            }
            // normalize blending to usable color
            fillColor.R /= rawSurfaceKnown;
            fillColor.G /= rawSurfaceKnown;
            fillColor.B /= rawSurfaceKnown;
        }
    }

    class RawBitmapBGRA
    {
        private int PixelSize = 4;
        private BitmapData bitmap;

        public RawBitmapBGRA(BitmapData bitmap)
        {
            this.bitmap = bitmap;
        }

        public rawColor getPixel(int x, int y)
        {
            rawColor c = new rawColor();
            unsafe
            {
                byte* row = (byte*)bitmap.Scan0 + y * bitmap.Stride;
                int xRef = x * PixelSize;

                c.B = row[xRef];
                c.G = row[xRef + 1];
                c.R = row[xRef + 2];
                c.A = row[xRef + 3];
            }
            return c;
        }

        public void setPixel(int x, int y, rawColor c)
        {
            unsafe
            {
                byte* row = (byte*)bitmap.Scan0 + y * bitmap.Stride;
                int xRef = x * PixelSize;

                row[xRef] = (byte)c.B;
                row[xRef + 1] = (byte)c.G;
                row[xRef + 2] = (byte)c.R;
                row[xRef + 3] = (byte)c.A;
            }
        }

        public void setLine(int x1, int x2, int y, rawColor c)
        {
            unsafe
            {
                // redress X1 <= X2, to draw from left to right
                int X1 = (x1 < x2 ? x1 : x2);
                int X2 = (x1 < x2 ? x2 : x1);

                // bounds checking
                X1 = (X1 >= bitmap.Width ? bitmap.Width-1 : X1);
                X1 = (X1 < 0 ? 0 : X1);

                X2 = (X2 >= bitmap.Width ? bitmap.Width-1 : X2);
                X2 = (X2 < 0 ? 0 : X2);

                if (y >= 0 && y < bitmap.Height)
                {
                    int Y = y;

                    // bi-dimentional array mapping
                    byte* row = (byte*)bitmap.Scan0 + y * bitmap.Stride;
                    for (int x = X1; x <= X2; x++)
                    {
                        int xRef = x * PixelSize;

                        row[xRef] = (byte)c.B;
                        row[xRef + 1] = (byte)c.G;
                        row[xRef + 2] = (byte)c.R;
                        row[xRef + 3] = (byte)c.A;
                    }
                }
            }
        }
        public void blendFromColorLine(int x1, int x2, int y, Triangle t)
        {
            unsafe
            {
                rawColor rclr = new rawColor();

                // redress X1 <= X2, to draw from left to right
                int X1 = (x1 < x2 ? x1 : x2);
                int X2 = (x1 < x2 ? x2 : x1);

                int initialWith = x2 - x1+1;
                t.RawSurfaceSize += initialWith;

                // bounds checking
                X1 = (X1 >= bitmap.Width ? bitmap.Width-1 : X1);
                X1 = (X1 < 0 ? 0 : X1);

                X2 = (X2 >= bitmap.Width ? bitmap.Width-1 : X2);
                X2 = (X2 < 0 ? 0 : X2);

                int knownWidth = X2 - X1+1; // working area

                if (y >= 0 && y < bitmap.Height)
                {
                    t.RawSurfaceKnown += knownWidth; 
                    
                    int Y = y;

                    // bi-dimentional array mapping
                    byte* row = (byte*)bitmap.Scan0 + y * bitmap.Stride;
                    for (int x = X1; x <= X2; x++)
                    {
                        int xRef = x * PixelSize;

                        rclr.B = row[xRef];
                        rclr.G = row[xRef + 1];
                        rclr.R = row[xRef + 2];
                        rclr.A = row[xRef + 3];

                        t.blendColor(rclr);
                    }
                }
            }
        }
    }

    class rawColor
    {
        public int B, G, R;
        public byte A; // alpha : 255=opaque

        public rawColor()
        {
            B = G = R = 0; // default to black
            A = 255;// opaque
        }

        public rawColor(Color c)
        {
            B = c.B;
            G = c.R;
            R = c.R;
            A = c.A;
        }
    }

    // to avoid rounding errors
    class fPoint
    {
        private float x, y;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public fPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
