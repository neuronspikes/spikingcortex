using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpikingNeurons
{
    /// <summary>
    /// Neuron Fibre2
    /// (CC) Stéphane Denis
    /// 
    /// Fibre2 is the notion of neuron grouping.
    /// It defines its basis relationship to other groups.
    /// 
    ///<!-- Creative Commons License -->
    ///<a href="http://creativecommons.org/licenses/GPL/2.0/">
    ///<img alt="CC-GNU GPL" border="0" src="http://creativecommons.org/images/public/cc-GPL-a.png" /></a><br />
    ///This software is licensed under the <a href="http://creativecommons.org/licenses/GPL/2.0/">CC-GNU GPL</a> version 2.0 or later.
    ///<!-- /Creative Commons License -->
    /// </summary>
    [DataContract(Name = "Fibre", Namespace = "http://model.NeuronSpikes.org")]
    [KnownType(typeof(SpikingNeuron))]
    public class Fibre
    {
        public Fibre()
        {
            treshold = 1.0;
            spikeEffect = 0; // spikes do reset neuron state
            exitationLeak = 1.0; // neurons have no exitation leak
            inhibitionLeak = 0.9; // neurons have little inhibition leak

        }
        /// <summary>
        /// ReadOnly 
        /// </summary>
        
        public string Name
        {
            get { return name; }
        }
        [DataMember]
        protected string name;

        [DataMember]
        protected int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        /// <summary>
        /// Limit the resources allocated to this Fibre.
        /// This is the maximum number of neuron this Fibre can handle.
        /// 
        /// This constraint have an impact on the quality of learning and communication
        /// by 
        /// imposing reuse through natural selection in context of attrition
        /// or encouraging diversity in context of abundance.
        /// </summary>
        [DataMember]
        public int SizeConstraint
        {
            get { return sizeConstraint; }
            set { sizeConstraint = value; }
        }
        private int sizeConstraint;
 
        /// <summary>
        /// characteristics of constituants neurons
        /// determines when spike occurs = state >= treshold
        /// </summary>
        [DataMember]
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
        [DataMember]
        public double SpikeEffect
        {
            get { return spikeEffect; }
            set
            {
                if (value < 0 || value > 1) throw new InvalidOperationException();
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
        [DataMember]
        public double ExitationLeak
        {
            get { return exitationLeak; }
            set
            {
                if (value < 0 || value > 1) throw new InvalidOperationException();
                exitationLeak = value;
            }
        }
        private double exitationLeak;

        [DataMember]
        public double InhibitionLeak
        {
            get { return inhibitionLeak; }
            set
            {
                if (value < 0 || value > 1) throw new InvalidOperationException();
                inhibitionLeak = value;
            }
        }
        private double inhibitionLeak;

        [DataMember]
        public List<SpikingNeuron> neurons;
        public IEnumerable<SpikingNeuron> Neurons
        {
            get { return neurons.AsEnumerable(); }
        }

        /// <summary>
        /// previously spiked and newly spiked lists are not limited
        /// to the current fibre, but neurons included in the frame processing
        /// </summary>
        protected List<SpikingNeuron> previouslySpiked;
        protected List<SpikingNeuron> newlySpiked;
    }
}
