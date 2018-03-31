using System;

namespace DotLearning.Mathematics.LinearAlgebra
{
    public partial class Vector
    {
        public static Vector operator *(Vector v, double c)
        {
            if (v == null) throw new ArgumentNullException(nameof(v));

            var u = new Vector(v.Count);

            for (var i = 0; i < v.Count; i++)
                u[i] = v[i] * c;

            return u;
        }

        public static Vector operator *(double c, Vector v) => v * c;

        public static Vector operator +(Vector u, Vector v)
        {
            ValidateVectors(u, v, "Cannot add a vector of size {1} to a vector of size {0}");

            var w = new Vector(u.Count);

            for (var i = 0; i < u.Count; i++)
                w[i] = u[i] + v[i];

            return w;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        public static double operator *(Vector u, Vector v)
        {
            ValidateVectors(u, v, "Cannot calculate dot product of vectors of size {0} and {1}");

            var dotProduct = 0d;

            for (var i = 0; i < u.Count; i++)
                dotProduct += u[i] * v[i];

            return dotProduct;
        }

        public static bool operator ==(Vector v, Vector u)
        {
            if (ReferenceEquals(v, null))
                return ReferenceEquals(u, null);

            return v.Equals(u);
        }

        public static bool operator !=(Vector v, Vector u)
        {
            if (ReferenceEquals(v, null))
                return !ReferenceEquals(u, null);

            if (ReferenceEquals(u, null)) return true;

            return ArrayFunctions.ContentNotEqual(v._items, u._items);
        }

        public static implicit operator Matrix(Vector v)
        {
            if (v == null) return null;
            return new Matrix(v._items, true);
        }

        /// <summary>
        /// Converts a column matrix into a vector.
        /// </summary>
        /// <param name="m">A single-column matrix.</param>
        public static explicit operator Vector(Matrix m)
        {
            if (m == null) return null;
            if (m.Columns != 1) throw new InvalidCastException("Matrix with multiple columns cannot be cast to a Vector");

            var v = new Vector(m.Rows);
            for (var i = 0; i < m.Rows; i++)
                v[i] = m[i, 0];

            return v;
        }

        public static Vector HadamardProduct(Vector u, Vector v)
        {
            ValidateVectors(u, v, "Cannot calculate Hadamard product for vectors of size {0} and {1}");

            var product = new Vector(u.Count);

            for (var i = 0; i < u.Count; i++)
                product[i] = u[i] * v[i];

            return product;
        }

        /// <summary>
        /// Applies a function element-wise to a vector.
        /// </summary>
        /// <param name="f">Function to vectorise.</param>
        /// <param name="v">Vector to apply function to.</param>
        /// <returns>A new vector defined by u[i] = f(v[i]).</returns>
        public static Vector Apply(Func<double, double> f, Vector v)
        {
            if (f == null) throw new ArgumentNullException(nameof(f));
            if (v == null) throw new ArgumentNullException(nameof(v));

            var result = new Vector(v.Count);
            for (var i = 0; i < v.Count; i++)
                result[i] = f(v[i]);

            return result;
        }

        /// <summary>
        /// Ensures both vectors are non-null and have the same Count.
        /// </summary>
        private static void ValidateVectors(Vector u, Vector v, string sizeMismatchError)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            if (v == null) throw new ArgumentNullException(nameof(v));

            if (u.Count != v.Count)
                throw new ArgumentException(string.Format(sizeMismatchError, u.Count, v.Count), nameof(v));
        }
    }
}
