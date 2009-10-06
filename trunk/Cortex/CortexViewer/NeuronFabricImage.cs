using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using System.Drawing.Imaging;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using SpikingNeurons;

namespace CortexViewer
{
    class PictureNeuronStates
    {
        private WriteableBitmap bitmap;
        public WriteableBitmap Bitmap
        {
            get { return bitmap; }
        }
        private Int32Rect rectangle;
        private Int32Rect rectAll;
        private int rawStride;
        private BitmapSource bitmapSrc;

        byte[] rawImage;
        List<SpikingNeuron> neuronList;

        public PictureNeuronStates(int width, int height, List<SpikingNeuron> neuronList)
        {
            this.neuronList = neuronList;
            rectangle = new Int32Rect(0, 0, width, height);
            rectAll = new Int32Rect(0, 0, 1, 1);

            // Define parameters used to create the BitmapSource.
            System.Windows.Media.PixelFormat pf = System.Windows.Media.PixelFormats.Bgra32;

            rawStride = (width * pf.BitsPerPixel + 7) / 8;
            rawImage = new byte[rawStride * height];

            // Create a BitmapSource.
            bitmapSrc = BitmapSource.Create(width, height,
                32, 32, pf, null,
                rawImage, rawStride);

            this.bitmap = new WriteableBitmap(bitmapSrc);
            bitmap.Lock();
            bitmap.Unlock();
            updateImage();
        }

        private long updateRawImageFromNeurons()
        {
            int w = rectangle.Width;
            int h = rectangle.Height;
            long coverage = w * h;
            long size = neuronList.LongCount<SpikingNeuron>();
            long coverageGap = size - coverage; // neurons not displayed, negative means unused areas (transparent)

            int index = 0; // to scan lines...
            lock (this)
            {
                // process
                int PixelSize = 4;
                int x = 0, y = 0;
                unsafe
                {
                    for (int yScan = y; yScan < (y + h); yScan++)
                    {

                        for (int xScan = x; xScan < (x + w); xScan++)
                        {
                            int xRef = xScan * PixelSize;
                            byte r = 0, g = 0, b = 0, a = 0;//black and transparent by default

                            if (index < size)
                            {
                                SpikingNeuron neuron = neuronList[index++];
                                r = (neuron.State < 0 ? (byte)(neuron.State * -255) : (byte)0); ;
                                g = (neuron.State > 0 ? (byte)(neuron.State * 255) : (byte)0);
                                b = (neuron.Spiked ? (byte)255 : (byte)0);
                                a = 255;
                            }
                            rawImage[xRef] = b;
                            rawImage[xRef + 1] = g;
                            rawImage[xRef + 2] = r;
                            rawImage[xRef + 3] = a;
                        }
                    }
                }
            }
            return coverageGap;
        }

        public long updateImage()
        {
            long coverage = 0;
            lock(bitmap){
            if (neuronList == null)// no data!
            {
                if (bitmap != null)
                {
                    // pupoulate the image with random data. Useful to debug
                    bitmap.Lock();
                    Random value = new Random();
                    value.NextBytes(rawImage);
                    bitmap.WritePixels(rectangle, rawImage, rawStride, 0);
                    bitmap.Unlock();
                }
            }
            else // update from neuron list states
            {
                if (bitmap != null)
                {

                    bitmap.Lock();
                    coverage = updateRawImageFromNeurons();

                    bitmap.WritePixels(rectangle, rawImage, rawStride, 0);
                    bitmap.Unlock();
                }

            }
            return coverage;
        }
        }
    }
}
