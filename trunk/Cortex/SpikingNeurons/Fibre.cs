using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingNeurons
{
    /// <summary>
    /// Neuron Fibre
    /// (CC) Stéphane Denis
    /// 
    /// Fibre is the notion of neuron grouping.
    /// It defines its basis relationship to other groups.
    /// 
    ///<!-- Creative Commons License -->
    ///<a href="http://creativecommons.org/licenses/GPL/2.0/">
    ///<img alt="CC-GNU GPL" border="0" src="http://creativecommons.org/images/public/cc-GPL-a.png" /></a><br />
    ///This software is licensed under the <a href="http://creativecommons.org/licenses/GPL/2.0/">CC-GNU GPL</a> version 2.0 or later.
    ///<!-- /Creative Commons License -->
    /// </summary>
    public class NeuronFamily
    {
        /// <summary>
        /// ReadOnly 
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        protected string name;

        protected int size;
        /// <summary>
        /// Limit the resources allocated to this fibre.
        /// This is the maximum number of neuron this fibre can handle.
        /// 
        /// This constraint have an impact on the quality of learning and communication
        /// by 
        /// imposing reuse through natural selection in context of attrition
        /// or encouraging diversity in context of abundance.
        /// </summary>
        public int SizeConstraint
        {
            get { return sizeConstraint; }
            set { sizeConstraint = value; }
        }
        private int sizeConstraint;

        protected List<SpikingNeuron> neurons;

        public List<SpikingNeuron> Neurons
        {
            get { return neurons; }
        }
    }

    public class Fibre:NeuronFamily
    {
    }
}
