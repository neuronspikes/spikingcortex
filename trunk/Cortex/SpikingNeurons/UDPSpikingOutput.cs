using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace SpikingNeurons
{
    public class UDPSpikingOutput: OutputFibre
    {
        private int port;
        private string destination;
        UdpClient udpClient;
        Fabric fabric;

        public UDPSpikingOutput(Fabric fabric, string destination, int port)
        {
            this.fabric = fabric;
            this.destination = destination;
            this.port = port;
            udpClient = new UdpClient(destination, port);
        }

        public void transmitAsDeltasUDPStream()
        {
            List<byte> stream = new List<byte>();
            int neuronIndex=0, streamIndex=0;
            foreach( SpikingNeuron n in fabric.Outputs){
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
