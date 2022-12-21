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
    public partial class Form1 : Form
    {
        string sourcePath = "";
        public string editingPath = "";
        string savingPath = "";
        HistogramPopUp histPopUp;
        Form2 codePopUp;
        public Form1()
        {
            InitializeComponent();
        }

        #region Utilitary

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Clear();

                pictureBox1.Load(openFileDialog1.FileName);
                MyImage source = new(pictureBox1.ImageLocation);
                sourcePath = pictureBox1.ImageLocation;
                string[] tab = sourcePath.Split("\\");


                for (int i = 0; i < tab.Length - 1; i++) editingPath += tab[i] + "\\";
                editingPath = editingPath + "edits\\";
                Directory.CreateDirectory(editingPath);
                savingPath = editingPath + tab[tab.Length - 1].Substring(0, tab[tab.Length - 1].Length - 4); //chemin + nom de l'image

                currentSize.Text = String.Format("Current Size : {0} x {1} px", source.Header.Width, source.Header.Height);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
            if (histPopUp != null) histPopUp.Close();
            if (Directory.Exists(editingPath)) Directory.Delete(editingPath, true);
        }

        private void Clear()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
                sourcePath = "";
                savingPath = "";
                //foreach (string path in Directory.GetFiles(editingPath)) File.Delete(path);
                editingPath = "";
                currentSize.Text = "Current size : ";
                expectedSize.Text = "Expected size : ";
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void editsButton_Click(object sender, EventArgs e) //remove edits
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new(sourcePath);
                pictureBox1.Load(sourcePath);
                currentSize.Text = String.Format("Current Size : {0} x {1} px", source.Header.Width, source.Header.Height);
                expectedSize.Text = "Expected Size : ";
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage res = new MyImage(pictureBox1.ImageLocation);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!saveFileDialog1.FileName.Contains(editingPath))
                        if (saveFileDialog1.FileName != pictureBox1.ImageLocation) res.FromImageToFile(saveFileDialog1.FileName);
                        else MessageBox.Show("Saving path can't be the same as the image being displayed", "Saving not possible");
                    else MessageBox.Show("Save the image outside of the editing path", "Saving not possible");
                }
            }
        }

        #endregion

        #region Clicks

        private void greyscaleButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.Greyscale());
                savingPath = savingPath + "_grscl.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void BWButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.BlackAndWhite());
                savingPath += "_bandw.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }

        }

        private void negativeButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.Negative());
                savingPath = savingPath + "_neg.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void blurButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.Blur());
                savingPath = savingPath + "_blurred.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void sharpenButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.Sharpening());
                savingPath = savingPath + "_sharpened.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void edgeDetButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new MyImage(pictureBox1.ImageLocation);
                MyImage res = new MyImage(source.EdgeDetection());
                savingPath = savingPath + "_edged.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int value = (int)angle.Value;
                if (value != 0)
                {
                    MyImage source = new MyImage(pictureBox1.ImageLocation);
                    MyImage res = new MyImage(source.Rotate(value));
                    savingPath = savingPath + "_rotated.bmp";
                    res.FromImageToFile(savingPath);
                    pictureBox1.Load(savingPath);

                    currentSize.Text = String.Format("Current Size : {0} x {1} px", res.Header.Width, res.Header.Height);
                    expectedSize.Text = "Expected Size : ";
                }
            }

        }

        private void embossingButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new(pictureBox1.ImageLocation);
                MyImage res = new(source.Embossing());
                savingPath += "_embossed.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);
            }
        }

        private void scaleButton_Click(object sender, EventArgs e)
        {
            if (valueScale.Value > 0)
            {
                MyImage source = new(pictureBox1.ImageLocation);
                MyImage res = new(source.Scale((float)valueScale.Value));
                savingPath += "_scaled.bmp";
                res.FromImageToFile(savingPath);
                pictureBox1.Load(savingPath);

                currentSize.Text = String.Format("Current Size : {0} x {1} px", res.Header.Width, res.Header.Height);
                expectedSize.Text = "Expected Size : ";
            }

        }

        private void histogramButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                histPopUp = new HistogramPopUp(pictureBox1, editingPath);
                histPopUp.Show();
            }
        }


        private void encodeButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                codePopUp = new Form2(pictureBox1, editingPath, "encode");
                codePopUp.Show();
            }
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                codePopUp = new Form2(pictureBox1, editingPath, "decode");
                codePopUp.Show();
            }
        }

        private void fractalButton_Click(object sender, EventArgs e)
        {
            codePopUp = new Form2(pictureBox1, editingPath, "fractal");
            codePopUp.Show();
        }

        private void genQRCodeButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text.Length > 0)
            {
                try
                {
                    MyImage qrCode = new(new QRCode(textBox1.Text).PrintCode());
                    codePopUp = new Form2(pictureBox1, editingPath, "QR Code", qrCode);
                    codePopUp.Show();
                    textBox1.Text = "";
                }
                catch
                {
                    MessageBox.Show("Veuillez insérer un texte avec des caractères alphanumériques", "Génération impossible");
                    textBox1.Text = "";
                }
            }
        }

        #endregion

        #region Events

        private void angle_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new(pictureBox1.ImageLocation);
                MyImage res = new(source.Rotate((int)angle.Value));

                if (angle.Value != 0)
                    expectedSize.Text = String.Format("Expected Size : {0} x {1} px", res.Header.Width, res.Header.Height);
                else expectedSize.Text = "Expected Size :";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            zoomCheckBox.Checked = true;
            mirrorBox.Text = "Mirror";
            Width = 2800;
            Height = 1485;
            MessageBox.Show("Quand vous avez fini, veillez à fermer l'application via le bouton \"Close\" situé en bas à droite", "Information importante");
        }

        private void mirrorBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (mirrorBox.Text != "Mirror")
                {
                    MyImage source = new MyImage(pictureBox1.ImageLocation);
                    MyImage res = null;
                    if (mirrorBox.Text == "X-axis") res = new MyImage(source.MirrorX());
                    else if (mirrorBox.Text == "Y-axis") res = new MyImage(source.MirrorY());
                    else if (mirrorBox.Text == "Both") res = new MyImage(source.Mirror());
                    else MessageBox.Show("Choose one of the options within the box");
                    savingPath = savingPath + "_mirrored.bmp";
                    res.FromImageToFile(savingPath);
                    pictureBox1.Load(savingPath);
                }
            }
        }

        private void valueScale_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MyImage source = new(pictureBox1.ImageLocation);
                if (valueScale.Value > 0)
                {
                    expectedSize.Text = String.Format("Expected Size : {0} x {1} px", (int)(source.Header.Width * valueScale.Value), (int)(source.Header.Height * valueScale.Value));
                }
                else expectedSize.Text = "Expected Size :";
            }
        }

        private void zoomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (zoomCheckBox.Checked) pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            else pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        #endregion

    }
}
