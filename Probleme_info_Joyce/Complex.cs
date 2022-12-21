using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{
    /// <summary>
    /// Classe servant à manipuler les nombres complexes, à utiliser lors de la création de la fractale
    /// </summary>
    public class Complex
    {

        #region Attributes

        private readonly float real;
        private readonly float imaginary;

        #endregion

        #region Properties

        /// <summary>
        /// Partie réelle du nombre complexe courant
        /// </summary>
        public float Real { get => real; }

        /// <summary>
        /// Partie imaginary du nombre complexe courant
        /// </summary>
        public float Imaginary { get => imaginary; }

        /// <summary>
        /// Module du nombre complexe
        /// </summary>
        public float Module { get => (float)Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imaginary, 2)); }

        /// <summary>
        /// Sinus hyperbolique du nombre complexe
        /// </summary>
        public Complex Sinh { get => new Complex((float)(Math.Sinh(real) * Math.Cos(imaginary)), (float)(Math.Cosh(real) * Math.Sin(imaginary))); }

        /// <summary>
        /// Sinus du nombre complexe
        /// </summary>
        public Complex Sin { get => new Complex((float)(Math.Sin(real) * Math.Cosh(imaginary)), (float)(Math.Cos(real) * Math.Sinh(imaginary))); }

        #endregion

        #region Constructors

        /// <summary>
        /// Crée une instance de Complex grâce aux parties fournies
        /// </summary>
        /// <param name="real">Partie réelle du nombre complexe</param>
        /// <param name="imaginary">Partie imaginary du nombre complexe</param>
        public Complex(float real, float imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applique une puissance entière au nombre complexe de l'instance courante
        /// </summary>
        /// <param name="power">Puissance à appliquer</param>
        /// <returns>Un Complex correspondant au Complex de base à la power-ième puissance</returns>
        public Complex Pow(int power)
        {
            Complex res = null;
            if (power > 1)
            {
                res = this;
                for (int i = 1; i < power; i++) // could've done that differently; res = 1, puis res *= this avec i <= power
                    res *= this;
            }
            else if (power == 1) res = this;
            else if (power == 0) res = new(1, 0); // same as res = new Complex(1,0);
            return res;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Multiplie deux nombres complexes entre eux
        /// </summary>
        /// <param name="a">Premier terme de la multiplication</param>
        /// <param name="b">Deuxième terme de la multiplication</param>
        /// <returns>Produit des deux nombres complexes</returns>
        static public Complex operator *(Complex a, Complex b) //so we can apply the Power operation (with "*=")
        {
            Complex res = null;
            res = new Complex(a.real * b.real - a.imaginary * b.imaginary, a.real * b.imaginary + a.imaginary * b.real);
            return res;
        }

        /// <summary>
        /// Applique la division à deux nombres complexes
        /// </summary>
        /// <param name="a">Premier terme de la division</param>
        /// <param name="b">Deuxième terme de la division</param>
        /// <returns>Quotient des deux nombres complexes</returns>
        static public Complex operator /(Complex a, Complex b)
        {
            Complex res = new((float)((a.real * b.real + a.imaginary * b.imaginary) / (Math.Pow(b.real, 2) + Math.Pow(b.imaginary, 2))), (float)((a.real * b.imaginary - a.imaginary * b.real) / (Math.Pow(b.real, 2) + Math.Pow(b.imaginary, 2))));
            return res;
        }

        /// <summary>
        /// Divise un nombre complexe par un entier
        /// </summary>
        /// <param name="a">Premier terme de la division</param>
        /// <param name="b">Deuxième terme de la division</param>
        /// <returns>Quotient du Complex a par l'entier b</returns>
        static public Complex operator /(Complex a, int b)
        {
            Complex res = new(a.real / b, a.imaginary / b);
            return res;
        }

        /// <summary>
        /// Additionne deux nombres complexes entre eux
        /// </summary>
        /// <param name="a">Premier terme de l'addition</param>
        /// <param name="b">Deuxième terme de l'addition</param>
        /// <returns>Somme des deux nombres complexes</returns>
        static public Complex operator +(Complex a, Complex b)
        {
            Complex res = null;
            res = new Complex(a.real + b.real, a.imaginary + b.imaginary);
            return res;
        }

        /// <summary>
        /// Aditionne un entier à un nombre complexe
        /// </summary>
        /// <param name="a">Premier terme de l'addition</param>
        /// <param name="b">Deuxième terme de l'addition</param>
        /// <returns>Somme du Complex a avec l'entier b</returns>
        static public Complex operator +(Complex a, int b)
        {
            Complex res = null;
            res = new Complex(a.real + b, a.imaginary);
            return res;
        }

        /// <summary>
        /// Soustrait deux nombres complexes entre eux
        /// </summary>
        /// <param name="a">Premier terme de la soustraction</param>
        /// <param name="b">Deuxième terme de la soustraction</param>
        /// <returns>Complex correspondant à la différence du Complex a et du Complex b</returns>
        static public Complex operator -(Complex a, Complex b)
        {
            Complex res = new(a.real - b.real, a.imaginary - b.imaginary);
            return res;
        }

        #endregion

    }
}
