using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.Serialization;
using System.IO;
using SpikingNeurons;

namespace CortexViewer
{
    class Simulation
    {
        public bool stop = false; // true will stop "Live" thread
        private bool busy = false;
        public int interval;
        public PictureNeuronFibreStates inputPicture, outputPicture;
        Thread simThread;
 
        FabricViewer viewer;
        public Fabric fab;
        public UDPSpikingInputs udpInput;

        private static int frameDrops = 0;

        public DataContractSerializer fabricSerializer ;
        
        
        public Simulation(FabricViewer viewer)
        {
            this.viewer = viewer;

            fab = new Fabric("test");
            udpInput = new UDPSpikingInputs("testudp", 128 * 128, 64.0 / 256.0, 8000, 12000, 12000+8);
            fab.connectInputFibre(udpInput);

            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(typeof(SpikingNeuron));
            knownTypes.Add(typeof(UDPSpikingInputs));
            knownTypes.Add(typeof(UDPSpikingOutput));
            knownTypes.Add(typeof(Fibre));
            knownTypes.Add(typeof(string));
            knownTypes.Add(typeof(double));
            fabricSerializer = new DataContractSerializer(typeof(Fabric), knownTypes,int.MaxValue, false,true,null);

            // tuning for 8bit grayscalse picture
            udpInput.ExitationLeak = 254.0 / 256.0;
            fab.ExitationLeak = 254.0 / 256.0;

            inputPicture = new PictureNeuronFibreStates(128, 128, udpInput.Neurons);
            outputPicture = new PictureNeuronFibreStates(64, 64, fab.Neurons);

            interval = 1; // pause between cycles (in mSec) 
            startProcessing();
            udpInput.StartReception();
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
            if (!busy)
            {
                inputPicture.updateImage();//.updateImageFromSpikes(fab.getInputFibre("testudp").Neurons);
                outputPicture.updateImage();//.updateImageFromSpikes(fab.Outputs);

                viewer.Msg.Content = "Neuron count : " + SpikingNeuron.Count + " Connexions = " + SpikingNeuron.Connexions + " Learning drops = " + Fabric.NotLearning + " Frame drops = " + frameDrops;
                frameDrops = 0;
                Fabric.NotLearning = 0;
            }
        }

        public void saveFabric(string fileName){
            busy = true;
            viewer.Msg.Content = "Saving To " + fileName+"Please Wait!"; 
            FileStream file = File.Create(fileName);
            fabricSerializer.WriteObject(file,fab);
            busy = false;
        }

        public void loadFabric(string fileName)
        {
            // stop evetrything
            busy = true;
            viewer.Msg.Content = "Loading from " + fileName + "Please Wait!"; 
            udpInput.StopReception();
            stopProcessing();

            // load and rebind
            try
            {
                FileStream file = File.Open(fileName, FileMode.Open);
                fab = (Fabric)fabricSerializer.ReadObject(file);
                udpInput = (UDPSpikingInputs)fab.getInputFibre("testudp");
                inputPicture = new PictureNeuronFibreStates(128, 128, udpInput.Neurons);
                outputPicture = new PictureNeuronFibreStates(64, 64, fab.Neurons);
            }
            catch (System.IO.IOException) { 
            // TODO: Show error
            }

            // restart processes
            startProcessing();
            udpInput.StartReception();
            busy = false;
        }

        public void stopProcessing()
        {
            stop = true;
            if(simThread != null) simThread.Join();
        }
        public void startProcessing()
        {
            stopProcessing();
            simThread = new Thread(Live);
            stop = false;
            simThread.Start();
        }
    }
}
