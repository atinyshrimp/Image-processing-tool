using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{
    /// <summary>
    /// Class used to manipulate complex numbers, to be used when creating the fractal
    /// </summary>
    public class Complex
    {

        #region Attributes

        private readonly float real;
        private readonly float imaginary;

        #endregion

        #region Properties

        /// <summary>
        /// Real part of the current complex number
        /// </summary>
        public float Real { get => real; }

        /// <summary>
        /// Imaginary part of the current complex number
        /// </summary>
        public float Imaginary { get => imaginary; }

        /// <summary>
        /// Module of the current complex number
        /// </summary>
        public float Module { get => (float)Math.Sqrt(Math.Pow(real, 2) + Math.Pow(imaginary, 2)); }

        /// <summary>
        /// Hyperbolic sine of the complex number
        /// </summary>
        public Complex Sinh { get => new Complex((float)(Math.Sinh(real) * Math.Cos(imaginary)), (float)(Math.Cosh(real) * Math.Sin(imaginary))); }

        /// <summary>
        /// Sine of the current complex number
        /// </summary>
        public Complex Sin { get => new Complex((float)(Math.Sin(real) * Math.Cosh(imaginary)), (float)(Math.Cos(real) * Math.Sinh(imaginary))); }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an instance of Complex using the provided parts
        /// </summary>
        /// <param name="real">Real part of the complex number</param>
        /// <param name="imaginary">Imaginary part of the complex number</param>
        public Complex(float real, float imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies an integer power to the complex number of the current instance
        /// </summary>
        /// <param name="power">Power to apply</param>
        /// <returns>A Complex object corresponding to the original Complex to the power-th power</returns>
        public Complex Pow(int power)
        {
            Complex res = null;
            if (power > 1)
            {
                res = this;
                for (int i = 1; i < power; i++) // could've done that differently; res = 1, then res *= this with i <= power
                    res *= this;
            }
            else if (power == 1) res = this;
            else if (power == 0) res = new(1, 0); // same as res = new Complex(1,0);
            return res;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Multiply two Complex numbers together
        /// </summary>
        /// <param name="a">Multiplier</param>
        /// <param name="b">Multiplicand</param>
        /// <returns>Product of the two Complex objects</returns>
        static public Complex operator *(Complex a, Complex b) // so we can apply the Power operation (with "*=")
        {
            Complex res = null;
            res = new Complex(a.real * b.real - a.imaginary * b.imaginary, a.real * b.imaginary + a.imaginary * b.real);
            return res;
        }

        /// <summary>
        /// Applies division to two Complex numbers
        /// </summary>
        /// <param name="a">Dividend</param>
        /// <param name="b">Divisor</param>
        /// <returns>Quotient of the two Complex objects</returns>
        static public Complex operator /(Complex a, Complex b)
        {
            Complex res = new((float)((a.real * b.real + a.imaginary * b.imaginary) / (Math.Pow(b.real, 2) + Math.Pow(b.imaginary, 2))), (float)((a.real * b.imaginary - a.imaginary * b.real) / (Math.Pow(b.real, 2) + Math.Pow(b.imaginary, 2))));
            return res;
        }

        /// <summary>
        /// Divide a complex number by an integer
        /// </summary>
        /// <param name="a">Complex dividend</param>
        /// <param name="b">Integer divisor</param>
        /// <returns>Quotient of Complex a by the integer b</returns>
        static public Complex operator /(Complex a, int b)
        {
            Complex res = new(a.real / b, a.imaginary / b);
            return res;
        }

        /// <summary>
        /// Adds two Complex numbers together
        /// </summary>
        /// <param name="a">First term of the addition</param>
        /// <param name="b">Second term of the addition</param>
        /// <returns>Sum of the two Complex objects</returns>
        static public Complex operator +(Complex a, Complex b)
        {
            Complex res = null;
            res = new Complex(a.real + b.real, a.imaginary + b.imaginary);
            return res;
        }

        /// <summary>
        /// Adds an integer to a complex number
        /// </summary>
        /// <param name="a">Complex term of the addition</param>
        /// <param name="b">Integer term of the addition</param>
        /// <returns>Sum of Complex a with the integer b</returns>
        static public Complex operator +(Complex a, int b)
        {
            Complex res = null;
            res = new Complex(a.real + b, a.imaginary);
            return res;
        }

        /// <summary>
        /// Subtract two Complex numbers from each other
        /// </summary>
        /// <param name="a">Minuend</param>
        /// <param name="b">Subtrahend</param>
        /// <returns>Complex corresponding to the difference of Complex a and Complex b</returns>
        static public Complex operator -(Complex a, Complex b)
        {
            Complex res = new(a.real - b.real, a.imaginary - b.imaginary);
            return res;
        }

        #endregion

    }
}
