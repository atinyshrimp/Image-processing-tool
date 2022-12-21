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
    public class Complexe
    {

        #region Attributes

        private readonly float reelle;
        private readonly float imaginaire;

        #endregion

        #region Properties

        /// <summary>
        /// Partie réelle du nombre complexe courant
        /// </summary>
        public float Reelle { get => reelle; }

        /// <summary>
        /// Partie imaginaire du nombre complexe courant
        /// </summary>
        public float Imaginaire { get => imaginaire; }

        /// <summary>
        /// Module du nombre complexe
        /// </summary>
        public float Module { get => (float)Math.Sqrt(Math.Pow(reelle, 2) + Math.Pow(imaginaire, 2)); }

        /// <summary>
        /// Sinus hyperbolique du nombre complexe
        /// </summary>
        public Complexe Sinh { get => new Complexe((float)(Math.Sinh(reelle) * Math.Cos(imaginaire)), (float)(Math.Cosh(reelle) * Math.Sin(imaginaire))); }

        /// <summary>
        /// Sinus du nombre complexe
        /// </summary>
        public Complexe Sin { get => new Complexe((float)(Math.Sin(reelle) * Math.Cosh(imaginaire)), (float)(Math.Cos(reelle) * Math.Sinh(imaginaire))); }

        #endregion

        #region Constructors

        /// <summary>
        /// Crée une instance de Complexe grâce aux parties fournies
        /// </summary>
        /// <param name="reelle">Partie réelle du nombre complexe</param>
        /// <param name="imaginaire">Partie imaginaire du nombre complexe</param>
        public Complexe(float reelle, float imaginaire)
        {
            this.reelle = reelle;
            this.imaginaire = imaginaire;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applique une puissance entière au nombre complexe de l'instance courante
        /// </summary>
        /// <param name="power">Puissance à appliquer</param>
        /// <returns>Un Complexe correspondant au Complexe de base à la power-ième puissance</returns>
        public Complexe Pow(int power)
        {
            Complexe res = null;
            if (power > 1)
            {
                res = this;
                for (int i = 1; i < power; i++) //j'aurais pu faire ça autrement :/ res = 1, puis res *= this avec i <= power
                    res *= this;
            }
            else if (power == 1) res = this;
            else if (power == 0) res = new(1, 0); //pareil que res = new Complexe(1,0);
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
        static public Complexe operator *(Complexe a, Complexe b) //pour faire en sorte que la puissance puisse réalisable (notamment avec "*=")
        {
            Complexe res = null;
            res = new Complexe(a.reelle * b.reelle - a.imaginaire * b.imaginaire, a.reelle * b.imaginaire + a.imaginaire * b.reelle);
            return res;
        }

        /// <summary>
        /// Applique la division à deux nombres complexes
        /// </summary>
        /// <param name="a">Premier terme de la division</param>
        /// <param name="b">Deuxième terme de la division</param>
        /// <returns>Quotient des deux nombres complexes</returns>
        static public Complexe operator /(Complexe a, Complexe b)
        {
            Complexe res = new((float)((a.reelle * b.reelle + a.imaginaire * b.imaginaire) / (Math.Pow(b.reelle, 2) + Math.Pow(b.imaginaire, 2))), (float)((a.reelle * b.imaginaire - a.imaginaire * b.reelle) / (Math.Pow(b.reelle, 2) + Math.Pow(b.imaginaire, 2))));
            return res;
        }

        /// <summary>
        /// Divise un nombre complexe par un entier
        /// </summary>
        /// <param name="a">Premier terme de la division</param>
        /// <param name="b">Deuxième terme de la division</param>
        /// <returns>Quotient du Complexe a par l'entier b</returns>
        static public Complexe operator /(Complexe a, int b)
        {
            Complexe res = new(a.reelle / b, a.imaginaire / b);
            return res;
        }

        /// <summary>
        /// Additionne deux nombres complexes entre eux
        /// </summary>
        /// <param name="a">Premier terme de l'addition</param>
        /// <param name="b">Deuxième terme de l'addition</param>
        /// <returns>Somme des deux nombres complexes</returns>
        static public Complexe operator +(Complexe a, Complexe b)
        {
            Complexe res = null;
            res = new Complexe(a.reelle + b.reelle, a.imaginaire + b.imaginaire);
            return res;
        }

        /// <summary>
        /// Aditionne un entier à un nombre complexe
        /// </summary>
        /// <param name="a">Premier terme de l'addition</param>
        /// <param name="b">Deuxième terme de l'addition</param>
        /// <returns>Somme du Complexe a avec l'entier b</returns>
        static public Complexe operator +(Complexe a, int b)
        {
            Complexe res = null;
            res = new Complexe(a.reelle + b, a.imaginaire);
            return res;
        }

        /// <summary>
        /// Soustrait deux nombres complexes entre eux
        /// </summary>
        /// <param name="a">Premier terme de la soustraction</param>
        /// <param name="b">Deuxième terme de la soustraction</param>
        /// <returns>Complexe correspondant à la différence du Complexe a et du Complexe b</returns>
        static public Complexe operator -(Complexe a, Complexe b)
        {
            Complexe res = new(a.reelle - b.reelle, a.imaginaire - b.imaginaire);
            return res;
        }

        #endregion

    }
}
