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
        public PictureNeuronFibreStates inputPicture, outputPicture;
        Thread simThread;
 
        FabricViewer viewer;
        public Fabric fab;
        public UDPSpikingInputs udpInput;

        private static int frameDrops = 0;

        public Simulation(FabricViewer viewer)
        {
            this.viewer = viewer;

            fab = new Fabric("test");
            udpInput = new UDPSpikingInputs("testudp", 64*64, 12000);
            fab.connectInputFibre(udpInput);

            // tuning for 8bit grayscalse picture
            udpInput.ExitationLeak = 254.0 / 256.0;
            udpInput.SpikeWeight = 64.0 /256.0;

            fab.ExitationLeak = 254.0 / 256.0;

            inputPicture = new PictureNeuronFibreStates(64, 64, udpInput.Neurons);
            outputPicture = new PictureNeuronFibreStates(64, 64, fab.Neurons);

            interval = 1; // pause between cycles (in mSec) 
            simThread = new Thread(Live);
            simThread.Start();
        }

        public void Live()
        {
            while (!stop)
            {
                //compute neuron states        
                fab.processAndSee();

                //update view
                viewer.Dispatcher.BeginInvoke(new updateAllImagesDelegate(updateAllImages), null);
                frameDrops++;
                Thread.Sleep(interval);

            }
        }

        public delegate void updateAllImagesDelegate();

        public void updateAllImages()
        {
            inputPicture.updateImage();//.updateImageFromSpikes(fab.getInputFibre("testudp").Neurons);
            outputPicture.updateImage();//.updateImageFromSpikes(fab.Outputs);

            viewer.Msg.Content = "Neuron count : "+SpikingNeuron.Count+" Connexions = "+SpikingNeuron.Connexions+" Learning drops = "+Fabric.NotLearning+" Frame drops = "+frameDrops;
            frameDrops=0;
            Fabric.NotLearning = 0;
        }
    }
}
