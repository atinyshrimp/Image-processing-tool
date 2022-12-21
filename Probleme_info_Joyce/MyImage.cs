using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace PSI_Joyce
{
    /// <summary>
    /// Classe reprenant le principe de la classe Bitmap de Microsoft
    /// </summary>
    public class MyImage
    {
        #region Attributes

        private HeaderInfo header;
        private string type = ".bmp";
        private Pixel[,] image;

        #endregion

        #region Properties

        /// <summary>
        /// Classe regroupant toutes les informations de l'entête (54 premiers bytes) du fichier .bmp
        /// </summary>
        public HeaderInfo Header { get => header; }

        /// <summary>
        /// Image retranscrite en matrice de pixels
        /// </summary>
        public Pixel[,] Image { get => image; }


        #endregion

        #region Constructors

        /// <summary>
        /// Construit une instance d'objet MyImage à partir d'un fichier
        /// </summary>
        /// <param name="file">fichier bitmap (.bmp) servant de base à la création de l'instance</param>
        public MyImage(string file) //forcément un fichier .bmp
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
        /// Crée une instance de MyImage à partir d'une matrice de Pixel
        /// </summary>
        /// <param name="pxm">Matrice de Pixel correspond à une photo</param>
        public MyImage(Pixel[,] pxm)
        {
            image = pxm;
            header = new HeaderInfo(pxm);
        }
        #endregion

        #region Methods

        #region Utilitary methods

        /// <summary>
        /// Enregistre l'image, à la suite des modifications potentiellement apportées, dans un dossier de l'ordinateur
        /// </summary>
        /// <param name="file">Chemin sous lequel il faudra sauvegarder l'image</param>
        public void FromImageToFile(string file)
        {
            if (file.Contains(type))
            {
                byte[] res = new byte[header.Size];

                List<byte> output = new List<byte>();

                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                        for (int k = 2; k >= 0; k--) output.Add(image[i, j][k]); //ordre de lecture des bitmaps dans notre cas : bleu, vert, rouge

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
                    if (i < header.SizeOffset) res[i] = header.Array[i]; //premiers 54 bytes donc de l'info
                    else res[i] = bitmap[i - header.SizeOffset];
                }
                
                File.WriteAllBytes(file, res);
            }
        }

        /// <summary>
        /// Convertit le tableau de bytes passé en paramètres en entier
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        static public int ConvertirEndianToInt(byte[] tab)
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
        /// Convertit un entier en tableau de bytes
        /// </summary>
        /// <param name="val">Entier à convertir</param>
        /// <param name="length">Longueur du tableau voulu (soit 2, soit 4)</param>
        /// <returns></returns>
        static public byte[] ConvertirIntToEndian(int val, int length)
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
        /// Remplit à gauche une chaîne de caractères représentant un nombre binaire
        /// </summary>
        /// <param name="bin">Le nombre binaire à remplir</param>
        /// <param name="length">Nombre total de bits voulus</param>
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

        #region Modification des Pixels

        /// <summary>
        /// Rend l'instance de MyImage en naunces de gris
        /// </summary>
        /// <returns>une matrice de Pixel, chacun correspondant à une nuance de gris</returns>
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
        /// Affiche l'image courante en noir et blanc
        /// </summary>
        /// <returns>Matrice de Pixel où chacun des Pixels est soit noir, soit blanc</returns>
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
        /// Inverse les couleurs de l'image courante
        /// </summary>
        /// <returns>Matrice de Pixels où chaque pixel est la couleur négative du pixel originale</returns>
        public Pixel[,] Negatif()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0)
            {
                res = new Pixel[header.Height, header.Width];
                for (int i = 0; i < header.Height; i++)
                {
                    for (int j = 0; j < header.Width; j++)
                    {
                        res[i, j] = image[i, j].Negatif();
                    }
                }
            }
            return res;
        }

        #endregion

        #region Modification des positions des Pixels

        /// <summary>
        /// Flip l'image diagonalement
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
        /// Flip l'image en ayant en référence l'axe des x
        /// </summary>
        /// <returns>Matrice de Pixel symétrique à celle de l'instance courante par rapport à l'axe des abscisses</returns>
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
        /// Flip l'image avec l'axe des y comme référence
        /// </summary>
        /// <returns>Matrice de Pixel symétrique à celle de l'instance courante par rapport à l'axe des ordonnées</returns>
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
        /// Agrandit ou rétrécit l'image courante en fonction du facteur passé en paramètre
        /// </summary>
        /// <param name="ratio">facteur d'agrandissement/de rétrécissement : float strictement supérieur à 0</param>
        /// <returns>Matrice de Pixel agrandie si ratio > 1 et rétrécie si ratio est compris entre 0 et 1</returns>
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
        /// Exécute la matrice de Pixel de l'instance courante de MyImage, dans le sens horaire (+) et anti-horaire (-)
        /// </summary>
        /// <param name="angle">angle (en degré) à utiliser lors de la rotation (négatif pour le sens horaire)</param>
        /// <returns></returns>
        public Pixel[,] Rotate(int angle) //pour avoir un angle quelconque
        {
            Pixel[,] res = null;
            if (angle == 90 || angle == -90 || angle == 180 || angle == -180)
            {
                switch(angle)
                {
                    case -90: //sens horaire

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

                    case 90: //sens anti-horaire

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

        #region Filtres (TD4)

        /// <summary>
        /// Applique une convolution sur l'image de l'instance courante
        /// </summary>
        /// <param name="filtre">Noyau à utiliser lors de la convolution</param>
        /// <returns></returns>
        private Pixel[,] Convolve(float[,] filtre)
        {
            Pixel[,] res = null;
            if (image != null && filtre != null && image.Length > 0 && filtre.Length > 0)
            {
                int hauteur = image.GetLength(0);
                int largeur = image.GetLength(1);
                res = new Pixel[hauteur, largeur];
                int offset = filtre.GetLength(0) / 2;

                //calcul du coefficient de "pondération"
                float coeff = 0;
                foreach (float elt in filtre) coeff += elt;
                if (coeff == 0) coeff = 1;

                for (int i = offset; i < hauteur - offset; i++)
                {
                    for (int j = offset; j < largeur - offset; j++)
                    {
                        int r = (int)((filtre[0, 0] * image[i - 1, j - 1].Red + filtre[0, 1] * image[i - 1, j].Red + filtre[0, 2] * image[i - 1, j + 1].Red +
                                        filtre[1, 0] * image[i, j - 1].Red + filtre[1, 1] * image[i, j].Red + filtre[1, 2] * image[i, j + 1].Red +
                                        filtre[2, 0] * image[i + 1, j - 1].Red + filtre[2, 1] * image[i + 1, j].Red + filtre[2, 2] * image[i + 1, j + 1].Red) / coeff);
                        if (r < 0) r = 0;
                        else if (r > 255) r = 255;

                        int g = (int)((filtre[0, 0] * image[i - 1, j - 1].Green + filtre[0, 1] * image[i - 1, j].Green + filtre[0, 2] * image[i - 1, j + 1].Green +
                                        filtre[1, 0] * image[i, j - 1].Green + filtre[1, 1] * image[i, j].Green + filtre[1, 2] * image[i, j + 1].Green +
                                        filtre[2, 0] * image[i + 1, j - 1].Green + filtre[2, 1] * image[i + 1, j].Green + filtre[2, 2] * image[i + 1, j + 1].Green) / coeff);
                        if (g < 0) g = 0;
                        else if (g > 255) g = 255;

                        int b = (int)((filtre[0, 0] * image[i - 1, j - 1].Blue + filtre[0, 1] * image[i - 1, j].Blue + filtre[0, 2] * image[i - 1, j + 1].Blue +
                                        filtre[1, 0] * image[i, j - 1].Blue + filtre[1, 1] * image[i, j].Blue + filtre[1, 2] * image[i, j + 1].Blue +
                                        filtre[2, 0] * image[i + 1, j - 1].Blue + filtre[2, 1] * image[i + 1, j].Blue + filtre[2, 2] * image[i + 1, j + 1].Blue) / coeff);
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
                        if (res[i, j] == null)
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
        /// Applique un effet flou sur l'image courante
        /// </summary>
        /// <returns></returns>
        public Pixel[,] Flou()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Blur);
            return res;
        }

        /// <summary>
        /// Applique l'effet "détection des bords" sur l'image courante
        /// </summary>
        /// <returns></returns>
        public Pixel[,] DetectionBord()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.EdgeDetect);
            return res;

        }

        /// <summary>
        /// Applique l'effet "Renforcement" sur l'image courante
        /// </summary>
        /// <returns>Matrice de Pixel correspondant à une image plus nette que l'originale</returns>
        public Pixel[,] Renforcement()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Sharpen);
            return res;
        }

        /// <summary>
        /// Renforce les bords d'une image
        /// </summary>
        /// <returns></returns>
        public Pixel[,] Repoussage()
        {
            Pixel[,] res = null;
            if (image != null && image.Length > 0) res = Convolve(Convolution.Embossing);
            return res;

        }

        #endregion

        #region TD5

        /// <summary>
        /// Génère un histogramme de toutes les couleurs de l'image
        /// </summary>
        /// <returns>Matrice de pixels représentant un histogramme RGB d'une image</returns>
        public Pixel[,] Histogram()
        {
            Pixel[,] res = new Pixel[200, 256];
            int hauteur = res.GetLength(0);
            int borderPadding = 25; //le max du graphe sera à 25px du haut de l'image

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
                    if (res[i, j] == null)
                        res[i, j] = Pixel.White;
            return res;
        }


        /// <summary>
        /// Génère un histogramme basé sur l'image de l'instance courante et la couleur soumise
        /// </summary>
        /// <param name="color">Couleur sur laquelle l'histogramme sera basé</param>
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
                        if (res[i, j] == null)
                            res[i, j] = Pixel.Black;

            }
            return res;
        }


        /// <summary>
        /// Cache une image dans une autre
        /// </summary>
        /// <param name="mat">Image à masquer dans l'instance actuelle</param>
        /// <returns></returns>
        public Pixel[,] EncodeImage(Pixel[,] mat)
        {
            Pixel[,] res = null;
            if (mat != null && mat.Length > 0)
            {
                if (mat.GetLength(0) <= image.GetLength(0) && mat.GetLength(1) <= image.GetLength(1))
                {
                    res = new Pixel[image.GetLength(0), image.GetLength(1)];
                    string[] hiderPix = new string[3]; //tableau des composantes de couleur du pixel de l'image principale
                    string[] toHidePix = new string[3]; //tableau des composantes de couleur du pixel de l'image à cacher
                    string[] resPix = new string[3]; //de même pour le pixel de l'image résultante


                    for (int i = 0; i < image.GetLength(0); i++)
                    {
                        for (int j = 0; j < image.GetLength(1); j++)
                        {
                            if (i < mat.GetLength(0) && j < mat.GetLength(1))
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    //conversion en binaire de la valeur de la composante de couleur
                                    hiderPix[k] = Convert.ToString(image[i, j][k], 2);
                                    toHidePix[k] = Convert.ToString(mat[i, j][k], 2);

                                    string strongHider = FillBinary(hiderPix[k], 8).Substring(0, 4); //bits de poids fort de l'image principale
                                    string strongToHide = FillBinary(toHidePix[k], 8).Substring(0, 4); //bits de poids fort de l'image à cacher

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
        /// Récupère une image cachée dans une autre, s'il y en a une
        /// </summary>
        /// <returns>L'image cachée dans l'instance courante</returns>
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
                            resPix[k] = FillBinary(Convert.ToString(image[i, j][k], 2), 8); //convertir le byte en binaire et le remplit à gauche si besoin
                            resPix[k] = resPix[k].Substring(4, 4) + "0000"; //on ne prend que les bits de poids faible qui correspondent aux bits de poids forts de l'image cachée
                        }
                        res[i, j] = new Pixel(Convert.ToByte(resPix[0], 2), Convert.ToByte(resPix[1], 2), Convert.ToByte(resPix[2], 2));                    
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// Génère une fractale parmi une sélection
        /// </summary>
        /// <param name="type">type de la fractale voulue</param>
        /// <returns>Image (matrice de pixels) représentant une fractale</returns>
        static public Pixel[,] Fractal(string type = "")
        {
            int hauteur = 800;
            int largeur = 800;
            int iteration = 255;
            int factor = (int)Math.Sqrt(Math.Pow(hauteur / 2, 2) + Math.Pow(largeur / 2, 2));
            float zoom = 1f; //2.75f
            float decalageY = 0f;
            float decalageX = 0f; //-.5f
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
                                //z = c * z.Sin;
                                break;
                        }
                        k--;
                    }
                    res[i, j] = new Pixel(colors[k].R, colors[k].G, colors[k].B);//new Pixel((byte)k, (byte)k, (byte)k);
                }
            }

            return res;
        }

        #endregion

        #endregion

    }
        
}
