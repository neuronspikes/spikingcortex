using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Cortex
{
    /// <summary>
    /// Neuron Fabric
    /// (CC) Stéphane Denis
    /// 
    ///<!-- Creative Commons License -->
    ///<a href="http://creativecommons.org/licenses/GPL/2.0/">
    ///<img alt="CC-GNU GPL" border="0" src="http://creativecommons.org/images/public/cc-GPL-a.png" /></a><br />
    ///This software is licensed under the <a href="http://creativecommons.org/licenses/GPL/2.0/">CC-GNU GPL</a> version 2.0 or later.
    ///<!-- /Creative Commons License -->
    /// </summary>
    public class Fabric
    {
        /// <summary>
        /// Default constructor. 
        /// Treshold=1
        /// SpikeEffect=0 Complete reset
        /// Leak=1 No leak
        /// </summary>
        /// <param name="name">Unique name of this neuron fabric</param>
        public Fabric(string name)
        {
            this.name = name;
            neurons = new HashSet<SpikingNeuron>();
            inputs = new Dictionary<Int32, SpikingNeuron>();
            outputs = new Dictionary<Int32, SpikingNeuron>();

            treshold = 1;
            spikeEffect = 0; // spikes do reset neuron state
            leak = 1; // neurons have no leak
        }
        
        private string name;
        /// <summary>
        /// ReadOnly 
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private double treshold;
        /// <summary>
        /// characteristics of constituants neurons
        /// determines when spike occurs = state >= treshold
        /// </summary>
        public double Treshold
        {
            get { return treshold; }
            set
            {
                if (value < 0) throw new InvalidOperationException();
                treshold = value;
            }
        }

        private double spikeEffect;
        /// <summary>
        /// What happen to the state when spiking
        /// Spike Effect is applied by multiplying state by this
        /// </summary>
        public double SpikeEffect
        {
            get { return spikeEffect; }
            set
            {
                if (value < 0 || value >= 1) throw new InvalidOperationException(); // No, 1 is not acceptable!
                spikeEffect = value;
            }
        }

        private double leak;
        /// <summary>
        /// Leaking allows temporal consideration
        /// leaking is applied by multiplying state by leak : 
        /// 1 = no leakage = static state
        /// 0 = total leakage = state lost instantly after frame processing
        /// </summary>
        public double Leak
        {
            get { return leak; }
            set
            {
                if (value < 0 || value > 1) throw new InvalidOperationException();
                leak = value;
            }
        }

        // internal workset of neurons
        private HashSet<SpikingNeuron> neurons;

        // exposed input worksets
        private Dictionary<Int32, SpikingNeuron> inputs;

        public void addInput(Int32 key, SpikingNeuron n){
            neurons.Add(n);
            inputs.Add(key,n);
        }

        private Dictionary<Int32, SpikingNeuron> outputs;
        public void addOutput(Int32 key, SpikingNeuron n)
        {
            neurons.Add(n);
            outputs.Add(key, n);
        }




        /// <summary>
        /// Live!
        ///
        /// Cycle through the fabric, put some life in it and return who spiked
        /// </summary>
        /// <returns>Set of neurons who spiked in this slice of life.</returns>
        public HashSet<SpikingNeuron> processAndSee()
        {
            HashSet<SpikingNeuron> activated = new HashSet<SpikingNeuron>();
            foreach (SpikingNeuron n in neurons)
            {
                if (n.processAndSee())
                {
                    // did spike
                    activated.Add(n);
                }
            }
            return activated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spiked"></param>
        /// <returns></returns>
        public HashSet<SpikingNeuron> learnTrough(HashSet<SpikingNeuron> spiked)
        {
            // todo: implement learning algorithm
            return spiked;
        }
    }
}
