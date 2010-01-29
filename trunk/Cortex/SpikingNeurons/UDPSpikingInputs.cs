using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;

namespace SpikingNeurons
{
    [DataContract(Name = "UDPInput", Namespace = "http://model.NeuronSpikes.org")]
    public class UDPSpikingInputs : Fibre
    {
        [DataMember]
        int neuronsPerPort;
        
        [DataMember]
        int lowerPort;

        [DataMember]
        int higherPort;

        private List<UDPReceivePortHandler> portHandlers;

        [DataMember]
        private double spikeWeight;
        public double SpikeWeight
        {
            get { return spikeWeight; }
            set { spikeWeight = value; }
        }

        public UDPSpikingInputs(string name, int size,double spikeWeight, int neuronsPerPort, int lowerPort,int higherPort)
        {
            //validate sizing
            int fibreRepresented = (higherPort - lowerPort) * neuronsPerPort;

            if (fibreRepresented < size) Console.WriteLine("WARNING : Fibre " + this.Name + " on ports " + lowerPort + "-" + higherPort + " have " + (size - fibreRepresented) + " unused input neurons.");
            if (fibreRepresented > size) Console.WriteLine("WARNING : Fibre " + this.Name + " on ports " + lowerPort + "-" + higherPort + " may receive unmapped spikes. " + (fibreRepresented - size) + " input neurons missings.");

            //initialize 
            this.name = name;
            this.size = size;
            this.spikeWeight = spikeWeight;

            neurons = new List<SpikingNeuron>();
            for (int i = 0; i < size; i++)
            {
                lock(neurons) neurons.Add(new SpikingNeuron(this));
            }

            this.neuronsPerPort = neuronsPerPort;
            this.lowerPort = lowerPort;
            this.higherPort = higherPort;

            StartReception();
        }

        public void StartReception()
        {
            if (portHandlers == null)
            {
                portHandlers = new List<UDPReceivePortHandler>();
                int neuronOffset = 0;
                for (int port = lowerPort; port <= higherPort; port++)
                {
                    portHandlers.Add(new UDPReceivePortHandler(port, neuronOffset, this));
                    neuronOffset += neuronsPerPort;
                }
            }
            foreach (UDPReceivePortHandler ph in portHandlers) ph.Start();
        }

        public void StopReception()
        {
            foreach(UDPReceivePortHandler ph in portHandlers) ph.Stop();
        }

        public void interpretStreamAsDeltas(int offset, Byte[] receiveBytes)
        {
            int adrs = 0;
            // interpret data as spike adress deltas
            foreach (byte code in receiveBytes)
            {
                if (code == 0)
                {
                    adrs += 255;
                }
                else // there is a spike
                {
                    adrs += code; // since zero have a meaning...
                    if (adrs < size)
                        neurons[adrs - 1].addCharge(spikeWeight); // trigger spike
                }
            }
        }
        public void interpretStreamAs8bitAdresses(int offset, Byte[] receiveBytes)
        {
            // interpret data as 8 bit spike adress (intensive action 0-255 range)
            foreach (byte code in receiveBytes)
            {
                SpikingNeuron t = neurons[offset+code];

                t.addCharge(spikeWeight); // trigger spike
            }
        }

        public void interpretStreamAsPowers(int offset, Byte[] receiveBytes)
        {
            // interpret stream as intensity => number of spikes per frame (very intensive action on all SpikingThings)
            int adrs = 0;
            int maxAdrs = neurons.Count;
            foreach (byte code in receiveBytes)
            {
                if (adrs < maxAdrs)
                {
                    SpikingNeuron t = neurons[offset + adrs++];

                    for (byte pow = 0; pow < code; pow++)
                        t.addCharge(spikeWeight); // trigger spike
                }
            }
        }
    }

    class UDPReceivePortHandler
    {
        // configuration
        int port;
        int neuronAddressOffset;
        UDPSpikingInputs usi;

        AsyncCallback callBack;
        bool canReceive = false;

        private UdpClient udpClient;
        private IPEndPoint remoteIpEndPoint;

        public UDPReceivePortHandler(int port, int neuronAddressOffset, UDPSpikingInputs usi)
        {
            this.port = port;
            this.neuronAddressOffset = neuronAddressOffset;
            this.usi = usi;

            udpClient = new UdpClient(port);
            remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            callBack = new AsyncCallback(ReceiveCallback);
        }

        public void Start(){
            canReceive = true;
            udpClient.BeginReceive(callBack, this);
        }

        public void Stop()
        {
            canReceive = false;
            udpClient.Close();
        }


        public void ReceiveCallback(IAsyncResult ar)
        {
            UDPReceivePortHandler ph = (UDPReceivePortHandler)(ar.AsyncState);
            if (ph.udpClient != null)
            {
                IPEndPoint e = ph.remoteIpEndPoint;

                try
                {
                    Byte[] receivedBytes = ph.udpClient.EndReceive(ar, ref e);

                    usi.interpretStreamAsDeltas(ph.neuronAddressOffset, receivedBytes);

                    // Prepare for next shot
                    if (ph.canReceive) ph.udpClient.BeginReceive(callBack, ph);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }
    }
}
