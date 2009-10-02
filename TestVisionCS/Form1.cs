using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;using System.Text;
using System.Windows.Forms;

namespace TestVisionCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SeeButton_Click(object sender, EventArgs e)
        {

            this.DstPicH.Image = Vision.Transform((Bitmap)this.SrcPic.Image, Vision.hue);
            this.DstPicS.Image = Vision.Transform((Bitmap)this.SrcPic.Image, Vision.sat);
            this.DstPicB.Image = Vision.Transform((Bitmap)this.SrcPic.Image, Vision.bright);
            this.DstPicHNeg.Image = Vision.Transform((Bitmap)this.DstPicH.Image, Vision.neg);
            this.DstPicSNeg.Image = Vision.Transform((Bitmap)this.DstPicS.Image, Vision.neg);
            this.DstPicBNeg.Image = Vision.Transform((Bitmap)this.DstPicB.Image, Vision.neg);
            this.pictureBoxRetina.Image = Vision.See((Bitmap)this.SrcPic.Image, 160, 120, 100, 0.0f);

            //Program.workerThread.Start(); start spiking

        }
        public PictureBox getSrcPic()
        {
            return this.SrcPic;
        }
        public PictureBox getMonitor()
        {
            this.Monitor.Image = (Image)this.SrcPic.Image.Clone();
            return this.Monitor;
        }

        private void DestPicBNeg_Click(object sender, EventArgs e)
        {

        }

        private void DstPicB_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBoxRetina_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            this.pictureBoxRetina.Image = Vision.See((Bitmap)this.SrcPic.Image, x, y, 100, 0.0f);


        }

        private void pictureBoxRetina_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            this.pictureBoxRetina.Image = Vision.See((Bitmap)this.SrcPic.Image, x, y, 90, 0.0f);
        }
    }
}
