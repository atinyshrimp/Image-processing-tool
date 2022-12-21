using System;
using System.IO;
using PSI_Joyce;
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
    public partial class Form2 : Form
    {
        string method = "";
        string editingPath;
        bool first = true;
        MyImage image1;
        MyImage image2;
        MyImage res;

        public Form2(PictureBox source, string editingPath, string method, MyImage img = null)
        {
            InitializeComponent();
            if (method == "fractal" || method == "QR Code") StartPosition = FormStartPosition.CenterScreen;
            else if (method == "encode" || method == "decode")
            {
                Width = 840; //2409 | 1910;
                Height = 627; //1761; // 1675;
            }

            if (source.Image != null)
                image1 = new MyImage(source.ImageLocation);
            this.method = method;
            this.editingPath = editingPath;
            res = img;
        }

        #region Clicks

        private void chooseButton_Click(object sender, EventArgs e)
        {
            if (method == "encode")
            {
                if (first)
                {
                    first = false;
                    openFileDialog1.Title = "Choose an image to hide...";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        image2 = new MyImage(openFileDialog1.FileName);
                        res = new MyImage(image1.EncodeImage(image2.Image));
                        res.FromImageToFile(editingPath + "encodedPicture.bmp");
                        pictureBox1.Load(editingPath + "encodedPicture.bmp");
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != null && saveFileDialog1.FileName != "")
                {
                    if (!saveFileDialog1.FileName.Contains(editingPath))
                    {
                        res.FromImageToFile(saveFileDialog1.FileName);
                        Close();
                    }
                    else MessageBox.Show("Save the image outside of the editing path", "Saving not possible");
                }
            }
        }

        #endregion

        #region Events

        private void Form2_Load(object sender, EventArgs e)
        {
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (method == "encode")
            {
                this.Text = "Hide a picture";
                label2.Text = "Encoded Picture";
                comboBox1.Enabled = false;
            }
            else if (method == "decode")
            {
                Text = "Recover a hidden picture";
                label2.Text = "Recovered picture";

                chooseButton.Enabled = false;
                comboBox1.Enabled = false;
                res = new MyImage(image1.DecodeImage());
                res.FromImageToFile(editingPath + "decodedPicture.bmp");
                pictureBox1.Load(editingPath + "decodedPicture.bmp");
            }
            else if (method == "fractal")
            {
                Text = "Generate a fractal";
                label2.Text = "Fractal";
                comboBox1.Text = "Choose a type of fractal...";

                chooseButton.Enabled = false;
            }

            else if (method == "QR Code")
            {
                Text = "Generate a QR Code";
                label2.Text = "Generated QR Code";

                chooseButton.Enabled = false;
                comboBox1.Enabled = false;

                res = new MyImage(res.MirrorX());
                res = new(res.Scale(100));
                res.FromImageToFile(editingPath + "QRCode.bmp");
                pictureBox1.Load(editingPath + "QRCode.bmp");

            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string type = comboBox1.Text;
            if (type != "Choose a type of fractal...")
            {
                res = new MyImage(MyImage.Fractal(type));

                if (!File.Exists(editingPath + String.Format("{0} Fractal.bmp", type)))
                    res.FromImageToFile(editingPath + String.Format("{0} Fractal.bmp", type));

                pictureBox1.Load(editingPath + String.Format("{0} Fractal.bmp", type));
                label2.Text = String.Format("{0} Fractal", type);
            }
        }

        #endregion

    }
}
