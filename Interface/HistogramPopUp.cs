using System;
using PSI_Joyce;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interface
{
    public partial class HistogramPopUp : Form
    {
        #region Attributes

        string ogEditingPath;
        string editingPath;
        MyImage histRed;
        MyImage histGreen;
        MyImage histBlue;
        MyImage histLum;
        MyImage hist;
        MyImage sourceImage;
        bool firstRed = true;
        bool firstGreen = true;
        bool firstBlue = true;
        bool firstAll = true;
        bool firstLum = true;

        #endregion

        public HistogramPopUp(PictureBox source, string path)
        {
            InitializeComponent();
            ogEditingPath = path;
            editingPath = path + "\\histograms\\";
            Directory.CreateDirectory(editingPath);
            sourceImage = new MyImage(source.ImageLocation);
            histRed = new MyImage(sourceImage.Histogram("red"));
            histGreen = new MyImage(sourceImage.Histogram("green"));
            histBlue = new MyImage(sourceImage.Histogram("blue"));
            histLum = new MyImage(sourceImage.Histogram("luminosity"));
            hist = new MyImage(sourceImage.Histogram());

            redHist.Checked = true;

            this.FormClosed += this.HistogramPopUp_FormClosed;
        }

        #region Radio Buttons

        private void allHist_CheckedChanged(object sender, EventArgs e)
        {
            if (allHist.Checked)
            {
                if (firstAll)
                {
                    firstAll = false;
                    hist.FromImageToFile(editingPath + "histogram.bmp");
                }
                pictureBox1.Load(editingPath + "histogram.bmp");
            }
        }

        private void blueHist_CheckedChanged(object sender, EventArgs e)
        {
            if (blueHist.Checked)
            {
                if (firstBlue)
                {
                    firstBlue = false;
                    histBlue.FromImageToFile(editingPath + "blueHistogram.bmp");
                }
                pictureBox1.Load(editingPath + "blueHistogram.bmp");
            }
        }

        private void greenHist_CheckedChanged(object sender, EventArgs e)
        {
            if (greenHist.Checked)
            {
                if (firstGreen)
                {
                    firstGreen = false;
                    histGreen.FromImageToFile(editingPath + "greenHistogram.bmp");
                }
                    pictureBox1.Load(editingPath + "greenHistogram.bmp");
            }
        }
        private void lumHist_CheckedChanged(object sender, EventArgs e)
        {
            if (lumHist.Checked)
            {
                if (firstLum)
                {
                    firstLum = false;
                    histLum.FromImageToFile(editingPath + "lumHistogram.bmp");
                }
                pictureBox1.Load(editingPath + "lumHistogram.bmp");
            }
        }

        private void redHist_CheckedChanged(object sender, EventArgs e)
        {
            if(redHist.Checked)
            {
                if (firstRed)
                {
                    firstRed = false;
                    histRed.FromImageToFile(editingPath + "redHistogram.bmp");
                }
                pictureBox1.Load(editingPath + "redHistogram.bmp");
            }
        }

        #endregion

        #region Others

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (redHist.Checked || greenHist.Checked || blueHist.Checked || allHist.Checked || lumHist.Checked)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!saveFileDialog1.FileName.Contains(ogEditingPath))
                    {
                        if (redHist.Checked) histRed.FromImageToFile(saveFileDialog1.FileName);
                        else if (greenHist.Checked) histGreen.FromImageToFile(saveFileDialog1.FileName);
                        else if (blueHist.Checked) histBlue.FromImageToFile(saveFileDialog1.FileName);
                        else if (lumHist.Checked) histLum.FromImageToFile(saveFileDialog1.FileName);
                        else hist.FromImageToFile(saveFileDialog1.FileName);
                    }
                    else MessageBox.Show("Save the image outside of the editing path", "Saving not possible");
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
            Directory.Delete(editingPath, true);
        }

        private void HistogramPopUp_FormClosed(object sender, FormClosedEventArgs e)
        {
/*            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (File.Exists(pictureBox1.ImageLocation)) File.Delete(pictureBox1.ImageLocation);
                pictureBox1.Image = null;
                Directory.Delete(editingPath, true);

            }
*/        }

        #endregion
    }
}
