using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{

    /// <summary>
    /// Classe gérant le fonctionnement des pixels
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
        /// Composante rouge du Pixel
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
        /// Composante verte du Pixel
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
        /// Composante bleue du Pixel
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
        /// La moyenne des trois composantes
        /// </summary>
        public byte Avrg { get => (byte)((red + green + blue) / 3); }

        /// <summary>
        /// Indexation de la classe Pixel
        /// </summary>
        /// <param name="n">0 : Composante rouge; 1 : Verte; 2 : Bleue</param>
        /// <returns>Byte correspondant à la composante selectionnée</returns>
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
        /// Pixel représentant la couleur noire
        /// </summary>
        static public Pixel Black { get => new Pixel(0, 0, 0); }

        /// <summary>
        /// Pixel représentant la couleur blanche
        /// </summary>
        static public Pixel White { get => new(255, 255, 255); }

        /// <summary>
        /// Pixel représentant la couleur bleue
        /// </summary>
        static public Pixel BlueColor { get => new(0, 0, 255); }

        #endregion

        #region Constructors

        /// <summary>
        /// Construit un Pixel, triplet de bytes correspondant aux composantes rouge, verte et bleu d'une couleur
        /// </summary>
        /// <param name="red">Composante rouge du Pixel, comprise entre 0 et 255</param>
        /// <param name="green">Composante verte du Pixel, comprise entre 0 et 255</param>
        /// <param name="blue">Composante bleue du Pixel, comprise entre 0 et 255</param>
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
        /// Vérifie si deux instances de Pixel sont égales
        /// </summary>
        /// <param name="a">Premier pixel (comparateur)</param>
        /// <param name="b">Deuxième pixel (comparé)</param>
        /// <returns>True si les deux Pixels sont les mêmes, False sinon</returns>
        static public bool operator ==(Pixel a, Pixel b)
        {
            bool res = false;
            if (a.red == b.red && a.green == b.green && a.blue == b.blue) res = true;
            return res;
        }

        /// <summary>
        /// Vérifie si deux instances de Pixel sont différentes
        /// </summary>
        /// <param name="a">Premier pixel (comparateur)</param>
        /// <param name="b">Deuxième pixel (comparé)</param>
        /// <returns>True si les deux Pixels sont différents, False sinon</returns>

        static public bool operator !=(Pixel a, Pixel b)
        {
            bool res = false;
            if (a.red != b.red || a.green != b.green || a.blue != b.blue) res = true;
            return res;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue">Queue contenant tous les bytes de l'image bitmap</param>
        /// <returns>Pixel correspondant aux trois premiers bytes dans la Queue passée en paramètres</returns>
        static public Pixel ReadPixel(Queue<byte> queue)
        {
            Pixel res = null;
            if (queue != null && queue.Count > 0)
            {
                //ordre lu en bitmap
                byte b = queue.Dequeue();
                byte g = queue.Dequeue();
                byte r = queue.Dequeue();
                res = new Pixel(r, g, b);
            }
            return res;
        }

        /// <summary>
        /// Applique l'opération logique "Ou exclusif"
        /// </summary>
        /// <param name="a">Première entrée</param>
        /// <param name="b">Deuxième entrée</param>
        /// <returns>Pixel noir si les deux entrées sont différentes, Pixel blanc sinon</returns>
        static public Pixel XOR(Pixel a, Pixel b)
        {
            Pixel res;
            res = a != b ? Pixel.Black : Pixel.White; //if (a != b) { res = Pixel.Black; } else { res = Pixel.White; } opérateur conditionel ternaire
            return res;
        }

        #endregion

        #region Color changes

        /// <summary>
        /// Colore le Pixel courant en nuance de gris
        /// </summary>
        /// <returns>Pixel grisé</returns>
        public Pixel Grayscale()
        {
            byte grey = Convert.ToByte(0.299 * red + 0.587 * green + 0.114 * blue);
            return new Pixel(grey, grey, grey);
        }

        /// <summary>
        /// Applique l'effet "Négatif" à l'instance courante de Pixel
        /// </summary>
        /// <returns>Pixel correspondant à la couleur inverse du Pixel de base</returns>
        public Pixel Negatif()
        {
            return new Pixel((byte)(255 - red), (byte)(255 - green), (byte)(255 - blue));
        }
        #endregion

        #endregion
    }
}
