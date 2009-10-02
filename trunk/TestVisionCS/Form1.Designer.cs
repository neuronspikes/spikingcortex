namespace TestVisionCS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DstPicH = new System.Windows.Forms.PictureBox();
            this.SrcPic = new System.Windows.Forms.PictureBox();
            this.SeeButton = new System.Windows.Forms.Button();
            this.DstPicB = new System.Windows.Forms.PictureBox();
            this.DstPicBNeg = new System.Windows.Forms.PictureBox();
            this.DstPicS = new System.Windows.Forms.PictureBox();
            this.DstPicSNeg = new System.Windows.Forms.PictureBox();
            this.DstPicHNeg = new System.Windows.Forms.PictureBox();
            this.Monitor = new System.Windows.Forms.PictureBox();
            this.pictureBoxRetina = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicBNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicSNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicHNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Monitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRetina)).BeginInit();
            this.SuspendLayout();
            // 
            // DstPicH
            // 
            this.DstPicH.Location = new System.Drawing.Point(338, 258);
            this.DstPicH.Name = "DstPicH";
            this.DstPicH.Size = new System.Drawing.Size(320, 240);
            this.DstPicH.TabIndex = 0;
            this.DstPicH.TabStop = false;
            // 
            // SrcPic
            // 
            this.SrcPic.Image = ((System.Drawing.Image)(resources.GetObject("SrcPic.Image")));
            this.SrcPic.Location = new System.Drawing.Point(12, 12);
            this.SrcPic.Name = "SrcPic";
            this.SrcPic.Size = new System.Drawing.Size(320, 240);
            this.SrcPic.TabIndex = 1;
            this.SrcPic.TabStop = false;
            // 
            // SeeButton
            // 
            this.SeeButton.Location = new System.Drawing.Point(338, 12);
            this.SeeButton.Name = "SeeButton";
            this.SeeButton.Size = new System.Drawing.Size(75, 23);
            this.SeeButton.TabIndex = 2;
            this.SeeButton.Text = "> See >";
            this.SeeButton.UseVisualStyleBackColor = true;
            this.SeeButton.Click += new System.EventHandler(this.SeeButton_Click);
            // 
            // DstPicB
            // 
            this.DstPicB.Location = new System.Drawing.Point(12, 258);
            this.DstPicB.Name = "DstPicB";
            this.DstPicB.Size = new System.Drawing.Size(320, 240);
            this.DstPicB.TabIndex = 3;
            this.DstPicB.TabStop = false;
            this.DstPicB.Click += new System.EventHandler(this.DstPicB_Click);
            // 
            // DstPicBNeg
            // 
            this.DstPicBNeg.Location = new System.Drawing.Point(12, 504);
            this.DstPicBNeg.Name = "DstPicBNeg";
            this.DstPicBNeg.Size = new System.Drawing.Size(320, 240);
            this.DstPicBNeg.TabIndex = 4;
            this.DstPicBNeg.TabStop = false;
            this.DstPicBNeg.Click += new System.EventHandler(this.DestPicBNeg_Click);
            // 
            // DstPicS
            // 
            this.DstPicS.Location = new System.Drawing.Point(664, 258);
            this.DstPicS.Name = "DstPicS";
            this.DstPicS.Size = new System.Drawing.Size(320, 240);
            this.DstPicS.TabIndex = 5;
            this.DstPicS.TabStop = false;
            // 
            // DstPicSNeg
            // 
            this.DstPicSNeg.Location = new System.Drawing.Point(664, 504);
            this.DstPicSNeg.Name = "DstPicSNeg";
            this.DstPicSNeg.Size = new System.Drawing.Size(320, 240);
            this.DstPicSNeg.TabIndex = 6;
            this.DstPicSNeg.TabStop = false;
            // 
            // DstPicHNeg
            // 
            this.DstPicHNeg.Location = new System.Drawing.Point(338, 504);
            this.DstPicHNeg.Name = "DstPicHNeg";
            this.DstPicHNeg.Size = new System.Drawing.Size(320, 240);
            this.DstPicHNeg.TabIndex = 7;
            this.DstPicHNeg.TabStop = false;
            // 
            // Monitor
            // 
            this.Monitor.Location = new System.Drawing.Point(417, 12);
            this.Monitor.Name = "Monitor";
            this.Monitor.Size = new System.Drawing.Size(320, 240);
            this.Monitor.TabIndex = 8;
            this.Monitor.TabStop = false;
            // 
            // pictureBoxRetina
            // 
            this.pictureBoxRetina.Location = new System.Drawing.Point(743, 12);
            this.pictureBoxRetina.Name = "pictureBoxRetina";
            this.pictureBoxRetina.Size = new System.Drawing.Size(320, 240);
            this.pictureBoxRetina.TabIndex = 9;
            this.pictureBoxRetina.TabStop = false;
            this.pictureBoxRetina.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRetina_MouseMove);
            this.pictureBoxRetina.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBoxRetina.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRetina_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 774);
            this.Controls.Add(this.pictureBoxRetina);
            this.Controls.Add(this.Monitor);
            this.Controls.Add(this.DstPicHNeg);
            this.Controls.Add(this.DstPicSNeg);
            this.Controls.Add(this.DstPicS);
            this.Controls.Add(this.DstPicBNeg);
            this.Controls.Add(this.DstPicB);
            this.Controls.Add(this.SeeButton);
            this.Controls.Add(this.SrcPic);
            this.Controls.Add(this.DstPicH);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DstPicH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicBNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicSNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DstPicHNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Monitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRetina)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox DstPicH;
        private System.Windows.Forms.PictureBox SrcPic;
        private System.Windows.Forms.Button SeeButton;
        private System.Windows.Forms.PictureBox DstPicB;
        private System.Windows.Forms.PictureBox DstPicBNeg;
        private System.Windows.Forms.PictureBox DstPicS;
        private System.Windows.Forms.PictureBox DstPicSNeg;
        private System.Windows.Forms.PictureBox DstPicHNeg;
        private System.Windows.Forms.PictureBox Monitor;
        private System.Windows.Forms.PictureBox pictureBoxRetina;
    }
}

