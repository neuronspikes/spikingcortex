using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SpikingNeurons
{
    class UDPSpikingInputs: InputFibre
    {
        UdpClient udpClient;
        IPEndPoint RemoteIpEndPoint;

        public UDPSpikingInputs(int port, int size)
        {
            udpClient = new UdpClient(port);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        }


        public int processAndSee()
        {
            int count = 0;
            int adrs = 0;

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

            // interpret data as spike adress deltas
            foreach (byte code in receiveBytes){
                if (code == 0)
                {
                    adrs += 255;
                }
                else // there is a spike
                {
                    adrs += code;
                    Set[adrs].doSpikeMethod(); // trigger spike
                    count++;
                }
            }
            return count;
        }
    }
}
