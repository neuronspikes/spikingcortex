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
 
        FabricViewer viewer;
        Fabric fab;
        UDPSpikingInputs udpInput;

        public Simulation(FabricViewer viewer)
        {
            this.viewer = viewer;

            fab = new Fabric("test");
            udpInput = new UDPSpikingInputs(fab,"testudp", 64*64, 12000);
            fab.connectInputFibre(udpInput);

            // tuning for 8bit grayscalse picture
            fab.Leak = 254.0 / 256.0;
            udpInput.SpikeWeight = 64.0 /256.0;

            inputPicture = new PictureNeuronStates(1, 64, fab.getInputFibre("testudp").Neurons);
            fabricPicture = new PictureNeuronStates(64, 200, fab.getSpikingNeurons());
            outputPicture = new PictureNeuronStates(1, 64, fab.Outputs);

            interval = 4; // pause between cycles (in mSec) 
            simThread = new Thread(Live);
            simThread.Start();

            // start asynchronous input spikes reception
            udpInput.BeginReception();
        }

        public void Live()
        {
            while (!stop)
            {
                //compute neuron states        
                fab.processAndSee();

                //update view
                viewer.Dispatcher.BeginInvoke(new updateAllImagesDelegate(updateAllImages), null);
                Thread.Sleep(interval);
            }
        }

        public delegate void updateAllImagesDelegate();

        public void updateAllImages()
        {
            inputPicture.updateImageFromSpikes(fab.getInputFibre("testudp").Neurons);
            fabricPicture.updateImage();
            outputPicture.updateImageFromSpikes(fab.Outputs);

            viewer.ImageDroppedTextBlock.Text = "";
            viewer.ComputeDroppedTextBlock.Text = "Dropped learning : " + Fabric.NotLearning;
            Fabric.NotLearning = 0;
        }
    }
}
