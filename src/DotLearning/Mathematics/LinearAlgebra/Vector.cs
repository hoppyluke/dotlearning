using System;

namespace DotLearning.Mathematics.LinearAlgebra
{
    public partial class Vector : IEquatable<Vector>
    {
        private readonly double[] _items;

        /// <summary>
        /// Gets the number of items contained in this vector.
        /// </summary>
        public int Count => _items.Length;

        /// <summary>
        /// Calculates the magnitude (AKA length) of this vector.
        /// </summary>
        public double Magnitude
        {
            get
            {
                var magnitude = 0d;
                for (var i = 0; i < Count; i++)
                    magnitude += this[i] * this[i];

                return Math.Sqrt(magnitude);
            }
        }

        public double this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public Vector(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Vector size must be positive");
            _items = new double[count];
        }

        public Vector(double[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            _items = new double[items.Length];
            Array.Copy(items, _items, items.Length);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector other)
                return Equals(other);

            return false;
        }

        public bool Equals(Vector other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (Count != other.Count) return false;

            return ArrayFunctions.ContentEqual(_items, other._items);
        }

        public override int GetHashCode()
        {
            return ArrayFunctions.ContentHash(_items);
        }

        public override string ToString()
        {
            const int maxItems = 10;
            if (Count <= maxItems) return $"Vector[{Count}]: [{string.Join(", ", _items)}]";

            var itemsToShow = new ArraySegment<double>(_items, 0, maxItems);
            return $"Vector[{Count}]: [{string.Join(", ", itemsToShow)}, ...]";
        }
    }
}
