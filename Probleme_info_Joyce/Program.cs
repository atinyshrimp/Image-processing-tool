using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Media;
using System.Diagnostics;
using System.IO;


namespace PSI_Joyce
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        #region Méthodes utilitaires

        /// <summary>
        /// Affiche un tableau de bytes (méthode utilisée dans les tests)
        /// </summary>
        /// <param name="tab">tableau de bytes à afficher</param>
        /// <returns></returns>
        public static string AfficheTableau(byte[] tab)
        {
            string res = "";
            if (tab != null && tab.Length > 0)
            {
                for (int i = 0; i < tab.Length; i++) res += tab[i] + " ";
            }
            return res;
        }

        /// <summary>
        /// Affiche un tableau de string (méthode utilisée dans les tests)
        /// </summary>
        /// <param name="tab">tableau de chaînes de caractères à afficher</param>
        /// <returns></returns>
        public static string AfficheTableau(string[] tab)
        {
            string res = "";
            if (tab != null && tab.Length > 0)
            {
                for (int i = 0; i < tab.Length; i++) res += tab[i] + " ";
            }
            return res;
        }

        #endregion

        static void Main(string[] args)
        {
            QRCode test = new("HELLO WORLD HELLO WORLD HELLO");
            //MyImage test2 = new("Hello_World.bmp");

            //Console.WriteLine(test2.ReadQRCode());

            MyImage qrCode = new(test.Code);
            MyImage mask = new(test.Mask);
            mask = new(mask.Scale(100));
            qrCode = new(qrCode.MirrorX());
            qrCode = new(qrCode.Scale(100));
            qrCode.FromImageToFile("Hello world v2.bmp");

            /*            MyImage fractal = new(MyImage.Fractal("Sin Julia"));
                        fractal.FromImageToFile("JuliaFractal.bmp");
            */
            /*            MyImage test = new("TEST3C.bmp");
                        test.FromImageToFile("testPadding.bmp");
            */
        }
    }
}
