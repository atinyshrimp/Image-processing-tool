
namespace Interface
{
    partial class HistogramPopUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistogramPopUp));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.redHist = new System.Windows.Forms.RadioButton();
            this.greenHist = new System.Windows.Forms.RadioButton();
            this.blueHist = new System.Windows.Forms.RadioButton();
            this.allHist = new System.Windows.Forms.RadioButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lumHist = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 407);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(362, 339);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.cancelButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.saveButton, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 348);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(454, 56);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cancelButton.Location = new System.Drawing.Point(376, 16);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.saveButton.AutoSize = true;
            this.saveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveButton.Location = new System.Drawing.Point(3, 15);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(118, 25);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save the histogram";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.redHist);
            this.flowLayoutPanel1.Controls.Add(this.greenHist);
            this.flowLayoutPanel1.Controls.Add(this.blueHist);
            this.flowLayoutPanel1.Controls.Add(this.allHist);
            this.flowLayoutPanel1.Controls.Add(this.lumHist);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(371, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(86, 339);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // redHist
            // 
            this.redHist.AutoSize = true;
            this.redHist.Location = new System.Drawing.Point(3, 3);
            this.redHist.Name = "redHist";
            this.redHist.Size = new System.Drawing.Size(45, 19);
            this.redHist.TabIndex = 0;
            this.redHist.TabStop = true;
            this.redHist.Text = "Red";
            this.redHist.UseVisualStyleBackColor = true;
            this.redHist.CheckedChanged += new System.EventHandler(this.redHist_CheckedChanged);
            // 
            // greenHist
            // 
            this.greenHist.AutoSize = true;
            this.greenHist.Location = new System.Drawing.Point(3, 28);
            this.greenHist.Name = "greenHist";
            this.greenHist.Size = new System.Drawing.Size(56, 19);
            this.greenHist.TabIndex = 1;
            this.greenHist.TabStop = true;
            this.greenHist.Text = "Green";
            this.greenHist.UseVisualStyleBackColor = true;
            this.greenHist.CheckedChanged += new System.EventHandler(this.greenHist_CheckedChanged);
            // 
            // blueHist
            // 
            this.blueHist.AutoSize = true;
            this.blueHist.Location = new System.Drawing.Point(3, 53);
            this.blueHist.Name = "blueHist";
            this.blueHist.Size = new System.Drawing.Size(48, 19);
            this.blueHist.TabIndex = 2;
            this.blueHist.TabStop = true;
            this.blueHist.Text = "Blue";
            this.blueHist.UseVisualStyleBackColor = true;
            this.blueHist.CheckedChanged += new System.EventHandler(this.blueHist_CheckedChanged);
            // 
            // allHist
            // 
            this.allHist.AutoSize = true;
            this.allHist.Location = new System.Drawing.Point(3, 78);
            this.allHist.Name = "allHist";
            this.allHist.Size = new System.Drawing.Size(39, 19);
            this.allHist.TabIndex = 3;
            this.allHist.TabStop = true;
            this.allHist.Text = "All";
            this.allHist.UseVisualStyleBackColor = true;
            this.allHist.CheckedChanged += new System.EventHandler(this.allHist_CheckedChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "BMP Files (*.bmp)|*.bmp";
            // 
            // lumHist
            // 
            this.lumHist.AutoSize = true;
            this.lumHist.Location = new System.Drawing.Point(3, 103);
            this.lumHist.Name = "lumHist";
            this.lumHist.Size = new System.Drawing.Size(84, 19);
            this.lumHist.TabIndex = 4;
            this.lumHist.TabStop = true;
            this.lumHist.Text = "Luminosity";
            this.lumHist.UseVisualStyleBackColor = true;
            this.lumHist.CheckedChanged += new System.EventHandler(this.lumHist_CheckedChanged);
            // 
            // HistogramPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 407);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HistogramPopUp";
            this.Text = "Histograms";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HistogramPopUp_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton redHist;
        private System.Windows.Forms.RadioButton greenHist;
        private System.Windows.Forms.RadioButton blueHist;
        private System.Windows.Forms.RadioButton allHist;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton lumHist;
    }
}