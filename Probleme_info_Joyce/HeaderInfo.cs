using System.IO;

namespace PSI_Joyce
{

    /// <summary>
    /// Classe contenant toutes les informations des entêtes d'un fichier bitmap
    /// </summary>
    public class HeaderInfo
    {
        #region Attributes

        private int size;
        private int sizeOffset; // size offset : place where the image's bitmap starts
        private int height;
        private int width;
        private int bpc; // bits per color
        private int padding;

        private byte[] array;

        #endregion

        #region Properties

        /// <summary>
        /// Taille du fichier Bitmap (entêtes incluses)
        /// </summary>
        public int Size { get => size; }

        /// <summary>
        /// L'indice où commence le bitmap de l'image
        /// </summary>
        public int SizeOffset { get => sizeOffset; }

        /// <summary>
        /// Hauteur de l'image (en pixel)
        /// </summary>
        public int Height { get => height; }

        /// <summary>
        /// Largeur de l'image (en pixel)
        /// </summary>
        public int Width { get => width; }

        /// <summary>
        /// Nombre de 0 à compter pour enregistrer l'image adéqueatement au format Bitmap
        /// </summary>
        public int Padding { get => padding; }

        /// <summary>
        /// Tableau de bytes contenant tous les bytes des en-têtes du fichier bitmap
        /// </summary>
        public byte[] Array { get => array; }
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Création de l'entête d'une image à partir de son chemin
        /// </summary>
        /// <param name="file">Chemin menant au fichier .bmp à partir duquel l'entête sera créée</param>
        public HeaderInfo(string file)
        {
            if (file.Contains(".bmp"))
            {

                byte[] tab = File.ReadAllBytes(file);

                byte[] ogSize = new byte[4]; //donné sur 4 octets
                for (int i = 0; i < 4; i++) ogSize[i] = tab[2 + i];
                size = MyImage.EndianToInt(ogSize);

                byte[] ogSizeOffset = new byte[4]; //donné sur 4 octets
                for (int i = 0; i < 4; i++) ogSizeOffset[i] = tab[10 + i];
                sizeOffset = MyImage.EndianToInt(ogSizeOffset);

                array = new byte[sizeOffset];
                for (int i = 0; i < sizeOffset; i++) array[i] = tab[i];

                byte[] ogWidth = new byte[4]; //donné sur 4 octets
                for (int i = 0; i < 4; i++) ogWidth[i] = tab[18 + i];
                width = MyImage.EndianToInt(ogWidth);

                byte[] ogHeight = new byte[4]; //donné sur 4 octets
                for (int i = 0; i < 4; i++) ogHeight[i] = tab[22 + i];
                height = MyImage.EndianToInt(ogHeight);

                byte[] ogBpc = new byte[2]; //donné sur 2 octets
                for (int i = 0; i < 2; i++) ogBpc[i] = tab[26 + i];
                bpc = MyImage.EndianToInt(ogBpc);

                padding = width * 3 % 4;
            }
        }

        /// <summary>
        /// Création de l'entête d'une image à partir d'une matrice de Pixel
        /// </summary>
        /// <param name="pxm"></param>
        /// 
        public HeaderInfo(Pixel[,] pxm)
        {
            sizeOffset = 54; // tt le temps 54 ??
            //pxm = new Pixel[pxm.GetLength(0), pxm.GetLength(1)];
            height = pxm.GetLength(0); //nb de ligne
            width = pxm.GetLength(1);
            bpc = 1;
            padding = width * 3 % 4;
            size = sizeOffset + width * height * 3 + height * padding;

            array = new byte[sizeOffset];
            array[0] = 66;
            array[1] = 77;

            for (int i = 0; i < 4; i++)
            {
                #region Header du fichier

                array[2 + i] = MyImage.IntToEndian(size, 4)[i]; //taille du fichier bitmap (header inclus)
                array[6 + i] = 0; //reserved, can be 0 if created manually
                array[10 + i] = MyImage.IntToEndian(sizeOffset, 4)[i];

                #endregion

                #region Header de l'image

                array[14 + i] = MyImage.IntToEndian(40, 4)[i];
                array[18 + i] = MyImage.IntToEndian(width, 4)[i];
                array[22 + i] = MyImage.IntToEndian(height, 4)[i];
                array[30 + i] = 0; //Compression BI_RGB ; la plus commune
                array[34 + i] = MyImage.IntToEndian(width * height * 3 * bpc, 4)[i]; //taille de l'image en octets
                array[38 + i] = MyImage.IntToEndian(11811, 4)[i]; //?? résolution horizontale par défaut de l'image "Coco"
                array[42 + i] = MyImage.IntToEndian(11811, 4)[i]; //?? résolution verticale de l'image (pixel par mètre, cmmt savoir ça ?)
                array[46 + i] = 0; //nombre de couleurs dans la palette de couleur ?? peut-être 0 aussi
                array[50 + i] = 0; //nombre de couleurs "importantes" utilisées, 0 si elles le sont toutes

                #endregion
            }

            for (int i = 0; i < 2; i++)
            {
                array[26 + i] = MyImage.IntToEndian(1, 2)[i]; //bit par couleur
                array[28 + i] = MyImage.IntToEndian(24, 2)[i]; //pour une image de 24bit
            }


        }

        #endregion
    }
}
