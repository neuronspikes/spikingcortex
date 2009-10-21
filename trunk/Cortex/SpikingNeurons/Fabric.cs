using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingNeurons
{
    /// <summary>
    /// Neuron Fabric
    /// (CC) Stéphane Denis
    /// 
    /// Fabric is the finest grain of cortex architecture configuration.
    /// It defines its affinities, rates and treshold of a group of neurons.
    /// 
    ///<!-- Creative Commons License -->
    ///<a href="http://creativecommons.org/licenses/GPL/2.0/">
    ///<img alt="CC-GNU GPL" border="0" src="http://creativecommons.org/images/public/cc-GPL-a.png" /></a><br />
    ///This software is licensed under the <a href="http://creativecommons.org/licenses/GPL/2.0/">CC-GNU GPL</a> version 2.0 or later.
    ///<!-- /Creative Commons License -->
    /// </summary>
    public class Fabric : Fibre 
    {
        /// <summary>
        /// Default constructor. 
        /// Treshold=1
        /// SpikeEffect=0 : Complete reset
        /// Leak=1 : No leak
        /// 
        /// No size constraint >> Be careful, your computer can die!
        /// </summary>
        /// <param name="name">Unique name of this neuron fabric</param>
        public Fabric(string name)
        {
            this.name = name;
            neurons = new HashSet<SpikingNeuron>();
            inputs = new Dictionary<string,InputFibre>();
            outputs = new Dictionary<Int32, SpikingNeuron>();

            treshold = 1;
            spikeEffect = 0; // spikes do reset neuron state
            leak = 1; // neurons have no leak
        }



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
        private double treshold;

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
        private double spikeEffect;

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
        private double leak;

        // internal workset of neurons, may contains neurons from other fabric (inputs)
        private HashSet<SpikingNeuron> neurons;

        // input interfaces
        private Dictionary<string, InputFibre> inputs;
        /// <summary>
        /// Inputs are bound to a set of dedicated neurons to have 
        /// efferent connections in the fabric. These neurons are spiked by their input
        /// </summary>
        /// <param name="input"></param>
        public void connectInputFibre(InputFibre input){
            inputs.Add(input.Name, input);

            foreach(SpikingThing st in input.Set.Values){
                SpikingNeuron sn = new SpikingNeuron(this);
                neurons.Add(sn);
                st.doSpikeMethod = sn.spike;
            }
        }

        // exposed output worksets
        private Dictionary<Int32, SpikingNeuron> outputs;
        public void addOutput(Int32 key, SpikingNeuron n)
        {
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
