namespace TripleAGameCreator
{
    partial class Automatic_Connection_Finder
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.toolStripProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.increaseAccuracy = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addPointsBeforeRunning = new System.Windows.Forms.CheckBox();
            this.onlyAddPointsToSeaZones = new System.Windows.Forms.CheckBox();
            this.checkPolygonBounds = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Line Width:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 22);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(82, 18);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Location = new System.Drawing.Point(95, 134);
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(297, 22);
            this.toolStripProgressBar1.TabIndex = 5;
            // 
            // increaseAccuracy
            // 
            this.increaseAccuracy.AutoSize = true;
            this.increaseAccuracy.Location = new System.Drawing.Point(152, 19);
            this.increaseAccuracy.Name = "increaseAccuracy";
            this.increaseAccuracy.Size = new System.Drawing.Size(227, 17);
            this.increaseAccuracy.TabIndex = 6;
            this.increaseAccuracy.Text = "Increase Accuracy By Adding Points(Slow)";
            this.increaseAccuracy.UseVisualStyleBackColor = true;
            this.increaseAccuracy.CheckedChanged += new System.EventHandler(this.increaseAccuracy_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.addPointsBeforeRunning);
            this.groupBox1.Controls.Add(this.checkPolygonBounds);
            this.groupBox1.Controls.Add(this.onlyAddPointsToSeaZones);
            this.groupBox1.Controls.Add(this.increaseAccuracy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 118);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // addPointsBeforeRunning
            // 
            this.addPointsBeforeRunning.AutoSize = true;
            this.addPointsBeforeRunning.Enabled = false;
            this.addPointsBeforeRunning.Location = new System.Drawing.Point(17, 44);
            this.addPointsBeforeRunning.Name = "addPointsBeforeRunning";
            this.addPointsBeforeRunning.Size = new System.Drawing.Size(348, 17);
            this.addPointsBeforeRunning.TabIndex = 6;
            this.addPointsBeforeRunning.Text = "Add Points Before Running To Ensure Finding All Connections(Slow)";
            this.addPointsBeforeRunning.UseVisualStyleBackColor = true;
            // 
            // onlyAddPointsToSeaZones
            // 
            this.onlyAddPointsToSeaZones.AutoSize = true;
            this.onlyAddPointsToSeaZones.Enabled = false;
            this.onlyAddPointsToSeaZones.Location = new System.Drawing.Point(17, 67);
            this.onlyAddPointsToSeaZones.Name = "onlyAddPointsToSeaZones";
            this.onlyAddPointsToSeaZones.Size = new System.Drawing.Size(248, 17);
            this.onlyAddPointsToSeaZones.TabIndex = 6;
            this.onlyAddPointsToSeaZones.Text = "Only Add Polygon Points To Sea Zones(Faster)";
            this.onlyAddPointsToSeaZones.UseVisualStyleBackColor = true;
            // 
            // checkPolygonBounds
            // 
            this.checkPolygonBounds.AutoSize = true;
            this.checkPolygonBounds.Location = new System.Drawing.Point(17, 90);
            this.checkPolygonBounds.Name = "checkPolygonBounds";
            this.checkPolygonBounds.Size = new System.Drawing.Size(312, 17);
            this.checkPolygonBounds.TabIndex = 6;
            this.checkPolygonBounds.Text = "Check Polygon Bounds Intersection Before Scanning(Faster)";
            this.checkPolygonBounds.UseVisualStyleBackColor = true;
            // 
            // Automatic_Connection_Finder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 163);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStripProgressBar1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Automatic_Connection_Finder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Automatic Connection Finder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Automatic_Connection_Finder_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox increaseAccuracy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox addPointsBeforeRunning;
        private System.Windows.Forms.CheckBox onlyAddPointsToSeaZones;
        private System.Windows.Forms.CheckBox checkPolygonBounds;
    }
}