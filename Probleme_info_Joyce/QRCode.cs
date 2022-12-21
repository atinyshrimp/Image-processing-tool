using System;
using ReedSolomon;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{
    /// <summary>
    /// Classe gérant le fonctionnement d'un QRCode
    /// </summary>
    public class QRCode
    {
        #region Attributes

        private string mode = "0010"; //mode alphanumérique imposé pour le projet
        private int version; // 1 ou 2
        private string data;
        private byte[] dataBytes;
        private int errorCodeCount; //nombre d'octets pour le code d'erreur
        private byte[] errorCode;
        private int length;
        private string lengthBin;
        private int maxLength;
        private int maxBit;
        static private string[] filler = { "11101100", "00010001" };
        static private string formatInfoBits = "111011111000100"; //Masque de niveau 0 et code d'erreur L, donné par la respo module
        static private char[] character = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                                            'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/', ':'};

        private Pixel[,] code;
        static private bool[,] functionModules;
        static private Pixel[,] mask;
        static private string maskBits;
        private string bitChain;
        private int bitChainLength;

        #endregion

        #region Properties

        /// <summary>
        /// Ensemble des caractères pris en compte par le mode alphanumérique
        /// </summary>
        static public char[] Character { get => character; }

        /// <summary>
        /// Image du QR Code final
        /// </summary>
        public Pixel[,] Code { get => code; }

        /// <summary>
        /// Ensemble des bits à encoder dans l'image finale à retourner
        /// </summary>
        public string BitChain { get => bitChain; }

        /// <summary>
        /// Matrice de Pixels correspondant au masque à appliquer au QRCode
        /// </summary>
        public Pixel[,] Mask { get => mask; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Crée une instance de QR Code à partir d'une chaîne de caractères
        /// </summary>
        /// <param name="data">Mot à convertir en QR Code</param>
        public QRCode(string data)
        {
            if (data != "" && data != null)
            {
                data = data.ToUpper();
                bool alphanum = true;
                for (int i = 0; i < data.Length && alphanum; i++)
                    if (!character.Contains(data[i])) alphanum = false;

                if (alphanum)
                {
                    this.data = data;
                    dataBytes = new byte[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        dataBytes[i] = Convert.ToByte(data[i]);

                    length = data.Length;
                    lengthBin = MyImage.FillBinary(Convert.ToString(length, 2), 9); //nbre de caractères codé sur 9 bits en alphanumérique

                    if (data.Length <= 25)
                    {
                        version = 1;
                        code = new Pixel[21, 21];
                        functionModules = new bool[21, 21];
                        maxLength = 25; //25 char max pour le niveau de correction L (7%)
                        maxBit = 19 * 8; //19 octets max pour version 1 et erreur code L
                        errorCodeCount = 7; //7 octets pour le code d'erreur
                    }
                    else if (data.Length > 25 && data.Length <= 47)
                    {
                        version = 2;
                        code = new Pixel[25, 25];
                        functionModules = new bool[25, 25];
                        maxLength = 47; //same
                        maxBit = 34 * 8; //34 octets max pour version 1 et erreur code L
                        errorCodeCount = 10;
                    }

                    bitChain = mode + lengthBin;
                    FromStringToBit();
                    bitChainLength = bitChain.Length;
                    mask = DrawMask();
                    functionModules = SetModules();

                    code = DrawCode();
                }
            }
        }

        #endregion

        #region Method(s)

        #region Private methods

        private void AddTerminaison()
        {
            int cpt = 0;
            if (bitChain.Length < maxBit)
            {
                while (bitChain.Length < maxBit && cpt < 4) //on ne peut ajouter que quatres zéros maximum
                {
                    bitChain += "0";
                    cpt++;
                }
            }
        }

        private void FromStringToBit()
        {
            string[] pairs = GetPairs();
            for (int i = 0; i < pairs.Length; i++)
            {
                bitChain += Binary(pairs[i]);
            }

            AddTerminaison();

            if (bitChain.Length % 8 != 0)
                while (bitChain.Length % 8 != 0)
                    bitChain += "0";

            int cpt = 0;
            int maxi = (maxBit - bitChain.Length) / 8;
            while (cpt < maxi)
            {
                if (cpt == 0) bitChain += filler[0];
                else
                {
                    if (cpt % 2 == 0) bitChain += filler[0];
                    else bitChain += filler[1];
                }
                cpt++;
            }

            //ajout les bits de la correction de l'erreur
            dataBytes = new byte[bitChain.Length / 8];
            for (int i = 0; i < dataBytes.Length; i++)
            {
                string current = bitChain.Substring(8 * i, 8);
                dataBytes[i] = Convert.ToByte(current, 2);
            }

            errorCode = ReedSolomonAlgorithm.Encode(dataBytes, errorCodeCount, ErrorCorrectionCodeType.QRCode);

            for (int i = 0; i < errorCodeCount; i++)
                bitChain += MyImage.FillBinary(Convert.ToString(errorCode[i], 2), 8);
        }

        private void MotifsDeRecherche(Pixel[,] mat)
        {
            //placement des motifs de recherche
            for (int i = 0; i < 7; i++)
            {
                //en haut à gauche
                mat[i, 0] = Pixel.Black;
                mat[i, 6] = Pixel.Black;
                mat[6, i] = Pixel.Black;
                mat[0, i] = Pixel.Black;

                //en haut à droite
                mat[0, mat.GetLength(1) - 1 - i] = Pixel.Black;
                mat[6, mat.GetLength(1) - 1 - i] = Pixel.Black;
                mat[i, mat.GetLength(1) - 1] = Pixel.Black;
                mat[i, mat.GetLength(0) - 7] = Pixel.Black;

                //en bas à gauche
                mat[mat.GetLength(0) - 1 - i, 0] = Pixel.Black;
                mat[mat.GetLength(0) - 1 - i, 6] = Pixel.Black;
                mat[mat.GetLength(0) - 7, i] = Pixel.Black;
                mat[mat.GetLength(0) - 1, i] = Pixel.Black;

                //les carrés plein à l'intérieur de chaque carré
                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        mat[2 + k, 2 + l] = Pixel.Black;
                        mat[2 + k, mat.GetLength(1) - 3 - l] = Pixel.Black;
                        mat[mat.GetLength(0) - 3 - k, 2 + l] = Pixel.Black;
                    }
                }
            }

        }

        private void MotifsAlignement(Pixel[,] mat)
        {
            mat[18, 18] = Pixel.Black;
            for (int i = 0; i < 5; i++)
            {
                mat[16, 16 + i] = Pixel.Black;
                mat[20, 16 + i] = Pixel.Black;
                mat[16 + i, 16] = Pixel.Black;
                mat[16 + i, 20] = Pixel.Black;

            }
        }

        private void FormatInfo(Pixel[,] mat)
        {
            for (int i = 0; i < 9; i++)
            {
                mat[8, i] = Pixel.BlueColor;
                mat[i, 8] = Pixel.BlueColor;
            }

            for (int i = 0; i < 8; i++)
            {
                mat[mat.GetLength(0) - 1 - i, 8] = Pixel.BlueColor;
                mat[8, mat.GetLength(1) - 1 - i] = Pixel.BlueColor;
            }
        }

        private bool IsModuleFree(int x, int y)
        {
            int size = version == 1 ? 21 : 25;
            if (version == 2 && y >= 16 && y < 21 && x >= 16 && x < 21) return false;
            if (y > 8 && y < size - 8) return x != 6;
            if (x > 8 && x < size - 8) return y != 6;
            return x > 8 && y > 8;
        }

        private bool[,] SetModules()
        {
            bool[,] res = functionModules;
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(0); j++)
                {
                    res[i, j] = false;
                }
            }

            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(0); j++)
                {
                    if (i == 6 || j == 6) res[i, j] = true;
                    if (i < 9 && j < 9) res[i, j] = true;
                    if (i < 9 && j > res.GetLength(0) - 9) res[i, j] = true;
                    if (i > res.GetLength(0) - 9 && j < 9) res[i, j] = true;

                    if(version == 2)
                            if (i < 21 && i >= 16 && j < 21 && j >= 16) res[i, j] = true; 
                }
            }
            return res;
        }

        private Pixel[,] DrawMask()
        {
            Pixel[,] res = new Pixel[code.GetLength(0), code.GetLength(1)];
            maskBits = "";

            for (int i = res.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = res.GetLength(1) - 1; j >= 0; j--)
                {
                    res[i, j] = (i + j) % 2 == 0 ? Pixel.Black : Pixel.White;
                    maskBits += (i + j) % 2 == 0 ? '1' : '0';
                }
            }
            return res;
        }

        private Pixel[,] Padding(Pixel[,] mat)
        {
            Pixel[,] res = new Pixel[mat.GetLength(0) + 2, mat.GetLength(1) + 2];
            
            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = Pixel.White;

            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(1); j++)
                    res[i + 1, j + 1] = mat[i, j];

            return res;
        }

        private Pixel[,] DrawCode()
        {
            Pixel[,] res = new Pixel[code.GetLength(0), code.GetLength(1)];

            //remplissage pour que la matrice soit lisible et enregistrable
            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = Pixel.White;

            FormatInfo(res);

            //Motifs de synchronisation
            Pixel temp;
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0) temp = Pixel.Black;
                else temp = Pixel.White;

                res[6 + i, 6] = temp;
                res[6, 6 + i] = temp;
            }

            MotifsDeRecherche(res);

            if (version == 2)
                MotifsAlignement(res);

            res[4 * version + 9, 8] = Pixel.Black; //motif sombre commun à tous les QR Codes

            //entrée des données

            int cpt = 0;
            int qrSize = code.GetLength(0);
            for (int j = qrSize - 1; j >= 0 && cpt < bitChain.Length; j -= 2)
            {
                if (j == 6) j = 5;

                for (int i = qrSize - 1; i >= 0; i--)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        int y = j - k;
                        bool up = ((j + 1) & 2) == 0;
                        int x = up ? i : qrSize - 1 - i;

                        if (IsModuleFree(x, y))
                        {
                            try
                            {
                                res[x, y] = bitChain[cpt] == '1' ? Pixel.Black : Pixel.White;
                                cpt++;
                            }
                            catch { break; }
                        }
                    }
                }
            }

            //application du masque 0
            for (int i = 0; i < code.GetLength(0); i++)
            {
                for (int j = 0; j < code.GetLength(1); j++)
                {
                    temp = res[i, j];
                    if (IsModuleFree(i, j)) res[i, j] = Pixel.XOR(temp, mask[i, j]); //application de l'opération logique "ou exclusif"
                }
            }

            //remplissage des format info à côté des motifs de recherche
            for (int i = 0; i < 15; i++)
            {
                if (i < 7)
                {
                    res[code.GetLength(0) - 1 - i, 8] = formatInfoBits[i] == '1' ? Pixel.Black : Pixel.White;
                    if (i < 6)
                    {
                        if (formatInfoBits[i] == '1') res[8, i] = Pixel.Black;
                        else res[8, i] = Pixel.White;
                    }
                    else
                    {
                        if (formatInfoBits[i] == '1') res[8, i + 1] = Pixel.Black;
                        else res[8, i + 1] = Pixel.White;
                    }
                }
                else
                {
                    if (formatInfoBits[i] == '1') res[8, qrSize - 15 + i] = Pixel.Black;
                    else res[8, qrSize - 15 + i] = Pixel.White;

                    if (i < 9)
                    {
                        if (formatInfoBits[i] == '1') res[15 - i, 8] = Pixel.Black;
                        else res[15 - i, 8] = Pixel.White;
                    }
                    else
                    {
                        if (formatInfoBits[i] == '1') res[15 - 1 - i, 8] = Pixel.Black;
                        else res[15 - 1 - i, 8] = Pixel.White;
                    }
                }
            }


            return res;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Cherche à quel index la lettre passée en paramètres se trouve dans le tableau de caractères disponibles dans le mode alphanumérique
        /// </summary>
        /// <param name="lettre">Caractère à chercher dans le tabelau</param>
        /// <returns>Un entier correspondant à la place occupée par la lettre</returns>
        static public int FindIndex(char lettre)
        {
            int res = -1;
            if (character != null && character.Length > 0)
            {
                res = 0;
                bool found = false;
                for (int i = 0; i < character.Length && !found; i++)
                {
                    if (character[i] == lettre)
                    {
                        res = i;
                        found = true;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Sépare le mot à retranscrire en QRCode en paires
        /// </summary>
        /// <returns>Tableau de string où chaque index contient une chaîne de deux caractères (ou un seul pour le dernier index en fonction des cas)</returns>
        public string[] GetPairs()
        {
            string[] res = null;
            if (data != null && data.Length > 0)
            {
                string c = "";
                int i = 0;
                while (i < data.Length)
                {
                    if (i != 0 && i % 2 == 0) c += "~";
                    c += data[i];
                    i++;
                }
                res = c.Split('~'); //caractère séparateur non pris en compte par l'alphanumérique
            }
            return res;
        }

        /// <summary>
        /// Donne le nombre binaire correspondant à la paire de caractères passée en paramètres
        /// </summary>
        /// <param name="pair">Deux caractères à "traduire" en base 2</param>
        /// <returns></returns>
        static public string Binary(string pair)
        {
            string res = null;
            if (pair != null && (pair.Length == 1 || pair.Length == 2))
            {
                int temp = 0;
                int bitNumber = 11;

                if (pair.Length == 2)
                    for (int i = 0; i < 2; i++)
                        temp += (int)Math.Pow(45, 1 - i) * FindIndex(pair[i]);
                else temp = FindIndex(pair[0]);

                if (pair.Length == 1) bitNumber = 6;
                res = MyImage.FillBinary(Convert.ToString(temp, 2), bitNumber);
            }
            return res;
        }

        /// <summary>
        /// Applique un Padding de un Pixel au QRCode
        /// </summary>
        /// <returns></returns>
        public Pixel[,] PrintCode()
        {
            return Padding(code);
        }

        #endregion

        #endregion
    }
}
