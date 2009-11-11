using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingNeurons
{
    public class OutputFibre : Fibre
    {
        delegate void processOutput(List<SpikingNeuron> output);
    }
}
