﻿using System;
using System.Collections.Generic;
using DotLearning.Mathematics;
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
        public void Subtraction_ThrowsIfEitherVectorIsNull()
        {
            Vector u = null, v = new Vector(1);
            Assert.Throws<ArgumentNullException>(() => u - v);
            Assert.Throws<ArgumentNullException>(() => v - u);
            Assert.Throws<ArgumentNullException>(() => u - u);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 })]
        public void Subtraction_ReturnsElementwiseSubtraction(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);

            var w = u - v;

            for (var i = 0; i < vector1.Length; i++)
                Assert.Equal(u[i] - v[i], w[i]);
        }

        [Theory]
        [InlineData(new double[] { 1, 2 }, new double[] { 3, 4, 5 })]
        public void Subtraction_ThrowsForDifferentSizes(double[] vector1, double[] vector2)
        {
            var u = new Vector(vector1);
            var v = new Vector(vector2);
            Assert.Throws<ArgumentException>(() => u - v);
            Assert.Throws<ArgumentException>(() => v - u);
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

        [Fact]
        public void ExplicitConversionFromMatrix_HandlesNull()
        {
            Matrix m = null;
            var v = (Vector)m;
            Assert.Null(v);
        }

        [Theory]
        [InlineData(3, 2)]
        public void ExplicitConversionFromMatrix_ThrowsIfMatrixIsNotColumnMatrix(int rows, int columns)
        {
            var m = new Matrix(rows, columns);
            Assert.Throws<InvalidCastException>(() => (Vector)m);
        }

        [Theory]
        [InlineData(new double[] { 1 })]
        [InlineData(new double[] { 1, 2, 3 })]
        public void ExplicitConversionFromMatrix_ReturnsExpectedVector(double[] values)
        {
            var m = new Matrix(values, true);
            var v = (Vector)m;

            for (var i = 0; i < values.Length; i++)
                Assert.Equal(values[i], v[i]);
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

        #endregion

        #region Functions

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
        
        [Theory]
        [InlineData(new double[] {2}, 2d)]
        [InlineData(new double[] {3, 4}, 5d)]
        public void Magnitude_ReturnsCorrectResult(double[] vector, double expectedMagnitude)
        {
            var v = new Vector(vector);
            var actualLength = v.Magnitude;
            Assert.Equal(expectedMagnitude, actualLength);
        }

        [Fact]
        public void Apply_ThrowsIfParametersAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => Vector.Apply(x => x, null));
            Assert.Throws<ArgumentNullException>(() => Vector.Apply(null, new Vector(1)));
        }

        [Theory]
        [MemberData(nameof(ExamplesForApply))]
        public void Apply_ReturnsCorrectResult(Vector v, Func<double, double> f, Vector expected)
        {
            var result = Vector.Apply(f, v);
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public static void Zip_ThrowsIfEitherVectorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Vector.Zip(null, new Vector(1), (u, v) => 0d));
            Assert.Throws<ArgumentNullException>(() => Vector.Zip(new Vector(1), null, (u, v) => 0d));
            Assert.Throws<ArgumentNullException>(() => Vector.Zip(null, null, (u, v) => 0d));
        }

        [Fact]
        public static void Zip_ThrowsIfFunctionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Vector.Zip(new Vector(1), new Vector(1), null));
        }

        [Theory]
        [InlineData(1, 3)]
        public static void Zip_ThrowsIfVectorsAreDifferentSizes(int length1, int length2)
        {
            var u = new Vector(length1);
            var v = new Vector(length2);

            Assert.Throws<ArgumentException>(() => Vector.Zip(u, v, (x, y) => 0));
        }

        [Theory]
        [MemberData(nameof(ExamplesForZip))]
        public void Zip_AppliesFunctionPairwise(Vector u, Vector v, Func<double, double, double> f, Vector expected)
        {
            var result = Vector.Zip(u, v, f);
            Assert.Equal(expected, result);
        }

        #endregion

        [Theory]
        [InlineData(new double[] { 1 }, "Vector[1]: [1]")]
        [InlineData(new double[] { 1, 2, 3 }, "Vector[3]: [1, 2, 3]")]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "Vector[10]: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]")]
        public void ToString_ReturnsFullContentsForSmallVector(double[] v, string expected)
        {
            var vector = new Vector(v);
            var s = vector.ToString();
            Assert.Equal(expected, s);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, "Vector[11]: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, ...]")]
        public void ToString_TruncatesAfter10Items(double[] v, string expected)
        {
            var vector = new Vector(v);
            var s = vector.ToString();
            Assert.Equal(expected, s);
        }

        #region Examples

        public static IEnumerable<object[]> ExamplesForApply()
        {
            Func<double, double> @double = x => 2 * x;
            Func<double, double> identity = x => x;

            yield return new object[]
            {
                new Vector(new[] {1d}),
                @double,
                new Vector(new[] {2d})
            };

            yield return new object[]
            {
                new Vector(new[] {1d, 2d, 3d}),
                identity,
                new Vector(new[] {1d, 2d, 3d})
            };

            yield return new object[]
            {
                new Vector(new[] {1d, 2d, 3d}),
                @double,
                new Vector(new[] {2d, 4d, 6d})
            };
        }

        public static IEnumerable<object[]> ExamplesForZip()
        {
            yield return new object[]
            {
                new Vector(new[] { 1d, 2d, 3d}),
                new Vector(new[] { 4d, 5d, 6d}),
                FunctionalOperator.Add,
                new Vector(new[] { 5d, 7d, 9d})
            };

            yield return new object[]
            {
                new Vector(new[] { 4d, 5d, 6d}),
                new Vector(new[] { 3d, 2d, 1d}),
                FunctionalOperator.Subtract,
                new Vector(new[] { 1d, 3d, 5d})
            };
        }

        #endregion
    }
}
