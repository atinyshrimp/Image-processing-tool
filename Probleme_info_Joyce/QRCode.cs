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
    /// Class managing the operation of a QRCode
    /// </summary>
    public class QRCode
    {
        #region Attributes

        private string mode = "0010"; // alphanumeric mode imposed for the project
        private int version; // 1 or 2
        private string data;
        private byte[] dataBytes;
        private int errorCodeCount; // number of bytes for error code
        private byte[] errorCode;
        private int length;
        private string lengthBin;
        private int maxLength;
        private int maxBit;
        static private string[] filler = { "11101100", "00010001" };
        static private string formatInfoBits = "111011111000100"; // Level 0 mask and error code L, given by the respo module (imposed)
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
        /// Set of characters taken into account by the alphanumeric mode
        /// </summary>
        static public char[] Character { get => character; }

        /// <summary>
        /// Image of the final QR Code
        /// </summary>
        public Pixel[,] Code { get => code; }

        /// <summary>
        /// Set of bits to be encoded in the final image to be returned
        /// </summary>
        public string BitChain { get => bitChain; }

        /// <summary>
        /// Pixel matrix corresponding to the mask to apply to the QRCode
        /// </summary>
        public Pixel[,] Mask { get => mask; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creates a QR Code instance from a character string
        /// </summary>
        /// <param name="data">Word to convert to QR Code</param>
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
                    lengthBin = MyImage.FillBinary(Convert.ToString(length, 2), 9); // number of characters coded on 9 bits in alphanumeric

                    if (data.Length <= 25)
                    {
                        version = 1;
                        code = new Pixel[21, 21];
                        functionModules = new bool[21, 21];
                        maxLength = 25; // 25 char max for correction level L (7%)
                        maxBit = 19 * 8; // 19 bytes max for version 1 and error code L
                        errorCodeCount = 7; // 7 bytes for error code
                    }
                    else if (data.Length > 25 && data.Length <= 47)
                    {
                        version = 2;
                        code = new Pixel[25, 25];
                        functionModules = new bool[25, 25];
                        maxLength = 47; // same
                        maxBit = 34 * 8; // 34 bytes max for version 1 and error code L
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
                while (bitChain.Length < maxBit && cpt < 4) // we can only add a maximum of four zeros
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

            // adding error correction bits
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

        private void FinderPatterns(Pixel[,] mat)
        {
            // placement des motifs de recherche
            for (int i = 0; i < 7; i++)
            {
                // top left
                mat[i, 0] = Pixel.Black;
                mat[i, 6] = Pixel.Black;
                mat[6, i] = Pixel.Black;
                mat[0, i] = Pixel.Black;

                // top right
                mat[0, mat.GetLength(1) - 1 - i] = Pixel.Black;
                mat[6, mat.GetLength(1) - 1 - i] = Pixel.Black;
                mat[i, mat.GetLength(1) - 1] = Pixel.Black;
                mat[i, mat.GetLength(0) - 7] = Pixel.Black;

                // bottom left
                mat[mat.GetLength(0) - 1 - i, 0] = Pixel.Black;
                mat[mat.GetLength(0) - 1 - i, 6] = Pixel.Black;
                mat[mat.GetLength(0) - 7, i] = Pixel.Black;
                mat[mat.GetLength(0) - 1, i] = Pixel.Black;

                // the filled squares inside each square
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

        private void AlignmentPatterns(Pixel[,] mat)
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

            // padding so that the matrix is readable and savable
            for (int i = 0; i < res.GetLength(0); i++)
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = Pixel.White;

            FormatInfo(res);

            // Timing patterns
            Pixel temp;
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0) temp = Pixel.Black;
                else temp = Pixel.White;

                res[6 + i, 6] = temp;
                res[6, 6 + i] = temp;
            }

            FinderPatterns(res);

            if (version == 2)
                AlignmentPatterns(res);

            res[4 * version + 9, 8] = Pixel.Black; // dark pattern common to all QR Codes

            // data entry

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

            // Application of Mask 0
            for (int i = 0; i < code.GetLength(0); i++)
            {
                for (int j = 0; j < code.GetLength(1); j++)
                {
                    temp = res[i, j];
                    if (IsModuleFree(i, j)) res[i, j] = Pixel.XOR(temp, mask[i, j]); // application of the "exclusive or" logical operation
                }
            }

            // fill in format infos next to finder patterns
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
        /// Finds the index at which the letter passed in parameters is found in the char array of available characters in alphanumeric mode
        /// </summary>
        /// <param name="lettre">Character to look for in the array</param>
        /// <returns>An integer corresponding to the space occupied by the letter</returns>
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
        /// Separates the word to be transcribed into QRCode in pairs
        /// </summary>
        /// <returns>Array of string where each index contains a string of two characters (or only one for the last index depending on the case)</returns>
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
                res = c.Split('~'); // separator character not taken into account by the alphanumeric
            }
            return res;
        }

        /// <summary>
        /// Gives the binary number corresponding to the pair of characters passed in parameters
        /// </summary>
        /// <param name="pair">Two characters to transcribe in base 2</param>
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
        /// Applies a one-pixel padding to the QRCode
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
