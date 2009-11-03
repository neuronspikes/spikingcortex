using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingNeurons
{
    public delegate void doSpike();
    public class SpikingThing
    {
        protected bool spiked;
        public doSpike doSpikeMethod;

        public bool Spiked
        {
            get { return spiked; }
        }
        private static int idCount;
        private int id;

        public int Id
        {
            get { return id; }
        }

        public SpikingThing()
        {
            id = idCount++;
        }
        /// <summary>
        /// Live!
        /// A slice of life to compute state.
        /// </summary>
        /// <returns></returns>
        public bool processAndSee()
        {
            bool spike = false; // dummy 
            spiked = spike;// keep trace to debug
            return spike;
        }

    }
}
