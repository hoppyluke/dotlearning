using System;

namespace DotLearning.Mathematics.LinearAlgebra
{
    public partial class Matrix : IEquatable<Matrix>
    {
        private readonly double[,] _items;

        public int Rows { get; }
        public int Columns { get; }

        public double this[int i, int j]
        {
            get => _items[i, j];

            set => _items[i, j] = value;
        }

        private Matrix(Matrix source)
            : this(source.Rows, source.Columns)
        {
        }

        public Matrix(int rows, int columns)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException(nameof(rows), "Matrix size must be non-negative");

            if (columns <= 0)
                throw new ArgumentOutOfRangeException(nameof(columns), "Matrix size must be non-negative");

            Rows = rows;
            Columns = columns;
            _items = new double[rows, columns];
        }

        public Matrix(double[,] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            Rows = values.GetLength(0);
            Columns = values.GetLength(1);
            _items = new double[Rows, Columns];

            Array.Copy(values, _items, values.Length);
        }

        public Matrix(double[] values, bool isColumnMatrix)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            Rows = isColumnMatrix ? values.Length : 1;
            Columns = isColumnMatrix ? 1 : values.Length;

            _items = new double[Rows, Columns];
            
            for(var k = 0; k < values.Length; k++)
            {
                var i = isColumnMatrix ? k : 0;
                var j = isColumnMatrix ? 0 : k;
                _items[i, j] = values[k];
            }
        }

        public Matrix Transpose()
        {
            var transpose = new Matrix(Columns, Rows);

            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Columns; j++)
                    transpose[j, i] = this[i, j];

            return transpose;
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix other)
                return Equals(other);

            return false;
        }

        public bool Equals(Matrix other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            if (other.Rows != Rows || other.Columns != Columns) return false;

            return ArrayFunctions.ContentEqual(_items, other._items);
        }
        
        public override int GetHashCode()
        {
            // The computed hash is not stable if the matrix is mutated
            return ArrayFunctions.ContentHash(_items);
        }
    }
}
