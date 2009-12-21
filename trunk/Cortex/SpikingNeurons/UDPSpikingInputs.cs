using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace SpikingNeurons
{
    public class UDPSpikingInputs : Fibre
    {
        UdpClient udpClient;
        IPEndPoint RemoteIpEndPoint;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        private double spikeWeight;

        public double SpikeWeight
        {
            get { return spikeWeight; }
            set { spikeWeight = value; }
        }

        public UDPSpikingInputs(string name, int size, int port)
        {
            this.size = size;
            this.name = name;
            neurons = new List<SpikingNeuron>();
            for (int i = 0; i < size; i++)
            {
                lock(neurons) neurons.Add(new SpikingNeuron(this));
            }

            //IPEndPoint object will allow us to read datagrams sent from any source.
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);
            // Receive a message and write it to the console.
            udpClient = new UdpClient(RemoteIpEndPoint);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this);
        }

        public static bool messageReceived = false;

        public static void ReceiveCallback(IAsyncResult ar)
        {


            UDPSpikingInputs usi = (UDPSpikingInputs)(ar.AsyncState);
            UdpClient u = usi.udpClient;
            IPEndPoint e = usi.RemoteIpEndPoint;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);

            usi.interpretStreamAsDeltas(receiveBytes);

            // Prepare for next shot
            usi.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), usi);
        }

        public void interpretStreamAsDeltas(Byte[] receiveBytes)
        {
            int count = 0;
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
                    count++;
                }
            }
        }
        public void interpretStreamAs8bitAdresses(Byte[] receiveBytes)
        {
            // interpret data as 8 bit spike adress (intensive action 0-255 range)
            foreach (byte code in receiveBytes)
            {
                SpikingNeuron t = neurons[code];

                t.addCharge(spikeWeight); // trigger spike
            }
        }

        public void interpretStreamAsPowers(Byte[] receiveBytes)
        {
            // interpret stream as intensity => number of spikes per frame (very intensive action on all SpikingThings)
            int adrs = 0;
            int maxAdrs = neurons.Count;
            foreach (byte code in receiveBytes)
            {
                if (adrs < maxAdrs)
                {
                    SpikingNeuron t = neurons[adrs++];

                    for (byte pow = 0; pow < code; pow++)
                        t.addCharge(spikeWeight); // trigger spike
                }
            }
        }
    }

}
