using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace WindowsFormsApplication1
{
    [DataContract(Name = "UDPOutput", Namespace = "http://model.NeuronSpikes.org")]
    class Spikes2UDP
    {

        [DataMember]
        string target; // ip address or hostname (multicast not tested)

        [DataMember]
        int neuronsPerPort;

        [DataMember]
        int lowerPort;

        [DataMember]
        int higherPort;

        private List<UDPSendPortHandler> portHandlers;
        bool canTransmit = false;

        public Spikes2UDP(string target, int neuronsPerPort, int lowerPort, int higherPort)
        {
            this.target = target;
            this.neuronsPerPort = neuronsPerPort;
            this.lowerPort = lowerPort;
            this.higherPort = higherPort;
        }

        public void StartTransmission()
        {
            if (portHandlers == null)   // rebuilt here to allow recovery from deserialization and setup change
            {
                portHandlers = new List<UDPSendPortHandler>();
                int neuronOffset = 0;
                for (int port = lowerPort; port <= higherPort; port++)
                {
                    portHandlers.Add(new UDPSendPortHandler(target, port, neuronOffset, neuronsPerPort));
                    neuronOffset += neuronsPerPort;
                }
            }
            foreach (UDPSendPortHandler ph in portHandlers) ph.Start();
            canTransmit = true;
        }

        public void StopTransmission()
        {
            canTransmit = false;
            foreach (UDPSendPortHandler ph in portHandlers) ph.Stop();
        }

        // assumes ordered list
        // TODO: add ordering assertion
        public void transmitOrderedSpikesAsDeltasUDPStream(List<int> spikes)
        {
            if (canTransmit)
            {
                IEnumerator<UDPSendPortHandler> portEnumerator = portHandlers.GetEnumerator();
                
                if(portEnumerator.MoveNext()){
                    UDPSendPortHandler port=portEnumerator.Current;
                    List<byte> stream = new List<byte>();
                    int streamIndex = 0;

                    foreach (int spikeAdrs in spikes)
                    {
                        int offset = (spikeAdrs + 1) - streamIndex;

                        if (offset > (port.NeuronAddressOffset + port.NeuronsPerPort)) // send data and switch port to continue
                        {
                            port.send(stream);
                            if (portEnumerator.MoveNext())
                            {
                                port = portEnumerator.Current;
                                stream = new List<byte>();
                                streamIndex = port.NeuronAddressOffset;
                            }
                            else
                            {
                                int dropped = spikes.Count - (port.NeuronAddressOffset + port.NeuronsPerPort);
                                Console.WriteLine("Insufficient transmission space " + spikes.Count + " spikes dropped.");
                                break;
                            }
                        }

                        while (offset > 255)
                        {
                            stream.Add(0);
                            streamIndex += 255;
                            offset = (spikeAdrs + 1) - streamIndex;

                        }
                        if (offset == 0 || offset > 255) throw new Exception("transmitAsDeltasUDPStream error in streaming algorithm.");
                        if (stream.Count < port.NeuronAddressOffset+port.NeuronsPerPort)
                            stream.Add((byte)offset);
                        else
                        {
                            int dropped = spikes.Count - (port.NeuronAddressOffset+port.NeuronsPerPort);
                            Console.WriteLine("Insufficient transmission space " + spikes.Count + " spikes dropped.");
                            break;
                        }
                        streamIndex += offset;
                    }
                    port.send(stream);
                }
            }
        }
    }

    class UDPSendPortHandler
    {
        // configuration
        string target;
        int port;

        int neuronAddressOffset;
        public int NeuronAddressOffset
        {
            get { return neuronAddressOffset; }
        }

        int neuronsPerPort;
        public int NeuronsPerPort
        {
            get { return neuronsPerPort; }
        }

        bool canTransmit = false;

        private UdpClient udpClient;

        public UDPSendPortHandler(string target, int port, int neuronAddressOffset, int neuronsPerPort )
        {
            this.target = target;
            this.port = port;
            this.neuronAddressOffset = neuronAddressOffset;
            this.neuronsPerPort = neuronsPerPort;
        }

        public void Start()
        {
            udpClient = new UdpClient(target,port);
            if (neuronsPerPort > udpClient.Client.SendBufferSize) // Stop on unsafe port allocation
                throw new Exception("Neurons per port cannot exceed " + udpClient.Client.SendBufferSize+" on this interface.");
            canTransmit = true;
        }

        public void Stop()
        {
            canTransmit = false;
            udpClient.Close();
        }

        public void send(List<byte> stream)
        {
            if (canTransmit) udpClient.Send(stream.ToArray(), stream.Count);
        }
    }
}
