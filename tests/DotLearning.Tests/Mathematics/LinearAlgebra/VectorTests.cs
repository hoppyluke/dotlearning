using System;
using DotLearning.Mathematics.LinearAlgebra;
using Xunit;

namespace DotLearning.Tests.Mathematics.LinearAlgebra
{
    public class VectorTests
    {
        #region Constructors

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_FromInt_ThrowsIfNonPositive(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector(size));
        }

        [Fact]
        public void Constructor_FromInt_SetsCount()
        {
            var v = new Vector(5);
            Assert.Equal(5, v.Count);
        }

        [Fact]
        public void Constructor_FromArray_ThrowsIfNull()
        {
            double[] a = null;
            Assert.Throws<ArgumentNullException>(() => new Vector(a));
        }

        [Fact]
        public void Constructor_FromArray_SetsCount()
        {
            var a = new double[10];
            var v = new Vector(a);
            Assert.Equal(a.Length, v.Count);
        }

        [Fact]
        public void Constructor_FromArray_CopiesArray()
        {
            var a = new double[] { 1, 2, 3 };
            var v = new Vector(a);
            a[1] = 4;
            Assert.Equal(1, v[0]);
            Assert.Equal(2, v[1]);
            Assert.Equal(3, v[2]);
        }

        #endregion

        #region Operators

        [Theory]
        [InlineData(new double[] { 1, 2, 3 }, 2)]
        public void ScalarMultiplication_PerformsElementwiseMultiplication(double[] vector, double scalar)
        {
            var v = new Vector(vector);
            var u = v * scalar;

            for (var i = 0; i < vector.Length; i++)
                Assert.Equal(vector[i] * scalar, u[i]);
        }

        [Fact]
        public void ScalarMultiplication_ThrowsIfNull()
        {
            Vector v = null;
            var s = 3d;
            Assert.Throws<ArgumentNullException>(() => v * s);
        }

        [Fact]
        public void ScalarMultiplication_IsCommutative()
        {
            var v = new Vector(new double[] { 1, 2, 3 });
            var s = 3d;
            var u1 = v * s;
            var u2 = s * v;

            for (var i = 0; i < v.Count; i++)
                Assert.Equal(u1[i], u2[i]);
        }

