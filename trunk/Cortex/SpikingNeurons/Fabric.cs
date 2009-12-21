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
        /// Leak=0.9 : little leak
        /// 
        /// No size constraint >> Be careful, your computer can die!
        /// </summary>
        /// <param name="name">Unique name of this neuron fabric</param>
        public Fabric(string name)
        {
            this.name = name;
            neurons = new List<SpikingNeuron>();
            inputFibres = new Dictionary<string, Fibre>();
            workingSet = new List<SpikingNeuron>();
        }
        List<SpikingNeuron> workingSet;
        private static int notLearning = 0;

        public static int NotLearning
        {
            get { return Fabric.notLearning; }
            set { Fabric.notLearning = value; }
        }

        // input interfaces
        private Dictionary<string, Fibre> inputFibres;

        public Fibre getInputFibre(string name)
        {
            return inputFibres[name];
        }

        /// <summary>
        /// Inputs are bound to a set of dedicated neurons to have 
        /// efferent connections in the fabric. These neurons are spiked by their input
        /// </summary>
        /// <param name="input"></param>
        public void connectInputFibre(Fibre input)
        {
           
            inputFibres.Add(input.Name, input);
            lock (workingSet) workingSet.AddRange(input.Neurons);
        }


        /// <summary>
        /// Live!
        ///
        /// Cycle through the fabric, put some life in it and return who spiked
        /// </summary>
        /// <returns>Set of neurons who spiked in this slice of life.</returns>
        public void processAndSee()
        {
            previouslySpiked = new List<SpikingNeuron>();
            newlySpiked = new List<SpikingNeuron>(); 


            // compute state and action lists
            foreach (SpikingNeuron n in workingSet)
            {
                if (n.Spiked)
                {
                    // did spike last turn
                    previouslySpiked.Add(n);

                }
                if (n.processAndSee())
                {
                    newlySpiked.Add(n);
                }
            }

            // process efferent synapses 

            foreach (SpikingNeuron n in newlySpiked)
            {
                n.spike();
            }

            if ( (newlySpiked.Count > 0 || previouslySpiked.Count > 0))
                if (ready) learnTrough();
                else notLearning++;

        }

        private bool ready = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spiked"></param>
        /// <returns></returns>
        public void learnTrough()
        {
            ready = false; // false, if moderation is needed
            if (newlySpiked.Count == 0 && previouslySpiked.Count > 1 && canLearnNewConcepts)
            {
                SpikingNeuron n = new SpikingNeuron(this);
                lock(neurons) neurons.Add(n);
                lock(workingSet) workingSet.Add(n);

                n.updateExitationRelations(previouslySpiked, 1.0);
            }

            if(canDevelopConcurrency || canDevelopFeedback) foreach (SpikingNeuron n in newlySpiked)
            {
                if (canDevelopConcurrency) n.updateConcurrencyRelations(newlySpiked, -1.0);

                if (canDevelopFeedback) n.updateFeedbackRelations(previouslySpiked);
            }
            ready = true;
        }

        bool canLearnNewConcepts = false;

        public bool CanLearnNewConcepts
        {
            get { return canLearnNewConcepts; }
            set { canLearnNewConcepts = value; }
        }
        bool canDevelopConcurrency = false;

        public bool CanDevelopConcurrency
        {
            get { return canDevelopConcurrency; }
            set { canDevelopConcurrency = value; }
        }
        bool canDevelopFeedback = false;

        public bool CanDevelopFeedback
        {
            get { return canDevelopFeedback; }
            set { canDevelopFeedback = value; }
        }
    }
}
