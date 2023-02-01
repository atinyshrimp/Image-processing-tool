using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace PSI_Joyce
{
    /// <summary>
    /// Class recreating the principle of Microsoft's Bitmap class
    /// </summary>
    public class MyImage
    {
        #region Attributes

        private readonly string type = ".bmp";
        private HeaderInfo header;
        private Pixel[,] image;

        #endregion

        #region Properties

        /// <summary>
        /// Class grouping all the header information (first 54 bytes) of the .bmp file
        /// </summary>
        public HeaderInfo Header { get => header; }

        /// <summary>
        /// Image transcribed into a Pixel matrix
        /// </summary>
        public Pixel[,] Image { get => image; }


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a MyImage object from a file
        /// </summary>
        /// <param name="file">bitmap file (.bmp) serving as the basis for creating the instance</param>
        public MyImage(string file) // has to be a .bmp file
        {
            if (file.Contains(type))
            {
                header = new HeaderInfo(file);

                image = new Pixel[header.Height, header.Width];
                byte[] img = File.ReadAllBytes(file);
                Queue<byte> _img = new Queue<byte>();

                for (int i = header.SizeOffset; i < img.Length; i++) _img.Enqueue(img[i]);

                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        image[i, j] = Pixel.ReadPixel(_img);
                    }
                    if (header.Padding != 0)
                        for (int j = 1; j <= header.Padding; j++) _img.Dequeue();
                }
            }
        }

        /// <summary>
        /// Creates an instance of MyImage from a Pixel array
        /// </summary>
        /// <param name="pxm">Pixel matrix corresponding to an image</param>
        public MyImage(Pixel[,] pxm)
        {
            image = pxm;
            header = new HeaderInfo(pxm);
        }

        #endregion

        #region Methods

        #region Utilitary methods

        /// <summary>
        /// Saves the image, following any changes made, to a folder on the computer
        /// </summary>
        /// <param name="file">Path under which to save the image</param>
        public void FromImageToFile(string file)
        {
            if (file.Contains(type))
            {
                byte[] res = new byte[header.Size];

                List<byte> output = new List<byte>();

                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                        for (int k = 2; k >= 0; k--) output.Add(image[i, j][k]); // bitmap reading order in our case : blue, green, red

                    if (header.Padding != 0)
                    {
                        for (int k = 1; k <= header.Padding; k++)
                        {
                            output.Add(0);
                        }
                    }
                }

                byte[] bitmap = new byte[output.Count];
                output.CopyTo(bitmap);

                for (int i = 0; i < header.Size; i++)
                {
                    if (i < header.SizeOffset) res[i] = header.Array[i]; // first 54 bytes so just information
                    else res[i] = bitmap[i - header.SizeOffset];
                }
                
                File.WriteAllBytes(file, res);
            }
        }

        /// <summary>
        /// Convert the passed array of bytes into an integer
        /// </summary>
        /// <param name="tab">Byte array to convert</param>
        /// <returns></returns>
        static public int EndianToInt(byte[] tab)
        {
            int res = 0;
            if (tab != null && tab.Length > 0)
            {
                if (!BitConverter.IsLittleEndian) Array.Reverse(tab);

                if (tab.Length == 8) res = Convert.ToInt32(BitConverter.ToUInt64(tab, 0));
                else if (tab.Length == 4) res = Convert.ToInt32(BitConverter.ToUInt32(tab, 0));
                else if (tab.Length == 2) res = Convert.ToInt32(BitConverter.ToUInt16(tab, 0));
            }
            return res;
        }

        /// <summary>
        /// Convert an integer to an array of bytes
        /// </summary>
        /// <param name="val">Integer to convert</param>
        /// <param name="length">Length of the wanted array (either 2 or 4)</param>
        /// <returns></returns>
        static public byte[] IntToEndian(int val, int length)
        {
            byte[] res = null;
            if ((val >= 0 && length == 2) || (val >= 0 && length == 4))
            {
                switch (length)
                {
                    case 2:
                        res = BitConverter.GetBytes((Int16)val);
                        break;

                    case 4:
                        res = BitConverter.GetBytes(val);
                        break;
                }
            }
            return res;
        }

        /// <summary>
        /// Fills a string representing a binary number with 0s on the left
        /// </summary>
        /// <param name="bin">The binary number to fill</param>
        /// <param name="length">Total number of bits wanted</param>
        /// <returns></returns>
        static public string FillBinary(string bin, int length)
        {
            string res = null;
            if (bin != null && bin != "")
            {
                if (bin.Length < length)
                {
                    int filling = length - bin.Length;
                    string filler = "";

                    for (int l = 1; l <= filling; l++)
                        filler += "0";

                    res = filler + bin;

                }
                else if (bin.Length > length)
                {
                    res = bin.Substring(0, length);
                }
                else res = bin;
            }
            return res;
        }

        static private int FindMax(int[] tab)
        {
            int max = -1;
            if (tab != null && tab.Length > 0)
            {
                max = 0;
                for (int i = 0; i < tab.Length; i++)
                {
                    if (tab[i] > max) max = tab[i];
                }
            }
            return max;
        }

        #endregion

        #region Image processing methods (TD3)

        #region Modification of Pixels

        /// <summary>
        /// Makes instance of MyImage grayscale
        /// </summary>
        /// <returns>a matrix of Pixels, each Pixel corresponding to a shade of gray</returns>
        public Pixel[,] Greyscale()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];

                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        res[i, j] = image[i, j].Grayscale();
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Turns the current image in black and white
        /// </summary>
        /// <returns>Pixel Matrix where each Pixel is either black or white</returns>
        public Pixel[,] BlackAndWhite()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];
                for (int i = 0; i < header.Height; i++)
                {
                     for (int j = 0; j < header.Width; j++)
                    {
                        if (image[i, j].Avrg < 255/2) res[i, j] = new Pixel(0, 0, 0);
                        else res[i, j] = new Pixel(255, 255, 255);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Reverses the colors of the current image
        /// </summary>
        /// <returns>Pixel Matrix where each pixel is the negative color of the original pixel</returns>
        public Pixel[,] Negative()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];
                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        res[i, j] = image[i, j].Negative();
                    }
                }
            }
            return res;
        }

        #endregion

        #region Modification of Pixels' position

        /// <summary>
        /// Flips the image diagonally
        /// </summary>
        /// <returns></returns>
        public Pixel[,] Mirror()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[image.GetLength(0), image.GetLength(1)];
                for (int i = 0; i < image.GetLength(0); i++)
                    for (int j = 0; j < image.GetLength(1); j++)
                        res[i, j] = image[header.Height - 1 - i, header.Width - 1 - j];
            }
            return res;
        }

        /// <summary>
        /// Flip the image along the x-axis
        /// </summary>
        /// <returns>Pixel matrix symmetrical to that of the current instance relative to the x-axis</returns>
        public Pixel[,] MirrorX() 
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];
                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        res[i, j] = image[header.Height - 1 - i, j];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Flip the image along the y-axis
        /// </summary>
        /// <returns>Pixel matrix symmetrical to that of the current instance relative to the y-axis</returns>
        public Pixel[,] MirrorY()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];
                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        res[i, j] = image[i, header.Width - 1 - j];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Enlarges or shrinks the current image according to the factor passed in parameter
        /// </summary>
        /// <param name="ratio">scale factor: float strictly greater than 0</param>
        /// <returns>Pixel matrix enlarged if ratio > 1 and reduced if ratio is between 0 and 1</returns>
        public Pixel[,] Scale(float ratio)
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0 && ratio > 0)
            {
                res = new Pixel[(int)(header.Height * ratio), (int)(header.Width * ratio)];
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    for (int j = 0; j < res.GetLength(1); j++)
                    {
                        res[i, j] = image[(int)(i / ratio), (int)(j / ratio)];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Rotates the pixel matrix of the current MyImage instance, clockwise (-) and counter-clockwise (+)
        /// </summary>
        /// <param name="angle">angle (in degrees) to use when rotating (negative for clockwise)</param>
        /// <returns></returns>
        public Pixel[,] Rotate(int angle) // to be able to work with any angle
        {
            Pixel[,] res = null;
            if (angle == 90 || angle == -90 || angle == 180 || angle == -180)
            {
                switch(angle)
                {
                    case -90: // clockwise

                        #region Rotation -90°
                        res = new Pixel[image.GetLength(1), image.GetLength(0)];

                        int nLigne = 0;
                        int nColonne;
                        for (int j = image.GetLength(1) - 1; j >= 0 ; j--)
                        {
                            nColonne = 0;
                            for (int i = 0; i < image.GetLength(0); i++)
                            {
                                res[nLigne, nColonne] = image[i, j];
                                nColonne++;
                            }
                            nLigne++;
                        }

                        break;
                    #endregion 

                    case 90: //counter-clockwise

                        #region Rotation 90°
                        Pixel[,] temp = Rotate(180);
                        res = new Pixel[image.GetLength(1), image.GetLength(0)];

                        int nLine = 0;
                        int nColumn;
                        for (int j = image.GetLength(1) - 1; j >= 0; j--)
                        {
                            nColumn = 0;
                            for (int i = 0; i < image.GetLength(0); i++)
                            {
                                res[nLine, nColumn] = temp[i, j];
                                nColumn++;
                            }
                            nLine++;
                        }
                        break;
                    #endregion

                    case 180 or -180:

                        #region Rotation + ou - 180°
                        res = new Pixel[image.GetLength(0), image.GetLength(1)];
                        for (int i = 0; i < image.GetLength(0); i++)
                            for (int j = 0; j < image.GetLength(1); j++)
                                res[i, j] = image[image.GetLength(0) - 1 - i, image.GetLength(1) - 1 - j];

                        break;
                    #endregion
                }
            }
            else
            {
                double radian = angle * Math.PI / 180;
                int newHeight;

                if (angle < 90 && angle > -90)
                    newHeight = (int)Math.Abs(Math.Cos(Math.Abs(radian)) * header.Height + Math.Sin(Math.Abs(radian)) * header.Width);
                else newHeight = (int)Math.Abs(Math.Sqrt(Math.Pow(header.Height, 2) + Math.Pow(header.Width, 2))); //(int)Math.Abs(Math.Cos(Math.Abs(radian)) * header.Width + Math.Sin(Math.Abs(radian)) * header.Height);

                int newWidth = (int)Math.Abs(Math.Sqrt(Math.Pow(header.Height, 2) + Math.Pow(header.Width, 2)));

                res = new Pixel[newHeight, newWidth];
                int centerX = newWidth / 2;
                int centerY = newHeight / 2;

                for (int i = 0; i < newHeight; i++)
                {
                    for (int j = 0; j < newWidth; j++)
                    {
                        int x = (int)(Math.Cos(radian) * (j - centerX) + Math.Sin(radian) * (i - centerY) + header.Width / 2);
                        int y = (int)(-Math.Sin(radian) * (j - centerX) + Math.Cos(radian) * (i - centerY) + header.Height / 2);

                        if (x < header.Width && x >= 0 && y < header.Height && y >= 0)
                            res[i, j] = image[y, x];
                        else res[i, j] = Pixel.Black;
                    }
                }
            }
            return res;
        }

        #endregion

        #endregion

        #region Filters (TD4)

        /// <summary>
        /// Applies a convolution to the image of the current instance
        /// </summary>
        /// <param name="filter">Kernel to use when convolving</param>
        /// <returns></returns>
        private Pixel[,] Convolve(float[,] filter)
        {
            Pixel[,] res = null;
            if (image != null && filter != null && image.Length > 0 && filter.Length > 0)
            {
                int hauteur = image.GetLength(0);
                int largeur = image.GetLength(1);
                res = new Pixel[hauteur, largeur];
                int offset = filter.GetLength(0) / 2;

                //calcul du coefficient de "pondération"
                float coeff = 0;
                foreach (float elt in filter) coeff += elt;
                if (coeff == 0) coeff = 1;

                for (int i = offset; i < hauteur - offset; i++)
                {
                    for (int j = offset; j < largeur - offset; j++)
                    {
                        int r = (int)((filter[0, 0] * image[i - 1, j - 1].Red + filter[0, 1] * image[i - 1, j].Red + filter[0, 2] * image[i - 1, j + 1].Red +
                                        filter[1, 0] * image[i, j - 1].Red + filter[1, 1] * image[i, j].Red + filter[1, 2] * image[i, j + 1].Red +
                                        filter[2, 0] * image[i + 1, j - 1].Red + filter[2, 1] * image[i + 1, j].Red + filter[2, 2] * image[i + 1, j + 1].Red) / coeff);
                        if (r < 0) r = 0;
                        else if (r > 255) r = 255;

                        int g = (int)((filter[0, 0] * image[i - 1, j - 1].Green + filter[0, 1] * image[i - 1, j].Green + filter[0, 2] * image[i - 1, j + 1].Green +
                                        filter[1, 0] * image[i, j - 1].Green + filter[1, 1] * image[i, j].Green + filter[1, 2] * image[i, j + 1].Green +
                                        filter[2, 0] * image[i + 1, j - 1].Green + filter[2, 1] * image[i + 1, j].Green + filter[2, 2] * image[i + 1, j + 1].Green) / coeff);
                        if (g < 0) g = 0;
                        else if (g > 255) g = 255;

                        int b = (int)((filter[0, 0] * image[i - 1, j - 1].Blue + filter[0, 1] * image[i - 1, j].Blue + filter[0, 2] * image[i - 1, j + 1].Blue +
                                        filter[1, 0] * image[i, j - 1].Blue + filter[1, 1] * image[i, j].Blue + filter[1, 2] * image[i, j + 1].Blue +
                                        filter[2, 0] * image[i + 1, j - 1].Blue + filter[2, 1] * image[i + 1, j].Blue + filter[2, 2] * image[i + 1, j + 1].Blue) / coeff);
                        if (b < 0) b = 0;
                        else if (b > 255) b = 255;

                        res[i, j] = new Pixel((byte)r, (byte)g, (byte)b);
                    }
                }

                //traitement des bords
                for (int i = 0; i < hauteur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        if (res[i, j] is null)
                        {
                            if (i == 0 && j == 0) res[i, j] = res[i + 1, j + 1];
                            else if (i == 0 && j == largeur - 1) res[i, j] = res[i + 1, j - 1];
                            else if (i == hauteur - 1 && j == 0) res[i, j] = res[i - 1, j + 1];
                            else if (i == hauteur - 1 && j == largeur - 1) res[i, j] = res[i - 1, j - 10];
                            else if (i == 0) res[i, j] = res[i + 1, j];
                            else if (j == 0) res[i, j] = res[i, j + 1];
                            else if (j == largeur - 1) res[i, j] = res[i, j - 1];
                            else if (i == hauteur - 1) res[i, j] = res[i - 1, j];
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Applies a blur effect to the current image
        /// </summary>
        /// <returns></returns>
        public Pixel[,] Blur()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Blur);
            return res;
        }

        /// <summary>
        /// Applies the "edge detection" effect on the current image
        /// </summary>
        /// <returns></returns>
        public Pixel[,] EdgeDetection()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.EdgeDetect);
            return res;

        }

        /// <summary>
        /// Applies the "Sharpening" effect to the current image
        /// </summary>
        /// <returns>Pixel matrix corresponding to a sharper image than the original</returns>
        public Pixel[,] Sharpening()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Sharpen);
            return res;
        }

        /// <summary>
        /// Reinforce the edges of within an image
        /// </summary>
        /// <returns></returns>
        public Pixel[,] Embossing()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Embossing);
            return res;

        }

        #endregion

        #region TD5

        /// <summary>
        /// Generates a histogram of all the colors in the image
        /// </summary>
        /// <returns>Pixel matrix representing an RGB histogram of an image</returns>
        public Pixel[,] Histogram()
        {
            Pixel[,] res = new Pixel[200, 256];
            int hauteur = res.GetLength(0);
            int borderPadding = 25; // the max of the graph will be 25px from the top of the image

            int[] rouge = new int[256];
            int[] vert = new int[256];
            int[] bleu = new int[256];


            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    rouge[image[i, j].Red]++;
                    vert[image[i, j].Green]++;
                    bleu[image[i, j].Blue]++;
                }
            }

            int[] max = new int[3];

            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = new Pixel(0, 0, 0);

            int maxRouge = FindMax(rouge);
            int maxVert = FindMax(vert);
            int maxBleu = FindMax(bleu);

            max[0] = maxRouge; max[1] = maxVert; max[2] = maxBleu;

            int realMax = FindMax(max);

            for (int j = 0; j < res.GetLength(1); j++)
            {
                for (int i = 0; i < rouge[j] * (hauteur - borderPadding) / realMax; i++)
                {
                    res[i, j].Red = 255; 
                }

                for (int i = 0; i < vert[ j] * (hauteur - borderPadding) / realMax; i++)
                {
                    res[i, j].Green = 255;
                }

                for (int i = 0; i < bleu[j] * (hauteur - borderPadding) / realMax; i++)
                {
                    res[i, j].Blue = 255; ;
                }
            }

            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    if (res[i, j] is null)
                        res[i, j] = Pixel.White;
            return res;
        }


        /// <summary>
        /// Generates a histogram based on the image of the current instance and the submitted color
        /// </summary>
        /// <param name="color">Color on which the histogram will be based</param>
        /// <returns></returns>
        public Pixel[,] Histogram(string color)
        {
            Pixel[,] res = null;
            if (color == "red" || color == "green" || color == "blue" | color == "luminosity")
            {
                res = new Pixel[200, 256];
                int hauteur = res.GetLength(0);
                int borderPadding = 25;

                int[] rouge = new int[256];
                int[] vert = new int[256];
                int[] bleu = new int[256];
                int[] lum = new int[256];

                for (int i = 0; i < image.GetLength(0); i++)
                {
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        rouge[image[i, j].Red]++;
                        vert[image[i, j].Green]++;
                        bleu[image[i, j].Blue]++;
                        lum[image[i, j].Avrg]++;
                    }
                }

                int maxRouge = FindMax(rouge);
                int maxVert = FindMax(vert);
                int maxBleu = FindMax(bleu);
                int maxLum = FindMax(lum);

                switch (color)
                {
                    case "red":
                        for (int j = 0; j < res.GetLength(1); j++)
                        {
                            for (int i = 0; i < rouge[j] * (hauteur - borderPadding) / maxRouge; i++)
                            {
                                res[i, j] = new Pixel(255, 0, 0); 
                            }
                        }
                        break;

                    case "green":
                        for (int j = 0; j < res.GetLength(1); j++)
                        {
                            for (int i = 0; i < vert[j] * (hauteur - borderPadding) / maxVert; i++)
                            {
                                res[i, j] = new Pixel(0, 255, 0);
                            }
                        }
                        break;

                    case "blue":
                        for (int j = 0; j < res.GetLength(1); j++)
                        {
                            for (int i = 0; i < bleu[j] * (hauteur - borderPadding) / maxBleu; i++)
                            {
                                res[i, j] = new Pixel(0, 0, 255);
                            }
                        }
                        break;

                    case "luminosity":
                        for (int j = 0; j < res.GetLength(1); j++)
                        {
                            for (int i = 0; i < lum[j] * (hauteur - borderPadding) / maxLum; i++)
                            {
                                res[i, j] = Pixel.White;
                            }
                        }
                        break;
                }

                for (int i = 0; i < res.GetLength(0); i++)
                    for (int j = 0; j < res.GetLength(1); j++)
                        if (res[i, j] is null)
                            res[i, j] = Pixel.Black;

            }
            return res;
        }


        /// <summary>
        /// Hide an image within another
        /// </summary>
        /// <param name="mat">Image to hide in current instance</param>
        /// <returns></returns>
        public Pixel[,] EncodeImage(Pixel[,] mat)
        {
            Pixel[,] res = null;
            if (mat != null && mat.Length > 0)
            {
                if (mat.GetLength(0) <= image.GetLength(0) && mat.GetLength(1) <= image.GetLength(1))
                {
                    res = new Pixel[image.GetLength(0), image.GetLength(1)];
                    string[] hiderPix = new string[3]; // main image pixel color component array
                    string[] toHidePix = new string[3]; // array of color components of the image pixel to hide
                    string[] resPix = new string[3]; // same but for the pixel of the resulting image


                    for (int i = 0; i < image.GetLength(0); i++)
                    {
                        for (int j = 0; j < image.GetLength(1); j++)
                        {
                            if (i < mat.GetLength(0) && j < mat.GetLength(1))
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    // conversion to binary of the value of the color component
                                    hiderPix[k] = Convert.ToString(image[i, j][k], 2);
                                    toHidePix[k] = Convert.ToString(mat[i, j][k], 2);

                                    string strongHider = FillBinary(hiderPix[k], 8).Substring(0, 4); // most significant bits of the main image
                                    string strongToHide = FillBinary(toHidePix[k], 8).Substring(0, 4); // most significant bits of the image to hide

                                    resPix[k] = strongHider + strongToHide;
                                }

                                //res[i, j] = new Pixel(Convert.ToByte(resPix[0], 2), Convert.ToByte(resPix[1], 2), Convert.ToByte(resPix[2], 2));

                            }
                            else //res[i, j] = image[i, j];
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    hiderPix[k] = FillBinary(Convert.ToString(image[i, j][k], 2), 8);
                                    resPix[k] = hiderPix[k].Substring(0, 4) + "0000";
                                }
                            }
                            res[i, j] = new Pixel(Convert.ToByte(resPix[0], 2), Convert.ToByte(resPix[1], 2), Convert.ToByte(resPix[2], 2));
                        }
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// Retrieves an image hidden in another, if there is one
        /// </summary>
        /// <returns>The hidden image within the current instance</returns>
        public Pixel[,] DecodeImage()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[image.GetLength(0), image.GetLength(1)];
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        string[] resPix = new string[3];
                        
                        for (int k = 0; k < 3; k++)
                        {
                            resPix[k] = FillBinary(Convert.ToString(image[i, j][k], 2), 8); // convert the byte to binary and fill it on the left if needed
                            resPix[k] = resPix[k].Substring(4, 4) + "0000"; // we only take the least significant bits which correspond to the most significant bits of the hidden image
                        }
                        res[i, j] = new Pixel(Convert.ToByte(resPix[0], 2), Convert.ToByte(resPix[1], 2), Convert.ToByte(resPix[2], 2));                    
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// Generates a fractal from a selection
        /// </summary>
        /// <param name="type">type of the desired fractal</param>
        /// <returns>Image (Pixel matrix) representing a fractal</returns>
        static public Pixel[,] Fractal(string type = "")
        {
            int hauteur = 800;
            int largeur = 800;
            int iteration = 255;
            int factor = (int)Math.Sqrt(Math.Pow(hauteur / 2, 2) + Math.Pow(largeur / 2, 2));
            float zoom = 1f; // 2.75f
            float decalageY = 0f;
            float decalageX = 0f; // -.5f
            Pixel[,] res = new Pixel[hauteur, largeur];

            Complex center = new(0, 0);
            Complex c;
            Complex z;

            var colors = (from s in Enumerable.Range(0, 256)
                          select Color.FromArgb((s >> 5) * 36, (s >> 3 & 7) * 36, (s & 3) * 85)).ToArray();

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    z = new Complex((float)(1.5 * (i - hauteur / 2) / (0.5 * hauteur * zoom) + decalageY), (float)(1.0 * (j - largeur / 2) / (0.5 * largeur * zoom) + decalageX)); //cmmt on choisit Z0 ???
                    Complex og = z;
                    int k = iteration;
                    while (z.Module < 2 && k > 1)
                    {
                        switch (type)
                        {
                            case "Julia":
                                c = new(-.4f, -.59f);
                                z = z.Pow(2) + c;
                                break;

                            case "Cubic Julia":
                                c = new(-.5f, -.05f);
                                z = z.Pow(3) + c;
                                break;

                            case "Sin Julia":
                                c = new(1f, .2f);
                                z = z.Pow(4) + c;
                                // z = c * z.Sin;
                                break;
                        }
                        k--;
                    }
                    res[i, j] = new Pixel(colors[k].R, colors[k].G, colors[k].B); // new Pixel((byte)k, (byte)k, (byte)k);
                }
            }

            return res;
        }

        #endregion
        #endregion

    }
        
}