        [Fact]
        public void Addition_ThrowsIfEitherVectorIsNull()
        {
            Vector u = null, v = new Vector(1);
            Assert.Throws<ArgumentNullException>(() => u + v);
            Assert.Throws<ArgumentNullException>(() => v + u);
            Assert.Throws<ArgumentNullException>(() => u + u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 })]
        public void Addition_ReturnsElementwiseSum(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);

            var w = u + v;

            for (var i = 0; i < vector1.Length; i++)
                Assert.Equal(u[i] + v[i], w[i]);
        }

        [Theory]
        [InlineData(new double[] {1, 2}, new double[] {3, 4, 5})]
        public void Addition_ThrowsForDifferentSizes(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);
            Assert.Throws<ArgumentException>(() => u + v);
            Assert.Throws<ArgumentException>(() => v + u);
        }

        [Fact]
        public void DotProduct_ThrowsIfEitherVectorIsNull()
        {
            Vector u = null, v = new Vector(1);
            Assert.Throws<ArgumentNullException>(() => u * v);
            Assert.Throws<ArgumentNullException>(() => v * u);
            Assert.Throws<ArgumentNullException>(() => u * u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2 }, new double[] { 3, 4, 5 })]
        public void DotProduct_ThrowsForDifferentSizes(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);
            Assert.Throws<ArgumentException>(() => u * v);
            Assert.Throws<ArgumentException>(() => v * u);
        }

        [Theory]
        [InlineData(new double[] { 1, 3, -5 }, new double[] { 4, -2, -1 }, 3)]
        public void DotProduct_CalculatesCorrectValues(double[] vector1, double[] vector2, double expectedProduct)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);
            var p = u * v;
            Assert.Equal(expectedProduct, p);
        }

        [Fact]
        public void ImplicitConversionToMatrix_HandlesNull()
        {
            Vector v = null;
            Matrix m = v;
            Assert.Null(m);
        }

        [Theory]
        [InlineData(new double[] { 2, 4, 6, 8 })]
        public void ImplicitConversionToMatrix_ReturnsCorrectlySizedMatrix(double[] vector)
        {
            var v = new Vector(vector);
            Matrix m = v;
            Assert.Equal(1, m.Columns);
            Assert.Equal(vector.Length, m.Rows);
        }

        #endregion

        #region Equality

        [Theory]
        [InlineData("not vector")]
        public void Equals_ReturnsFalse_ForNonVector(object other)
        {
            var v = new Vector(3);
            Assert.False(v.Equals(other));
        }

        [Fact]
        public void Equals_ReturnsFalse_ForOneNull()
        {
            var v = new Vector(1);
            Vector u = null;
            object o = null;

            Assert.False(v == u);
            Assert.False(v.Equals(o));
            Assert.False(v.Equals(u));
        }

        [Theory]
        [InlineData(new double[] { 1 }, new double[] { 2, 3 })]
        public void Equals_ReturnsFalse_ForDifferentSizes(double[] vector1, double[] vector2)
        {
            var v = new Vector(vector1);
            var u = new Vector(vector2);
            object o = u;

            Assert.False(v == u);
            Assert.False(v.Equals(o));
            Assert.False(v.Equals(u));
        }

        [Theory]
        [InlineData(new double[] { 1, 2 }, new double[] { 3, 4 })]
        public void Equals_ReturnsFalse_ForDifferentVectors(double[] vector1, double[] vector2)
        {
            var v = new Vector(vector1);
            var u = new Vector(vector2);
            object o = u;

            Assert.False(v == u);
            Assert.False(v.Equals(o));
            Assert.False(v.Equals(u));
        }

        [Fact]
        public void Equals_ReturnsTrue_ForBothNull()
        {
            Vector v = null, u = null;
            Assert.True(v == u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 })]
        public void Equals_ReturnsTrue_ForEqualVector(double[] contents)
        {
            var v = new Vector(contents);
            var u = new Vector(contents);
            var o = (object)u;
            
            Assert.True(v.Equals(u));
            Assert.True(v.Equals(o));
            Assert.True(v == u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 })]
        public void Equals_ReturnsTrue_ForSelf(double[] contents)
        {
            var v = new Vector(contents);
            object o = v;

            Assert.True(v.Equals(v));
            Assert.True(v.Equals(o));

#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
            Assert.True(v == o);
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(v == v);
#pragma warning restore CS1718 // Comparison made to same variable
        }
        
        [Fact]
        public void NotEquals_ReturnsTrue_ForOneNull()
        {
            var v = new Vector(1);
            Vector u = null;
            Assert.True(v != u);
            Assert.True(u != v);
        }

        [Fact]
        public void NotEquals_ReturnsFalse_ForBothNull()
        {
            Vector v = null, u = null;
            Assert.False(v != u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2 }, new double[] { 3 })]
        public void NotEquals_ReturnsTrue_ForDifferentSizes(double[] vector1, double[] vector2)
        {
            var v = new Vector(vector1);
            var u = new Vector(vector2);
            Assert.True(v != u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2 }, new double[] { 3, 4 })]
        public void NotEquals_ReturnsTrue_ForDifferentVectors(double[] vector1, double[] vector2)
        {
            var v = new Vector(vector1);
            var u = new Vector(vector2);
            Assert.True(v != u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 })]
        public void NotEquals_ReturnsFalse_ForSameVector(double[] contents)
        {
            var v = new Vector(contents);
            var u = new Vector(contents);
            Assert.False(v != u);
        }

        [Fact]
        public void HadamardProduct_ThrowsIfEitherVectorIsNull()
        {
            Vector u = null, v = new Vector(1);

            Assert.Throws<ArgumentNullException>(() => Vector.HadamardProduct(u, v));
            Assert.Throws<ArgumentNullException>(() => Vector.HadamardProduct(v, u));
            Assert.Throws<ArgumentNullException>(() => Vector.HadamardProduct(u, u));
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 }, new double[] { 1, 2 })]
        public void HadamardProduct_ThrowsForDifferentSizes(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);

            Assert.Throws<ArgumentException>(() => Vector.HadamardProduct(u, v));
            Assert.Throws<ArgumentException>(() => Vector.HadamardProduct(v, u));
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 }, new double[] { 4, 10, 18 })]
        public void HadamardProduct_ReturnsCorrectVector(double[] vector1, double[] vector2, double[] expectedProduct)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);

            var product = Vector.HadamardProduct(u, v);

            Assert.Equal(u.Count, product.Count);

            for (var i = 0; i < product.Count; i++)
                Assert.Equal(expectedProduct[i], product[i]);
        }

        #endregion

        [Theory]
        [InlineData(new double[] {2}, 2d)]
        [InlineData(new double[] {3, 4}, 5d)]
        public void Magnitude_ReturnsCorrectResult(double[] vector, double expectedMagnitude)
        {
            var v = new Vector(vector);
            var actualLength = v.Magnitude;
            Assert.Equal(expectedMagnitude, actualLength);
        }
    }
}
