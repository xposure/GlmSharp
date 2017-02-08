using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Atma.Swizzle;

// ReSharper disable InconsistentNaming

namespace Atma
{
    /// <summary>
    /// Static class that contains static glm functions
    /// </summary>
    public static partial class glm
    {
        
        /// <summary>
        /// Creates a 2D array with all values (address: Values[x, y])
        /// </summary>
        public static double[,] Values(dmat2 m) => m.Values;
        
        /// <summary>
        /// Creates a 1D array with all values (internal order)
        /// </summary>
        public static double[] Values1D(dmat2 m) => m.Values1D;
        
        /// <summary>
        /// Returns an enumerator that iterates through all fields.
        /// </summary>
        public static IEnumerator<double> GetEnumerator(dmat2 m) => m.GetEnumerator();

    }
}
