using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace SpikingNeurons
{
    [DataContract(Name = "UDPOutput", Namespace = "http://model.NeuronSpikes.org")]
    public class UDPSpikingOutput
    {
        private int port;
        private string destination;
        UdpClient udpClient;
        Fibre fibre;

        public UDPSpikingOutput(Fibre fabric, string destination, int port)
        {
            this.fibre = fabric;
            this.destination = destination;
            this.port = port;
            udpClient = new UdpClient(destination, port);
        }

        public void transmitAsDeltasUDPStream()
        {
            List<byte> stream = new List<byte>();
            int neuronIndex=0, streamIndex=0;
            foreach( SpikingNeuron n in fibre.Neurons){
                if(n.Spiked){
                    while (neuronIndex < (streamIndex+255))
                    {
                        stream.Add(0);
                        neuronIndex += 255;
                    }
                    int offset = streamIndex - neuronIndex;
                    if (offset == 0 || offset > 255) throw new Exception("transmitAsDeltasUDPStream error in streaming algorithm.");
                    stream.Add((byte)offset);
                    streamIndex += offset;
                }
                
            }
            udpClient.Send(stream.ToArray(),stream.Count);
        }
    }
}
