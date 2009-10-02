namespace TouchlessDemo
{
    partial class TouchlessDemo
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
            this.buttonCameraProperties = new System.Windows.Forms.Button();
            this.comboBoxCameras = new System.Windows.Forms.ComboBox();
            this.pictureBoxDisplay = new System.Windows.Forms.PictureBox();
            this.groupBoxCamera = new System.Windows.Forms.GroupBox();
            this.FDrops = new System.Windows.Forms.Label();
            this.FrameCount = new System.Windows.Forms.Label();
            this.FDropsLabel = new System.Windows.Forms.Label();
            this.groupBoxCameraInfo = new System.Windows.Forms.GroupBox();
            this.checkBoxCameraFlipY = new System.Windows.Forms.CheckBox();
            this.checkBoxCameraFlipX = new System.Windows.Forms.CheckBox();
            this.checkBoxCameraFPSLimit = new System.Windows.Forms.CheckBox();
            this.labelCameraFPSValue = new System.Windows.Forms.Label();
            this.numericUpDownCameraFPSLimit = new System.Windows.Forms.NumericUpDown();
            this.labelCameraFPS = new System.Windows.Forms.Label();
            this.radioButtonCamera = new System.Windows.Forms.RadioButton();
            this.groupBoxMarkers = new System.Windows.Forms.GroupBox();
            this.groupBoxMarkerControl = new System.Windows.Forms.GroupBox();
            this.numericUpDownMarkerThresh = new System.Windows.Forms.NumericUpDown();
            this.labelMarkerThresh = new System.Windows.Forms.Label();
            this.checkBoxMarkerSmoothing = new System.Windows.Forms.CheckBox();
            this.checkBoxMarkerHighlight = new System.Windows.Forms.CheckBox();
            this.labelMarkerData = new System.Windows.Forms.RichTextBox();
            this.buttonMarkerRemove = new System.Windows.Forms.Button();
            this.comboBoxMarkers = new System.Windows.Forms.ComboBox();
            this.buttonMarkerAdd = new System.Windows.Forms.Button();
            this.groupBoxDemo = new System.Windows.Forms.GroupBox();
            this.labelDemoInstructions = new System.Windows.Forms.RichTextBox();
            this.buttonDefendDemo = new System.Windows.Forms.Button();
            this.buttonImageDemo = new System.Windows.Forms.Button();
            this.buttonSnakeDemo = new System.Windows.Forms.Button();
            this.buttonDrawDemo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).BeginInit();
            this.groupBoxCamera.SuspendLayout();
            this.groupBoxCameraInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraFPSLimit)).BeginInit();
            this.groupBoxMarkers.SuspendLayout();
            this.groupBoxMarkerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarkerThresh)).BeginInit();
            this.groupBoxDemo.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCameraProperties
            // 
            this.buttonCameraProperties.Location = new System.Drawing.Point(9, 19);
            this.buttonCameraProperties.Name = "buttonCameraProperties";
            this.buttonCameraProperties.Size = new System.Drawing.Size(137, 23);
            this.buttonCameraProperties.TabIndex = 17;
            this.buttonCameraProperties.Text = "Adjust Camera Properties";
            this.buttonCameraProperties.UseVisualStyleBackColor = true;
            this.buttonCameraProperties.Click += new System.EventHandler(this.buttonCameraProperties_Click);
            // 
            // comboBoxCameras
            // 
            this.comboBoxCameras.FormattingEnabled = true;
            this.comboBoxCameras.Location = new System.Drawing.Point(10, 19);
            this.comboBoxCameras.Name = "comboBoxCameras";
            this.comboBoxCameras.Size = new System.Drawing.Size(304, 21);
            this.comboBoxCameras.TabIndex = 14;
            this.comboBoxCameras.Text = "Select A Camera";
            this.comboBoxCameras.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameras_SelectedIndexChanged);
            this.comboBoxCameras.DropDown += new System.EventHandler(this.comboBoxCameras_DropDown);
            // 
            // pictureBoxDisplay
            // 
            this.pictureBoxDisplay.BackColor = System.Drawing.Color.Red;
            this.pictureBoxDisplay.Location = new System.Drawing.Point(338, 12);
            this.pictureBoxDisplay.Name = "pictureBoxDisplay";
            this.pictureBoxDisplay.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxDisplay.TabIndex = 19;
            this.pictureBoxDisplay.TabStop = false;
            this.pictureBoxDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseMove);
            this.pictureBoxDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseDown);
            this.pictureBoxDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxDisplay_MouseUp);
            // 
            // groupBoxCamera
            // 
            this.groupBoxCamera.Controls.Add(this.FDrops);
            this.groupBoxCamera.Controls.Add(this.FrameCount);
            this.groupBoxCamera.Controls.Add(this.FDropsLabel);
            this.groupBoxCamera.Controls.Add(this.groupBoxCameraInfo);
            this.groupBoxCamera.Controls.Add(this.comboBoxCameras);
            this.groupBoxCamera.Location = new System.Drawing.Point(12, 59);
            this.groupBoxCamera.Name = "groupBoxCamera";
            this.groupBoxCamera.Size = new System.Drawing.Size(320, 390);
            this.groupBoxCamera.TabIndex = 20;
            this.groupBoxCamera.TabStop = false;
            this.groupBoxCamera.Text = "Camera Settings";
            // 
            // FDrops
            // 
            this.FDrops.AutoSize = true;
            this.FDrops.Location = new System.Drawing.Point(162, 187);
            this.FDrops.Name = "FDrops";
            this.FDrops.Size = new System.Drawing.Size(28, 13);
            this.FDrops.TabIndex = 24;
            this.FDrops.Text = "0.00";
            this.FDrops.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // FrameCount
            // 
            this.FrameCount.AutoSize = true;
            this.FrameCount.Location = new System.Drawing.Point(164, 215);
            this.FrameCount.Name = "FrameCount";
            this.FrameCount.Size = new System.Drawing.Size(13, 13);
            this.FrameCount.TabIndex = 21;
            this.FrameCount.Text = "0";
            this.FrameCount.Click += new System.EventHandler(this.label1_Click);
            // 
            // FDropsLabel
            // 
            this.FDropsLabel.AutoSize = true;
            this.FDropsLabel.Location = new System.Drawing.Point(15, 187);
            this.FDropsLabel.Name = "FDropsLabel";
            this.FDropsLabel.Size = new System.Drawing.Size(105, 13);
            this.FDropsLabel.TabIndex = 23;
            this.FDropsLabel.Text = "Current Frame drops:";
            this.FDropsLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBoxCameraInfo
            // 
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFlipY);
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFlipX);
            this.groupBoxCameraInfo.Controls.Add(this.checkBoxCameraFPSLimit);
            this.groupBoxCameraInfo.Controls.Add(this.labelCameraFPSValue);
            this.groupBoxCameraInfo.Controls.Add(this.numericUpDownCameraFPSLimit);
            this.groupBoxCameraInfo.Controls.Add(this.labelCameraFPS);
            this.groupBoxCameraInfo.Controls.Add(this.buttonCameraProperties);
            this.groupBoxCameraInfo.Enabled = false;
            this.groupBoxCameraInfo.Location = new System.Drawing.Point(9, 48);
            this.groupBoxCameraInfo.Name = "groupBoxCameraInfo";
            this.groupBoxCameraInfo.Size = new System.Drawing.Size(305, 116);
            this.groupBoxCameraInfo.TabIndex = 20;
            this.groupBoxCameraInfo.TabStop = false;
            this.groupBoxCameraInfo.Text = "No Camera Selected";
            // 
            // checkBoxCameraFlipY
            // 
            this.checkBoxCameraFlipY.AutoSize = true;
            this.checkBoxCameraFlipY.Location = new System.Drawing.Point(155, 93);
            this.checkBoxCameraFlipY.Name = "checkBoxCameraFlipY";
            this.checkBoxCameraFlipY.Size = new System.Drawing.Size(119, 17);
            this.checkBoxCameraFlipY.TabIndex = 22;
            this.checkBoxCameraFlipY.Text = "Flip Image Vertically";
            this.checkBoxCameraFlipY.UseVisualStyleBackColor = true;
            this.checkBoxCameraFlipY.CheckedChanged += new System.EventHandler(this.checkBoxCameraFlip_CheckedChanged);
            // 
            // checkBoxCameraFlipX
            // 
            this.checkBoxCameraFlipX.AutoSize = true;
            this.checkBoxCameraFlipX.Location = new System.Drawing.Point(9, 93);
            this.checkBoxCameraFlipX.Name = "checkBoxCameraFlipX";
            this.checkBoxCameraFlipX.Size = new System.Drawing.Size(131, 17);
            this.checkBoxCameraFlipX.TabIndex = 22;
            this.checkBoxCameraFlipX.Text = "Flip Image Horizontally";
            this.checkBoxCameraFlipX.UseVisualStyleBackColor = true;
            this.checkBoxCameraFlipX.CheckedChanged += new System.EventHandler(this.checkBoxCameraFlip_CheckedChanged);
            // 
            // checkBoxCameraFPSLimit
            // 
            this.checkBoxCameraFPSLimit.AutoSize = true;
            this.checkBoxCameraFPSLimit.Location = new System.Drawing.Point(9, 70);
            this.checkBoxCameraFPSLimit.Name = "checkBoxCameraFPSLimit";
            this.checkBoxCameraFPSLimit.Size = new System.Drawing.Size(143, 17);
            this.checkBoxCameraFPSLimit.TabIndex = 21;
            this.checkBoxCameraFPSLimit.Text = "Limit Frames Per Second";
            this.checkBoxCameraFPSLimit.UseVisualStyleBackColor = true;
            this.checkBoxCameraFPSLimit.CheckedChanged += new System.EventHandler(this.checkBoxCameraFPSLimit_CheckedChanged);
            // 
            // labelCameraFPSValue
            // 
            this.labelCameraFPSValue.AutoSize = true;
            this.labelCameraFPSValue.Location = new System.Drawing.Point(153, 49);
            this.labelCameraFPSValue.Name = "labelCameraFPSValue";
            this.labelCameraFPSValue.Size = new System.Drawing.Size(28, 13);
            this.labelCameraFPSValue.TabIndex = 20;
            this.labelCameraFPSValue.Text = "0.00";
            // 
            // numericUpDownCameraFPSLimit
            // 
            this.numericUpDownCameraFPSLimit.Enabled = false;
            this.numericUpDownCameraFPSLimit.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Location = new System.Drawing.Point(156, 68);
            this.numericUpDownCameraFPSLimit.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Name = "numericUpDownCameraFPSLimit";
            this.numericUpDownCameraFPSLimit.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownCameraFPSLimit.TabIndex = 19;
            this.numericUpDownCameraFPSLimit.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownCameraFPSLimit.Visible = false;
            this.numericUpDownCameraFPSLimit.ValueChanged += new System.EventHandler(this.numericUpDownCameraFPSLimit_ValueChanged);
            // 
            // labelCameraFPS
            // 
            this.labelCameraFPS.AutoSize = true;
            this.labelCameraFPS.Location = new System.Drawing.Point(6, 49);
            this.labelCameraFPS.Name = "labelCameraFPS";
            this.labelCameraFPS.Size = new System.Drawing.Size(140, 13);
            this.labelCameraFPS.TabIndex = 0;
            this.labelCameraFPS.Text = "Current Frames Per Second:";
            // 
            // radioButtonCamera
            // 
            this.radioButtonCamera.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonCamera.AutoSize = true;
            this.radioButtonCamera.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonCamera.Location = new System.Drawing.Point(12, 12);
            this.radioButtonCamera.Name = "radioButtonCamera";
            this.radioButtonCamera.Size = new System.Drawing.Size(101, 39);
            this.radioButtonCamera.TabIndex = 21;
            this.radioButtonCamera.TabStop = true;
            this.radioButtonCamera.Text = "Camera";
            this.radioButtonCamera.UseVisualStyleBackColor = true;
            this.radioButtonCamera.CheckedChanged += new System.EventHandler(this.radioButtonMode_CheckedChanged);
            // 
            // groupBoxMarkers
            // 
            this.groupBoxMarkers.Controls.Add(this.groupBoxMarkerControl);
            this.groupBoxMarkers.Controls.Add(this.comboBoxMarkers);
            this.groupBoxMarkers.Controls.Add(this.buttonMarkerAdd);
            this.groupBoxMarkers.Location = new System.Drawing.Point(12, 59);
            this.groupBoxMarkers.Name = "groupBoxMarkers";
            this.groupBoxMarkers.Size = new System.Drawing.Size(320, 390);
            this.groupBoxMarkers.TabIndex = 21;
            this.groupBoxMarkers.TabStop = false;
            this.groupBoxMarkers.Text = "Marker Settings";
            // 
            // groupBoxMarkerControl
            // 
            this.groupBoxMarkerControl.Controls.Add(this.numericUpDownMarkerThresh);
            this.groupBoxMarkerControl.Controls.Add(this.labelMarkerThresh);
            this.groupBoxMarkerControl.Controls.Add(this.checkBoxMarkerSmoothing);
            this.groupBoxMarkerControl.Controls.Add(this.checkBoxMarkerHighlight);
            this.groupBoxMarkerControl.Controls.Add(this.labelMarkerData);
            this.groupBoxMarkerControl.Controls.Add(this.buttonMarkerRemove);
            this.groupBoxMarkerControl.Enabled = false;
            this.groupBoxMarkerControl.Location = new System.Drawing.Point(10, 48);
            this.groupBoxMarkerControl.Name = "groupBoxMarkerControl";
            this.groupBoxMarkerControl.Size = new System.Drawing.Size(304, 216);
            this.groupBoxMarkerControl.TabIndex = 25;
            this.groupBoxMarkerControl.TabStop = false;
            this.groupBoxMarkerControl.Text = "No Marker Selected";
            // 
            // numericUpDownMarkerThresh
            // 
            this.numericUpDownMarkerThresh.Location = new System.Drawing.Point(251, 44);
            this.numericUpDownMarkerThresh.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMarkerThresh.Name = "numericUpDownMarkerThresh";
            this.numericUpDownMarkerThresh.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownMarkerThresh.TabIndex = 5;
            this.numericUpDownMarkerThresh.ValueChanged += new System.EventHandler(this.numericUpDownMarkerThresh_ValueChanged);
            // 
            // labelMarkerThresh
            // 
            this.labelMarkerThresh.AutoSize = true;
            this.labelMarkerThresh.Location = new System.Drawing.Point(152, 46);
            this.labelMarkerThresh.Name = "labelMarkerThresh";
            this.labelMarkerThresh.Size = new System.Drawing.Size(93, 13);
            this.labelMarkerThresh.TabIndex = 4;
            this.labelMarkerThresh.Text = "Marker Threshold:";
            // 
            // checkBoxMarkerSmoothing
            // 
            this.checkBoxMarkerSmoothing.AutoSize = true;
            this.checkBoxMarkerSmoothing.Checked = true;
            this.checkBoxMarkerSmoothing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarkerSmoothing.Location = new System.Drawing.Point(6, 45);
            this.checkBoxMarkerSmoothing.Name = "checkBoxMarkerSmoothing";
            this.checkBoxMarkerSmoothing.Size = new System.Drawing.Size(124, 17);
            this.checkBoxMarkerSmoothing.TabIndex = 3;
            this.checkBoxMarkerSmoothing.Text = "Smooth Marker Data";
            this.checkBoxMarkerSmoothing.UseVisualStyleBackColor = true;
            this.checkBoxMarkerSmoothing.CheckedChanged += new System.EventHandler(this.checkBoxMarkerSmoothing_CheckedChanged);
            // 
            // checkBoxMarkerHighlight
            // 
            this.checkBoxMarkerHighlight.AutoSize = true;
            this.checkBoxMarkerHighlight.Checked = true;
            this.checkBoxMarkerHighlight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarkerHighlight.Location = new System.Drawing.Point(5, 19);
            this.checkBoxMarkerHighlight.Name = "checkBoxMarkerHighlight";
            this.checkBoxMarkerHighlight.Size = new System.Drawing.Size(103, 17);
            this.checkBoxMarkerHighlight.TabIndex = 2;
            this.checkBoxMarkerHighlight.Text = "Highlight Marker";
            this.checkBoxMarkerHighlight.UseVisualStyleBackColor = true;
            this.checkBoxMarkerHighlight.CheckedChanged += new System.EventHandler(this.checkBoxMarkerHighlight_CheckedChanged);
            // 
            // labelMarkerData
            // 
            this.labelMarkerData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMarkerData.Location = new System.Drawing.Point(7, 70);
            this.labelMarkerData.Name = "labelMarkerData";
            this.labelMarkerData.ReadOnly = true;
            this.labelMarkerData.Size = new System.Drawing.Size(291, 140);
            this.labelMarkerData.TabIndex = 1;
            this.labelMarkerData.Text = "";
            // 
            // buttonMarkerRemove
            // 
            this.buttonMarkerRemove.Location = new System.Drawing.Point(155, 15);
            this.buttonMarkerRemove.Name = "buttonMarkerRemove";
            this.buttonMarkerRemove.Size = new System.Drawing.Size(143, 23);
            this.buttonMarkerRemove.TabIndex = 0;
            this.buttonMarkerRemove.Text = "Remove This Marker";
            this.buttonMarkerRemove.UseVisualStyleBackColor = true;
            this.buttonMarkerRemove.Click += new System.EventHandler(this.buttonMarkerRemove_Click);
            // 
            // comboBoxMarkers
            // 
            this.comboBoxMarkers.Enabled = false;
            this.comboBoxMarkers.FormattingEnabled = true;
            this.comboBoxMarkers.Location = new System.Drawing.Point(165, 19);
            this.comboBoxMarkers.Name = "comboBoxMarkers";
            this.comboBoxMarkers.Size = new System.Drawing.Size(148, 21);
            this.comboBoxMarkers.TabIndex = 22;
            this.comboBoxMarkers.Text = "Edit An Existing Marker";
            this.comboBoxMarkers.SelectedIndexChanged += new System.EventHandler(this.comboBoxMarkers_SelectedIndexChanged);
            this.comboBoxMarkers.DropDown += new System.EventHandler(this.comboBoxMarkers_DropDown);
            // 
            // buttonMarkerAdd
            // 
            this.buttonMarkerAdd.Location = new System.Drawing.Point(10, 17);
            this.buttonMarkerAdd.Name = "buttonMarkerAdd";
            this.buttonMarkerAdd.Size = new System.Drawing.Size(151, 23);
            this.buttonMarkerAdd.TabIndex = 19;
            this.buttonMarkerAdd.Text = "Add A New Marker";
            this.buttonMarkerAdd.UseVisualStyleBackColor = true;
            this.buttonMarkerAdd.Click += new System.EventHandler(this.buttonMarkerAdd_Click);
            // 
            // groupBoxDemo
            // 
            this.groupBoxDemo.Controls.Add(this.labelDemoInstructions);
            this.groupBoxDemo.Controls.Add(this.buttonDefendDemo);
            this.groupBoxDemo.Controls.Add(this.buttonImageDemo);
            this.groupBoxDemo.Controls.Add(this.buttonSnakeDemo);
            this.groupBoxDemo.Controls.Add(this.buttonDrawDemo);
            this.groupBoxDemo.Location = new System.Drawing.Point(12, 59);
            this.groupBoxDemo.Name = "groupBoxDemo";
            this.groupBoxDemo.Size = new System.Drawing.Size(320, 390);
            this.groupBoxDemo.TabIndex = 26;
            this.groupBoxDemo.TabStop = false;
            this.groupBoxDemo.Text = "Demo Mode Instructions";
            // 
            // labelDemoInstructions
            // 
            this.labelDemoInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.labelDemoInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDemoInstructions.Enabled = false;
            this.labelDemoInstructions.Location = new System.Drawing.Point(6, 108);
            this.labelDemoInstructions.Name = "labelDemoInstructions";
            this.labelDemoInstructions.ReadOnly = true;
            this.labelDemoInstructions.Size = new System.Drawing.Size(308, 276);
            this.labelDemoInstructions.TabIndex = 24;
            this.labelDemoInstructions.Text = "";
            // 
            // buttonDefendDemo
            // 
            this.buttonDefendDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonDefendDemo.Location = new System.Drawing.Point(164, 63);
            this.buttonDefendDemo.Name = "buttonDefendDemo";
            this.buttonDefendDemo.Size = new System.Drawing.Size(150, 38);
            this.buttonDefendDemo.TabIndex = 23;
            this.buttonDefendDemo.Text = "Start Defend Demo";
            this.buttonDefendDemo.UseVisualStyleBackColor = true;
            // 
            // buttonImageDemo
            // 
            this.buttonImageDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonImageDemo.Location = new System.Drawing.Point(164, 19);
            this.buttonImageDemo.Name = "buttonImageDemo";
            this.buttonImageDemo.Size = new System.Drawing.Size(150, 38);
            this.buttonImageDemo.TabIndex = 22;
            this.buttonImageDemo.Text = "Start Image Demo";
            this.buttonImageDemo.UseVisualStyleBackColor = true;
            this.buttonImageDemo.Click += new System.EventHandler(this.buttonImageDemo_Click);
            // 
            // buttonSnakeDemo
            // 
            this.buttonSnakeDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonSnakeDemo.Location = new System.Drawing.Point(6, 63);
            this.buttonSnakeDemo.Name = "buttonSnakeDemo";
            this.buttonSnakeDemo.Size = new System.Drawing.Size(150, 38);
            this.buttonSnakeDemo.TabIndex = 21;
            this.buttonSnakeDemo.Text = "Start Snake Demo";
            this.buttonSnakeDemo.UseVisualStyleBackColor = true;
            // 
            // buttonDrawDemo
            // 
            this.buttonDrawDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonDrawDemo.Location = new System.Drawing.Point(6, 19);
            this.buttonDrawDemo.Name = "buttonDrawDemo";
            this.buttonDrawDemo.Size = new System.Drawing.Size(150, 38);
            this.buttonDrawDemo.TabIndex = 21;
            this.buttonDrawDemo.Text = "Start Draw Demo";
            this.buttonDrawDemo.UseVisualStyleBackColor = true;
            this.buttonDrawDemo.Click += new System.EventHandler(this.buttonDrawDemo_Click);
            // 
            // TouchlessDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 503);
            this.Controls.Add(this.radioButtonCamera);
            this.Controls.Add(this.pictureBoxDisplay);
            this.Controls.Add(this.groupBoxCamera);
            this.Controls.Add(this.groupBoxDemo);
            this.Controls.Add(this.groupBoxMarkers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TouchlessDemo";
            this.Text = "Touchless Demo";
            this.Load += new System.EventHandler(this.TouchlessDemo_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TouchlessDemo_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).EndInit();
            this.groupBoxCamera.ResumeLayout(false);
            this.groupBoxCamera.PerformLayout();
            this.groupBoxCameraInfo.ResumeLayout(false);
            this.groupBoxCameraInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraFPSLimit)).EndInit();
            this.groupBoxMarkers.ResumeLayout(false);
            this.groupBoxMarkerControl.ResumeLayout(false);
            this.groupBoxMarkerControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarkerThresh)).EndInit();
            this.groupBoxDemo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCameraProperties;
        private System.Windows.Forms.ComboBox comboBoxCameras;
        private System.Windows.Forms.PictureBox pictureBoxDisplay;
        private System.Windows.Forms.GroupBox groupBoxCamera;
        private System.Windows.Forms.RadioButton radioButtonCamera;
        private System.Windows.Forms.GroupBox groupBoxMarkers;
        private System.Windows.Forms.Button buttonMarkerAdd;
        private System.Windows.Forms.ComboBox comboBoxMarkers;
        private System.Windows.Forms.GroupBox groupBoxMarkerControl;
        private System.Windows.Forms.RichTextBox labelMarkerData;
        private System.Windows.Forms.Button buttonMarkerRemove;
        private System.Windows.Forms.GroupBox groupBoxCameraInfo;
        private System.Windows.Forms.Label labelCameraFPS;
        private System.Windows.Forms.GroupBox groupBoxDemo;
        private System.Windows.Forms.CheckBox checkBoxMarkerHighlight;
        private System.Windows.Forms.CheckBox checkBoxMarkerSmoothing;
        private System.Windows.Forms.Label labelMarkerThresh;
        private System.Windows.Forms.NumericUpDown numericUpDownCameraFPSLimit;
        private System.Windows.Forms.NumericUpDown numericUpDownMarkerThresh;
        private System.Windows.Forms.Button buttonDrawDemo;
        private System.Windows.Forms.Button buttonSnakeDemo;
        private System.Windows.Forms.Button buttonImageDemo;
        private System.Windows.Forms.Label labelCameraFPSValue;
        private System.Windows.Forms.CheckBox checkBoxCameraFPSLimit;
        private System.Windows.Forms.Button buttonDefendDemo;
        private System.Windows.Forms.RichTextBox labelDemoInstructions;
        private System.Windows.Forms.CheckBox checkBoxCameraFlipY;
        private System.Windows.Forms.CheckBox checkBoxCameraFlipX;
        private System.Windows.Forms.Label FrameCount;
        private System.Windows.Forms.Label FDrops;
        private System.Windows.Forms.Label FDropsLabel;
    }
}

