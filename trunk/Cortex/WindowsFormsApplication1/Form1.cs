using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Image2Spikes i2s;
        Spikes2UDP s2u;
        System.Drawing.Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
            i2s = new Image2Spikes();
            s2u = new Spikes2UDP("127.0.0.1", 8000, 12000, 12000 + 8);

            bitmap = new System.Drawing.Bitmap(pictureBox1.InitialImage);

            timer1.Start();
            s2u.StartTransmission();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            s2u.transmitOrderedSpikesAsDeltasUDPStream(i2s.getSpikesFromImage(bitmap));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
        }
    }
}
