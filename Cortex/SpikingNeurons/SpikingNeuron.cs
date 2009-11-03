using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingNeurons
{
    /// <summary>
    /// Spiking Neuron model
    /// (CC) Stéphane Denis
    /// 
    ///<!-- Creative Commons License -->
    ///<a href="http://creativecommons.org/licenses/GPL/2.0/">
    ///<img alt="CC-GNU GPL" border="0" src="http://creativecommons.org/images/public/cc-GPL-a.png" /></a><br />
    ///This software is licensed under the <a href="http://creativecommons.org/licenses/GPL/2.0/">CC-GNU GPL</a> version 2.0 or later.
    ///<!-- /Creative Commons License -->

    /// </summary>
    public class SpikingNeuron : SpikingThing
    {
        Fabric fabric;
        Dictionary<SpikingNeuron, double> efferentSynapses;

        private double state = 0;
        /// <summary>
        /// The internal neuron state
        /// Read-Only. 
        /// May be any value between minus infinite to positive infinite. Doesn't blow but may misbehave...
        /// </summary>
        public double State
        {
            get { return state; }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="f">Fabric that own this neuron. Defines its affinities, rates and treshold.</param>
        public SpikingNeuron(Fabric f)
        {
            fabric = f;
            efferentSynapses = new Dictionary<SpikingNeuron, double>();
        }

        /// <summary>
        /// Live!
        /// A slice of life to compute state.
        /// </summary>
        /// <returns></returns>
        new public bool processAndSee()
        {
            bool spike;
            lock (this)
            {
                spike = state >= fabric.Treshold;
                if (spike) state = 0;
                else state *= fabric.Leak;
                spiked = spike;// keep trace to debug
            }
            return spike;
        }

        /// <summary>
        /// Learn by developing incoming connexions.
        /// Create or update relationships to acheive desired exitation strength when sources are fired together.
        /// </summary>
        /// <param name="sources">Neurons that will contribute positivly to the charge.</param>
        /// <param name="totalStrength">(positive)Minimum expected charge resulting of full activation of sources.</param>
        public void updateExitationRelations(List<SpikingNeuron> sources, double totalStrength)
        {
            if (totalStrength < 0) throw new InvalidOperationException("Exitation is limited to positive strength.");
            // TODO: easy to optimize with a single loop that use parallel framework
            var newSources = from neuron in sources where !neuron.efferentSynapses.Keys.Contains<SpikingNeuron>(this) select neuron;
            var oldSources = from neuron in sources where neuron.efferentSynapses.Keys.Contains<SpikingNeuron>(this) select neuron;

            int contributors = sources.Count<SpikingNeuron>();

            foreach (SpikingNeuron n in newSources)
            {
                lock (n) { n.efferentSynapses.Add(this, totalStrength / contributors); }
            }

            foreach (SpikingNeuron n in oldSources)
            {
                if (n.efferentSynapses[this] > 0)// only for exitation
                {
                    double diff = n.efferentSynapses[this] - totalStrength / contributors;
                    if (diff > 0) // do not attenuate!
                    {
                        n.efferentSynapses[this] += diff;
                    }
                }
                else Console.WriteLine("WARNING : Contradiction on applying exitation from neuron "+this.Id
                    + " to neuron " + n.Id + " , which actually is an inhibition relationship. Change not applied.");
            }
        }

        /// <summary>
        /// Develop feedback connexions
        /// Update inhibition on sources in proportion of their contribution (suppose converse relationship)
        /// No positive feedback allowed
        /// </summary>
        /// <param name="sources"> Sources of exitation to inhibit. Each one is confirmed 
        /// as a positive contributor before applying inhibition relation.</param>
        public void updateFeedbackRelations(List<SpikingNeuron> sources)
        {
            var requiresFeedback = from neuron in sources where neuron.efferentSynapses.Keys.Contains<SpikingNeuron>(this) select neuron;

            // TODO: easy to optimize with a single loop that use parallel framework
            var newDestinations = from neuron in requiresFeedback where !this.efferentSynapses.Keys.Contains<SpikingNeuron>(neuron) select neuron;
            var oldDestinations = from neuron in requiresFeedback where this.efferentSynapses.Keys.Contains<SpikingNeuron>(neuron) select neuron;

            foreach (SpikingNeuron n in newDestinations)
            {
                if (n.efferentSynapses[this] > 0) // feedback on exitation only
                    lock (this) { this.efferentSynapses.Add(n, n.efferentSynapses[this] * -1.0); }
            }

            foreach (SpikingNeuron n in oldDestinations)
            {
                if (n.efferentSynapses[this] > 0) // feedback on exitation source only
                {
                    lock (n)
                    {
                        if (this.efferentSynapses[n] > 0)// Should already be on an inhibition relationship, otherwise monostable
                        {
                            // balance between existing and proposed feedback

                            this.efferentSynapses[n] = n.efferentSynapses[this] * -1.0;
                        }
                        //else Console.WriteLine("SEVERE WARNING : Contradiction on applying feedback from neuron "+this.Id
                        //+ " to neuron " + n.Id + " , which is an exitation relationship. Change not applied even if this means positive feedback loop (infinite loop!).");
                        // >>>>REMOVED>>>> this error needs investigation, but it to common to be a correct constraint
                    }
                }
            }
        }

        /// <summary>
        /// Develop inhibition on concurrent neurons.
        /// Create or update relationships to tries to inhibate concurrent neurons.
        /// </summary>
        /// <param name="targets">Neurons in concurrence</param>
        /// <param name="proportionalStrength">(negative)charge applied in proportion to targets tresholds.</param>
        public void updateConcurrencyRelations(List<SpikingNeuron> concurrents, double proportionalStrength)
        {
            if (proportionalStrength > 0) throw new InvalidOperationException("Inhibition is limited to negative strength.");

            // TODO: easy to optimize with a single loop that use parallel framework

            lock (this)
            {
                foreach (SpikingNeuron concurrent in concurrents)
                {
                    if (concurrent != this)
                    {
                        if (efferentSynapses.Keys.Contains<SpikingNeuron>(concurrent))
                        {

                            if (efferentSynapses[concurrent] < 0)// only for inhibition
                            {
                                double diff = efferentSynapses[concurrent] - fabric.Treshold * proportionalStrength;
                                if (diff > 0) // do not attenuate inhibition!
                                {
                                    efferentSynapses[concurrent] -= diff;
                                }
                            }
                        }
                        else
                        {

                            try
                            {
                                efferentSynapses.Add(concurrent, fabric.Treshold * proportionalStrength);
                            }
                            catch (System.ArgumentException e)
                            {
                                Console.WriteLine("WARNING : updateConcurrencyRelations on neuron "+this.Id+" - efferent synapse already contains this new target.");
                            }
                        }
                    }
                }
            }
        }

        public void spikeFromExternal()
        {
            lock (this)
            {
                // add treshold charge, this could be heavy in asynchronous systems
                // inhibition still applies
                this.state += this.fabric.Treshold;
            }
        }
        /// <summary>
        /// Transmit charges to efferent neurons
        /// </summary>
        public void spike()
        {
            lock (this)
            {
                foreach (SpikingNeuron n in efferentSynapses.Keys)
                {
                    // TODO: optimize to avoid this lookup
                    n.state += efferentSynapses[n];
                }
                spiked = true;
            }
        }
    }
}