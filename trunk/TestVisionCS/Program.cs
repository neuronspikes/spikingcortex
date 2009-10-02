using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace TestVisionCS
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static Thread workerThread;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 gui = new Form1();
            PictureBox src;
            PictureBox monitor;
            src= gui.getSrcPic();
            monitor = gui.getMonitor();
            SpikeFrameThread workerObject = new SpikeFrameThread(src,monitor);
            workerThread = new Thread(workerObject.DoWork);

            Console.WriteLine("main thread: Starting worker thread...");

            // Loop until SpikeFrameThread activates.
            //while (!workerThread.IsAlive) ;

            // start GUI and live on it

            Application.Run(gui);

            // Request that the SpikeFrameThread to stop itself:
            workerObject.RequestStop();
            //workerThread.Join();
            Console.WriteLine("main thread: Worker thread has terminated.");
        }
    }
}
