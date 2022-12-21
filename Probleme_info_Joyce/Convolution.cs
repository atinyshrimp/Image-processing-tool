using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_Joyce
{
    /// <summary>
    /// Classe répertoriant les matrices de convolution utilisées pour le traitement d'image fait dans la classe MyImage
    /// </summary>
    public class Convolution
    {
        #region Attributes

        static private float[,] blur = {   { 0.11f, 0.11f, 0.11f},
                                           { 0.11f, 0.11f, 0.11f},
                                           { 0.11f, 0.11f, 0.11f} };

        static private float[,] sharpen = { { 0f, -0.5f, 0f},
                                            { -0.5f, 3f, -0.5f },
                                            { 0f, -0.5f, 0f } };

        static private float[,] embossing = { { -2f, -1f, 0f },
                                               { -1f, 1f, 1f },
                                               { 0f, 1f, 2f } };

        static private float[,] edgeDetect = { { 0f, 1f, 0f },
                                               { 1f, -4f, 1f },
                                               { 0f, 1f, 0f } };

        static private float[,] edgeDetectBis = { { -1f, -1f, -1f },
                                               { -1f, 8f, -1f },
                                               { -1f, -1f, -1f } };

        static private float[,] identity = { { 0f, 0f, 0f },
                                               { 0f, 1f, 0f },
                                               { 0f, 0f, 0f } };

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        static public float[,] Blur { get => blur; }

        /// <summary>
        /// 
        /// </summary>
        static public float[,] Sharpen { get => sharpen; }

        /// <summary>
        /// 
        /// </summary>
        static public float[,] EdgeDetect { get => edgeDetect; }

        /// <summary>
        /// 
        /// </summary>
        static public float[,] EdgeDetectBis { get => edgeDetectBis; }

        /// <summary>
        /// 
        /// </summary>
        static public float[,] Embossing { get => embossing; }

        /// <summary>
        /// 
        /// </summary>
        static public float[,] Identity { get => identity; }
        #endregion

    }
}
