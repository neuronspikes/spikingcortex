using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using SpikingNeurons;

namespace CortexViewer
{
    class Simulation
    {
        public bool stop = false; // true will stop "Live" thread
        public int interval;
        public PictureNeuronStates inputPicture, outputPicture, fabricPicture;
        Thread simThread;
        int droppingImages, droppedImages, droppingComputing, droppedComputing;
        bool refreshing = false;
        bool refreshingComputing = false;
        FabricViewer viewer;
        Fabric fab;

        public Simulation(FabricViewer viewer)
        {
            this.viewer = viewer;
            inputPicture = new PictureNeuronStates(1, 15, null);
            outputPicture = new PictureNeuronStates(1, 15, null);
            fabricPicture = new PictureNeuronStates(100, 50, null);
            fab = new Fabric("test");
            

            droppingImages=0;
            droppedImages=0;
            droppingComputing=0;
            droppedComputing = 0;

            interval = 200; 
            simThread = new Thread(Live);
            simThread.Start();
        }

        public void Live()
        {
            while (!stop)
            {
                if (!refreshingComputing)
                {
                    refreshingComputing = true;
                    droppedComputing = droppingComputing;
                    droppingComputing = 0;
                    //compute neuron states

                    //update view
                    viewer.Dispatcher.BeginInvoke(new updateAllImagesDelegate(updateAllImages), null);
                    refreshingComputing = false;
                }
                else droppingComputing++;
                Thread.Sleep(interval);
            }
        }

        public delegate void updateAllImagesDelegate();

        public void updateAllImages()
        {
            if (!refreshing)
            {
                refreshing = true;
                droppedImages = droppingImages;
                droppingImages = 0;
                inputPicture.updateImage();
                outputPicture.updateImage();
                fabricPicture.updateImage();

                viewer.ImageDroppedTextBlock.Text = "Dropped images : "+ droppedImages;
                viewer.ComputeDroppedTextBlock.Text = "Dropped computations : " + droppedComputing;

                refreshing = false;
            }
            else droppingImages++;

        }
    }
}
