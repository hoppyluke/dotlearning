using System;
using System.Runtime.CompilerServices;

namespace DotLearning.Mathematics.LinearAlgebra
{
    /// <summary>
    /// Utility methods for working arrays.
    /// </summary>
    internal static class ArrayFunctions
    {
        /// <summary>
        /// Computes a hashcode for an array based on its contents.
        /// In the case where the array's element type is <see cref="double"/>,
        /// the hashcode is consistent with equality as defined by <see cref="ContentEqual(double[], double[])"/>.
        /// </summary>
        /// <typeparam name="T">Type of elements in the array.</typeparam>
        /// <param name="a">Array to compute hashcode for.</param>
        /// <returns>Hashcode which is not stable if the array is modified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ContentHash<T>(T[] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            var hash = 17;

            for (var i = 0; i < a.Length; i++)
                hash = hash * 23 + a[i].GetHashCode();

            return hash;
        }

        /// <summary>
        /// Computes a hashcode for a 2D array based on its contents.
        /// In the case where the array's element type is <see cref="double"/>,
        /// the hashcode is consistent with equality as defined by <see cref="ContentEqual(double[,], double[,])"/>.
        /// </summary>
        /// <typeparam name="T">Type of elements in the array.</typeparam>
        /// <param name="a">Array to compute hashcode for.</param>
        /// <returns>Hashcode which is not stable if the array is modified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ContentHash<T>(T[,] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            var hash = 17;

            var iMax = a.GetLength(0);
            var jMax = a.GetLength(1);

            for (var i = 0; i < iMax; i++)
                for (var j = 0; j < jMax; j++)
                    hash = hash * 23 + a[i, j].GetHashCode();

            return hash;
        }

        /// <summary>
        /// Element-wise equality for arrays.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns>True if and only if both arrays have the same length and contents, or are both null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContentEqual(double[] a, double[] b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            if (a.Length != b.Length) return false;

            for (var i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;

            return true;
        }

        /// <summary>
        /// Element-wise equality for arrays.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns>True if and only if both arrays have the same dimensions and contents, or are both null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContentEqual(double[,] a, double[,] b)
        {
            if (a == null) return b == null;
            if (b == null) return false;

            var iMax = a.GetLength(0);
            var jMax = a.GetLength(1);

            if (b.GetLength(0) != iMax || b.GetLength(1) != jMax) return false;

            for (var i = 0; i < iMax; i++)
                for (var j = 0; j < jMax; j++)
                    if (a[i, j] != b[i, j]) return false;
            
            return true;
        }

        /// <summary>
        /// Element-wise inequality for arrays.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns>True if the arrays have different lengths, different contents or one is null (and the other non-null).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContentNotEqual(double[] a, double[] b)
        {
            if (a == null) return b != null;
            if (b == null) return true;
            if (a.Length != b.Length) return true;

            for (var i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return true;

            return false;
        }

        /// <summary>
        /// Element-wise inequality for 2D arrays.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns>True if the arrays have different dimensions, different contents or one is null (and the other non-null).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContentNotEqual(double[,] a, double[,] b)
        {
            if (a == null) return b != null;
            if (b == null) return true;
            if (a.Length != b.Length) return true;

            var iMax = a.GetLength(0);
            var jMax = a.GetLength(1);

            for (var i = 0; i < iMax; i++)
                for (var j = 0; j < jMax; j++)
                    if (a[i, j] != b[i, j]) return true;
            
            return false;
        }
    }
}
