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
        public Form1()
        {
            InitializeComponent();
            i2s = new Image2Spikes(pictureBox1, Program.udpClient);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            i2s.SendImage();
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
