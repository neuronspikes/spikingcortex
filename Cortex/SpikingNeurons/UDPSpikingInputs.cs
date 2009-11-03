using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace SpikingNeurons
{
    public class UDPSpikingInputs : InputFibre
    {
        UdpClient udpClient;
        IPEndPoint RemoteIpEndPoint;
        int lastCount;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public UDPSpikingInputs(string name, int size, int port)
        {
            this.size = size;
            this.name = name;
            this.spikingThings = new List<SpikingThing>();

            for (int i = 0; i < size; i++)
            {
                spikingThings.Add(new SpikingThing());
            }

            //IPEndPoint object will allow us to read datagrams sent from any source.
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);
            // Receive a message and write it to the console.
            udpClient = new UdpClient(RemoteIpEndPoint);
        }


        public void BeginReception()
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), this);
        }

        public static bool messageReceived = false;

        public static void ReceiveCallback(IAsyncResult ar)
        {


            UDPSpikingInputs usi = (UDPSpikingInputs)(ar.AsyncState);
            UdpClient u = usi.udpClient;
            IPEndPoint e = usi.RemoteIpEndPoint;

            Byte[] receiveBytes = u.EndReceive(ar, ref e);

            usi.interpretStreamAsPowers(receiveBytes);

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
                    adrs += code;
                    if (adrs < size)
                        SpikingThings[adrs].doSpikeMethod(); // trigger spike
                    count++;
                }
            }
        }
        public void interpretStreamAs8bitAdresses(Byte[] receiveBytes)
        {
            // interpret data as 8 bit spike adress (intensive action 0-255 range)
            foreach (byte code in receiveBytes)
            {
                SpikingThing t = SpikingThings[code];

                t.doSpikeMethod(); // trigger spike
            }
        }

        public void interpretStreamAsPowers(Byte[] receiveBytes)
        {
            // interpret stream as intensity => number of spikes per frame (very intensive action on all SpikingThings)
            int adrs = 0;
            int maxAdrs = SpikingThings.Count;
            foreach (byte code in receiveBytes)
            {
                if (adrs < maxAdrs)
                {
                    SpikingThing t = SpikingThings[adrs++];

                    for (byte pow = 0; pow < code; pow++)
                        t.doSpikeMethod(); // trigger spike
                }
            }
        }
    }

}
