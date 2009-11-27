using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WindowsFormsApplication1
{
    static class Program
    {
        private static int port;
        private static string destination;
        public static UdpClient udpClient;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            port = 12000;
            destination ="127.0.0.1";
            udpClient = new UdpClient(destination, port);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
