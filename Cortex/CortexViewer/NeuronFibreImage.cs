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
    class PictureNeuronFibreStates
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
        IEnumerable<SpikingNeuron> neuronList;

        public PictureNeuronFibreStates(int width, int height, IEnumerable<SpikingNeuron> neuronList)
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

        private long updateRawImageFromNeuronStates()
        {
            int w = rectangle.Width;
            int h = rectangle.Height;
            long coverage = w * h;
            long size ;
            lock (neuronList)
            {
                size = neuronList.LongCount();
                long coverageGap = size - coverage; // neurons not displayed, negative means unused areas (black)
                // process
                int PixelSize = 4;
                IEnumerator<SpikingNeuron> enumerator = neuronList.GetEnumerator();
                lock (this) unsafe
                    {
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                int xRef = (y * w + x) * PixelSize;
                                byte r = 0, g = 0, b = 0, a = 0;//black and opaque by default

                                if (enumerator.MoveNext())
                                {
                                    SpikingNeuron neuron = enumerator.Current;


                                    // limit state display to range [-1:1]
                                    double s = neuron.State;
                                    s = (s > 1.0 ? 1.0 : s);
                                    s = (s < -1.0 ? -1.0 : s);

                                    r = (neuron.State < 0 ? (byte)(neuron.State * -255) : (byte)0); ;
                                    g = (neuron.State > 0 ? (byte)(neuron.State * 255) : (byte)0);
                                    b = (neuron.Spiked ? (byte)255 : (byte)64);
                                    a = 255;
                                }
                                rawImage[xRef] = b;
                                rawImage[xRef + 1] = g;
                                rawImage[xRef + 2] = r;
                                rawImage[xRef + 3] = a;
                            }
                        }
                    }
                return coverageGap;
            }
        }

        public long updateImage()
        {
            long coverage = 0;
            lock (bitmap)
            {
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
                        coverage = updateRawImageFromNeuronStates();

                        bitmap.WritePixels(rectangle, rawImage, rawStride, 0);
                        bitmap.Unlock();
                    }

                }
                return coverage;
            }
        }
    }
}
