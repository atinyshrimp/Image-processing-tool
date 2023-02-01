using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{

    /// <summary>
    /// Class managing the operation of pixels
    /// </summary>
    public class Pixel
    {
        #region Attributes

        private byte red;
        private byte green;
        private byte blue;

        #endregion

        #region Properties

        /// <summary>
        /// Pixel red component
        /// </summary>
        public byte Red 
        {
            get => red;
            set
            {
                if (value >= 0 && value <= 255) red = value;
            }
        }

        /// <summary>
        /// Pixel green component
        /// </summary>
        public byte Green
        {
            get => green;
            set
            {
                if (value >= 0 && value <= 255) green = value;
            }
        }

        /// <summary>
        /// Pixel blue component
        /// </summary>
        public byte Blue
        {
            get => blue;
            set
            {
                if (value >= 0 && value <= 255) blue = value;
            }
        }

        /// <summary>
        /// The average of the three color components
        /// </summary>
        public byte Avrg { get => (byte)((red + green + blue) / 3); }

        /// <summary>
        /// Indexer declaration of the Pixel class
        /// </summary>
        /// <param name="n">0 : Red component; 1 : Green component; 2 : Blue component</param>
        /// <returns>Byte corresponding to the selected componente</returns>
        public byte this[int n]
        {
            get
            {
                    if (n == 0) return red;
                    else if (n == 1) return green;
                    else if (n == 2) return blue;
                    else return 0;
            }
        }

        /// <summary>
        /// Pixel representing the black color
        /// </summary>
        static public Pixel Black { get => new Pixel(0, 0, 0); }

        /// <summary>
        /// Pixel representing the white color
        /// </summary>
        static public Pixel White { get => new(255, 255, 255); }

        /// <summary>
        /// Pixel representing the blue color
        /// </summary>
        static public Pixel BlueColor { get => new(0, 0, 255); }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a Pixel with a triplet of bytes corresponding to the red, green and blue components of a color
        /// </summary>
        /// <param name="red">Red component of the Pixel, between 0 and 255</param>
        /// <param name="green">Green component of the Pixel, between 0 and 255</param>
        /// <param name="blue">Blue component of the Pixel, between 0 and 255</param>
        public Pixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        #endregion

        #region Methods

        #region Utilitary methods

        /// <summary>
        /// Checks if two instances of Pixel are equal
        /// </summary>
        /// <param name="a">First pixel (comparator)</param>
        /// <param name="b">Second pixel (compared)</param>
        /// <returns>True if the two Pixels are the same, False otherwise</returns>
        static public bool operator ==(Pixel a, Pixel b)
        {
            return (a.red == b.red && a.green == b.green && a.blue == b.blue);
        }

        /// <summary>
        /// Checks if two Pixel instances are different
        /// </summary>
        /// <param name="a">First pixel (comparator)</param>
        /// <param name="b">Second pixel (compared)</param>
        /// <returns>True if the two Pixels are different, False otherwise</returns>

        static public bool operator !=(Pixel a, Pixel b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue">Queue containing all the bytes of the bitmap image</param>
        /// <returns>Pixel corresponding to the first three bytes in the Queue passed in parameters</returns>
        static public Pixel ReadPixel(Queue<byte> queue)
        {
            Pixel res = null;
            if (queue != null && queue.Count > 0)
            {
                // bitmap reading order
                byte b = queue.Dequeue();
                byte g = queue.Dequeue();
                byte r = queue.Dequeue();
                res = new Pixel(r, g, b);
            }
            return res;
        }

        /// <summary>
        /// Applies the "Exclusive Or" logical operation
        /// </summary>
        /// <param name="a">First entry</param>
        /// <param name="b">Second entry</param>
        /// <returns>Black Pixel if the two inputs are different, white Pixel otherwise</returns>
        static public Pixel XOR(Pixel a, Pixel b)
        {
            Pixel res;
            res = a != b ? Pixel.Black : Pixel.White; // if (a != b) { res = Pixel.Black; } else { res = Pixel.White; } ternary conditional operator
            return res;
        }

        #endregion

        #region Color changes

        /// <summary>
        /// Colors the current pixel in shades of gray
        /// </summary>
        /// <returns>Greyscale Pixel</returns>
        public Pixel Grayscale()
        {
            byte grey = Convert.ToByte(0.299 * red + 0.587 * green + 0.114 * blue);
            return new Pixel(grey, grey, grey);
        }

        /// <summary>
        /// Applies the "Negative" effect to the current instance of Pixel
        /// </summary>
        /// <returns>Pixel corresponding to the inverse color of the base Pixel</returns>
        public Pixel Negative()
        {
            return new Pixel((byte)(255 - red), (byte)(255 - green), (byte)(255 - blue));
        }
        #endregion

        #endregion
    }
}
