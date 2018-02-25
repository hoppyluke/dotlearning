using System;

namespace DotLearning.Mathematics.LinearAlgebra
{
    public partial class Matrix
    {
        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        public static Matrix operator *(Matrix m, double c)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));

            var n = new Matrix(m);

            for (var i = 0; i < m.Rows; i++)
                for (var j = 0; j < m.Columns; j++)
                    n[i, j] = m[i, j] * c;
            
            return n;
        }

        /// <summary>
        /// Scalar multiplication.
        /// </summary>
        public static Matrix operator *(double c, Matrix m) => m * c;

        /// <summary>
        /// Matrix addition.
        /// </summary>
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException($"Cannot add a {b.Rows}x{b.Columns} matrix to a {a.Rows}x{a.Columns} matrix");

            var sum = new Matrix(a);

            for (var i = 0; i < a.Rows; i++)
                for (var j = 0; j < a.Columns; j++)
                    sum[i, j] = a[i, j] + b[i, j];

            return sum;
        }

        /// <summary>
        /// Matrix multiplication.
        /// </summary>
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (a.Columns != b.Rows)
                throw new ArgumentException($"Cannot multiply a {a.Rows}x{a.Columns} matrix by a {b.Rows}x{b.Columns} matrix");

            var product = new Matrix(a.Rows, b.Columns);

            for (var i = 0; i < a.Rows; i++)
            {
                for (var j = 0; j < b.Columns; j++)
                {
                    var sum = 0d;

                    for(var k = 0; k < a.Columns; k++)
                        sum += a[i, k] * b[k, j];

                    product[i, j] = sum;
                }
            }

            return product;
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            if (ReferenceEquals(a, null))
                return !ReferenceEquals(b, null);

            if (ReferenceEquals(b, null)) return true;

            return ArrayFunctions.ContentNotEqual(a._items, b._items);
        }
    }
}
