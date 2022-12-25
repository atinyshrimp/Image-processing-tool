using System.IO;

namespace PSI_Joyce
{

    /// <summary>
    /// Class containing all header information of a bitmap file
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
        /// Bitmap file size (including headers)
        /// </summary>
        public int Size { get => size; }

        /// <summary>
        /// The index where the bitmap of the image begins (54 in most cases)
        /// </summary>
        public int SizeOffset { get => sizeOffset; }

        /// <summary>
        /// Height of the image, given in pixels
        /// </summary>
        public int Height { get => height; }

        /// <summary>
        /// Width of the image, given in pixels
        /// </summary>
        public int Width { get => width; }

        /// <summary>
        /// Number of 0s to count to save the image properly in Bitmap file format
        /// </summary>
        public int Padding { get => padding; }

        /// <summary>
        /// Byte array containing all header bytes of the bitmap file
        /// </summary>
        public byte[] Array { get => array; }
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creating the header of an image from its path
        /// </summary>
        /// <param name="file">Path leading to the .bmp file from which the header will be created</param>
        public HeaderInfo(string file)
        {
            if (file.Contains(".bmp"))
            {

                byte[] tab = File.ReadAllBytes(file);

                byte[] ogSize = new byte[4]; // given in 4 bytes
                for (int i = 0; i < 4; i++) ogSize[i] = tab[2 + i];
                size = MyImage.EndianToInt(ogSize);

                byte[] ogSizeOffset = new byte[4]; // given in 4 bytes
                for (int i = 0; i < 4; i++) ogSizeOffset[i] = tab[10 + i];
                sizeOffset = MyImage.EndianToInt(ogSizeOffset);

                array = new byte[sizeOffset];
                for (int i = 0; i < sizeOffset; i++) array[i] = tab[i];

                byte[] ogWidth = new byte[4]; // given in 4 bytes
                for (int i = 0; i < 4; i++) ogWidth[i] = tab[18 + i];
                width = MyImage.EndianToInt(ogWidth);

                byte[] ogHeight = new byte[4]; // given in 4 bytes
                for (int i = 0; i < 4; i++) ogHeight[i] = tab[22 + i];
                height = MyImage.EndianToInt(ogHeight);

                byte[] ogBpc = new byte[2]; // given in 2 bytes
                for (int i = 0; i < 2; i++) ogBpc[i] = tab[26 + i];
                bpc = MyImage.EndianToInt(ogBpc);

                padding = width * 3 % 4;
            }
        }

        /// <summary>
        /// Creation of the header of an image from a Pixel matrix
        /// </summary>
        /// <param name="pxm">Pixel array representing an image</param>
        /// 
        public HeaderInfo(Pixel[,] pxm)
        {
            sizeOffset = 54; // most of the time 54
            // pxm = new Pixel[pxm.GetLength(0), pxm.GetLength(1)];
            height = pxm.GetLength(0); // nb of lines
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

                array[2 + i] = MyImage.IntToEndian(size, 4)[i]; // bitmap file size (including headers)
                array[6 + i] = 0; // reserved, can be 0 if created manually
                array[10 + i] = MyImage.IntToEndian(sizeOffset, 4)[i];

                #endregion

                #region Header de l'image

                array[14 + i] = MyImage.IntToEndian(40, 4)[i];
                array[18 + i] = MyImage.IntToEndian(width, 4)[i];
                array[22 + i] = MyImage.IntToEndian(height, 4)[i];
                array[30 + i] = 0; // BI_RGB compression ; the most common
                array[34 + i] = MyImage.IntToEndian(width * height * 3 * bpc, 4)[i]; // image size, in bytes
                array[38 + i] = MyImage.IntToEndian(11811, 4)[i]; // default horizontal resolution
                array[42 + i] = MyImage.IntToEndian(11811, 4)[i]; // default vertical resolution (pixel per meter)
                array[46 + i] = 0; // nb of colors within the color palette ?? can also be 0
                array[50 + i] = 0; // nb of "important" colors used, 0 if they are all important

                #endregion
            }

            for (int i = 0; i < 2; i++)
            {
                array[26 + i] = MyImage.IntToEndian(1, 2)[i]; // bit per color
                array[28 + i] = MyImage.IntToEndian(24, 2)[i]; // for a 24bit image
            }


        }

        #endregion
    }
}
