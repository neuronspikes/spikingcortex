using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace CortexViewer
{
    class Simulation
    {
        public bool stop = false; // true will stop "Live" thread
        public int interval;
        public PictureNeuronStates input, output, fabric;
        Thread simThread;
        Dispatcher dispatcher;
        int dropping, dropped, droppingComputing, droppedComputing;
        bool refreshing = false;
        bool refreshingComputing = false;

        public Simulation(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            input = new PictureNeuronStates(50, 500, null);
            output = new PictureNeuronStates(1000, 500, null);
            fabric = new PictureNeuronStates(50, 500, null);

            interval = 100; 
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
                    dispatcher.BeginInvoke(new updateAllImagesDelegate(updateAllImages), null);
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
                dropped = dropping;
                dropping = 0;
                input.updateImage();
                output.updateImage();
                fabric.updateImage();
                refreshing = false;
            }
            else dropping++;

        }
    }
}
