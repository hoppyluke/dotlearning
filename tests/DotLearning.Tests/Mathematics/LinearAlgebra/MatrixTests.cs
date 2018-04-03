using System;
using System.Collections.Generic;
using DotLearning.Mathematics.LinearAlgebra;
using Xunit;

namespace DotLearning.Tests.Mathematics.LinearAlgebra
{
    public class MatrixTests
    {
        #region Constructors

        [Fact]
        public void Constructor_FromSize_SetsExpectedDimensions()
        {
            var m = new Matrix(3, 4);

            Assert.Equal(3, m.Rows);
            Assert.Equal(4, m.Columns);
        }

        [Theory]
        [InlineData(2, 0)]
        [InlineData(0, 2)]
        [InlineData(2, -1)]
        [InlineData(-2, 1)]
        public void Constructor_FromSize_ThrowsIfNonPositive(int rows, int columns)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix(rows, columns));
        }

        [Fact]
        public void Constructor_From2dArray_ThrowsIfNull()
        {
            double[,] values = null;
            Assert.Throws<ArgumentNullException>(() => new Matrix(values));
        }

        [Fact]
        public void Constructor_From2dArray_SetsAllValues()
        {
            var values = new double[,]
            {
                { 1d, 2d, 3d},
                { 4d, 5d, 6d}
            };

            var m = new Matrix(values);

            Assert.Equal(2, m.Rows);
            Assert.Equal(3, m.Columns);

            Assert.Equal(1d, m[0, 0]);
            Assert.Equal(2d, m[0, 1]);
            Assert.Equal(3d, m[0, 2]);
            Assert.Equal(4d, m[1, 0]);
            Assert.Equal(5d, m[1, 1]);
            Assert.Equal(6d, m[1, 2]);
        }

        [Fact]
        public void Constructor_From1dArray_ThrowsIfNull()
        {
            double[] array = null;
            Assert.Throws<ArgumentNullException>(() => new Matrix(array, true));
            Assert.Throws<ArgumentNullException>(() => new Matrix(array, false));
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4 })]
        public void Constructor_From1dArray_SetsSize(double[] array)
        {
            var rowMatrix = new Matrix(array, false);
            var columnMatrix = new Matrix(array, true);
            Assert.Equal(1, rowMatrix.Rows);
            Assert.Equal(array.Length, rowMatrix.Columns);
            Assert.Equal(1, columnMatrix.Columns);
            Assert.Equal(array.Length, columnMatrix.Rows);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4 })]
        public void Constructor_From1dArray_SetsValues(double[] array)
        {
            var rowMatrix = new Matrix(array, false);
            var columnMatrix = new Matrix(array, true);
            
            for(var i = 0; i < array.Length; i++)
            {
                Assert.Equal(array[i], rowMatrix[0, i]);
                Assert.Equal(array[i], columnMatrix[i, 0]);
            }
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4 })]
        public void Constructor_From1dArray_CopiesValues(double[] array)
        {
            var rowMatrix = new Matrix(array, false);
            var columnMatrix = new Matrix(array, true);

            array[0] = double.MinValue;

            Assert.NotEqual(double.MinValue, rowMatrix[0, 0]);
            Assert.NotEqual(double.MinValue, columnMatrix[0, 0]);
        }

        #endregion

        #region Element access

        [Fact]
        public void SetItem_ThenGet_ReturnsSameValue()
        {
            var m = new Matrix(1, 2);
            var x = 0.5d;
            m[0, 1] = x;
            var y = m[0, 1];
            Assert.Equal(x, y);
        }

        [Theory]
        [MemberData(nameof(AllIndices), 3, 5)]
        public void AllIndicesAreAccessible(int i, int j)
        {
            var m = new Matrix(3, 5);
            var x = 3 * j + i;
            m[i, j] = x;
            var y = m[i, j];
            Assert.Equal(x, y);
        }

        #endregion

        #region Equality

        [Fact]
        public void Equals_ReturnsFalse_ForNullObject()
        {
            var m = new Matrix(1, 1);
            object o = null;
            Assert.False(m.Equals(o));
        }

        [Fact]
        public void Equals_ReturnsFalse_ForNonMatrix()
        {
            var m = new Matrix(1, 1);
            object o = "hello";
            Assert.False(m.Equals(o));
        }

        [Fact]
        public void Equals_ReturnsFalse_ForNullMatrix()
        {
            var a = new Matrix(1, 1);
            Matrix b = null;
            Assert.False(a.Equals(b));
        }

        [Theory]
        [InlineData(1, 1, 2, 2)]
        [InlineData(3, 2, 2, 3)]
        public void Equals_ReturnsFalse_ForDifferentSizedMatrix(int m1, int n1, int m2, int n2)
        {
            var a = new Matrix(m1, n1);
            var b = new Matrix(m2, n2); 
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
        }
        
        [Theory]
        [MemberData(nameof(UnequalMatrixExamples))]
        public void Equals_ReturnsFalse_ForMatricesWithDifferentValues(Matrix a, Matrix b)
        {
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
        }

        [Theory]
        [MemberData(nameof(EqualMatrixExamples))]
        public void Equals_ReturnsTrue_ForMatricesWithSameValues(Matrix a, Matrix b)
        {
            Assert.False(ReferenceEquals(a, b));
            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
        }

        [Fact]
        public void EqualsOperator_ReturnsTrueForNullMatrices()
        {
            Matrix a = null, b = null;
            Assert.True(a == b);
        }

        [Fact]
        public void EqualsOperator_ReturnsFalseForOneNullMatrix()
        {
            Matrix a = null, b = new Matrix(1, 1);
            Assert.False(a == b);
            Assert.False(b == a);
        }

        [Theory]
        [InlineData(1, 1, 2, 2)]
        [InlineData(3, 2, 2, 3)]
        public void Equals_OperatorReturnsFalse_ForDifferentSizedMatrix(int m1, int n1, int m2, int n2)
        {
            var a = new Matrix(m1, n1);
            var b = new Matrix(m2, n2);
            Assert.False(a == b);
            Assert.False(b == a);
        }

        [Theory]
        [MemberData(nameof(EqualMatrixExamples))]
        public void EqualsOperator_ReturnsTrueForEqualMatrices(Matrix a, Matrix b)
        {
            Assert.True(a == b);
            Assert.True(b == a);
        }

        [Theory]
        [MemberData(nameof(UnequalMatrixExamples))]
        public void EqualsOperator_ReturnsFalseForNonEqualMatrices(Matrix a, Matrix b)
        {
            Assert.False(a == b);
            Assert.False(b == a);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsFalseForNullMatrices()
        {
            Matrix a = null, b = null;
            Assert.False(a != b);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsTrueForOneNullMatrix()
        {
            Matrix a = null, b = new Matrix(1, 1);
            Assert.True(a != b);
            Assert.True(b != a);
        }

        [Fact]
        public void NotEqualsOperator_ReturnsTrueForDifferentSizedMatrices()
        {
            var a = new Matrix(1, 1);
            var b = new Matrix(2, 2);

            Assert.True(a != b);
        }

        [Theory]
        [MemberData(nameof(EqualMatrixExamples))]
        public void NotEqualsOperator_ReturnsFalseForEqualMatrices(Matrix a, Matrix b)
        {
            Assert.False(a != b);
            Assert.False(b != a);
        }

        [Theory]
        [MemberData(nameof(UnequalMatrixExamples))]
        public void NotEqualsOperator_ReturnsTrueForNonEqualMatrices(Matrix a, Matrix b)
        {
            Assert.True(a != b);
            Assert.True(b != a);
        }

        [Theory]
        [MemberData(nameof(EqualMatrixExamples))]
        public void GetHashCode_ReturnsSameValueForEqualMatricies(Matrix a, Matrix b)
        {
            var hashA = a.GetHashCode();
            var hashB = b.GetHashCode();

            Assert.True(hashA == hashB);
        }
        
        #endregion

        #region Scalar/matrix operations

        [Fact]
        public void ScalarMultiplication_ThrowsIfNull()
        {
            Matrix m = null;

            Assert.Throws<ArgumentNullException>(() => m * 1d);
        }

        [Fact]
        public void ScalarMultiplication_MultiplesAllElements()
        {
            var m = new Matrix(new double[,]
            {
                { 1, 2, 3 },
                {4, 5, 6 }
            });

            var n = m * 2d;

            Assert.Equal(2d, n[0, 0]);
            Assert.Equal(4d, n[0, 1]);
            Assert.Equal(6d, n[0, 2]);
            Assert.Equal(8d, n[1, 0]);
            Assert.Equal(10d, n[1, 1]);
            Assert.Equal(12d, n[1, 2]);
        }

        [Fact]
        public void ScalarMultiplication_IsCommutative()
        {
            var m = new Matrix(new double[,]
            {
                { 1, 2, 3 },
                {4, 5, 6 }
            });

            var n1 = m * 2d;
            var n2 = 2d * m;

            Assert.Equal(n1[0, 0], n2[0, 0]);
            Assert.Equal(n1[0, 1], n2[0, 1]);
            Assert.Equal(n1[0, 2], n2[0, 2]);
            Assert.Equal(n1[1, 0], n2[1, 0]);
            Assert.Equal(n1[1, 1], n2[1, 1]);
            Assert.Equal(n1[1, 2], n2[1, 2]);
        }

        [Fact]
        public void Addition_ThrowsIfNull()
        {
            Matrix m = new Matrix(1, 1), n = null;
            Assert.Throws<ArgumentNullException>(() => m + n);
            Assert.Throws<ArgumentNullException>(() => n + m);
        }

        [Theory]
        [InlineData(3, 2, 2, 3)]
        [InlineData(1, 1, 2, 2)]
        public void Addition_ThrowsIfDimensionsNotEqual(int m1, int n1, int m2, int n2)
        {
            var m = new Matrix(m1, n1);
            var n = new Matrix(m2, n2);

            Assert.Throws<ArgumentException>(() => m + n);
        }

        [Fact]
        public void Addition_ReturnsElementwiseSum()
        {
            var m = new Matrix(new double[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            });

            var n = new Matrix(new double[,]
            {
                { 10, 20, 30 },
                { 40, 50, 60 },
                { 70, 80, 90 }
            });

            var s = m + n;

            Assert.Equal(11, s[0, 0]);
            Assert.Equal(22, s[0, 1]);
            Assert.Equal(33, s[0, 2]);
            Assert.Equal(44, s[1, 0]);
            Assert.Equal(55, s[1, 1]);
            Assert.Equal(66, s[1, 2]);
            Assert.Equal(77, s[2, 0]);
            Assert.Equal(88, s[2, 1]);
            Assert.Equal(99, s[2, 2]);
        }
        
        [Fact]
        public void Subtraction_ThrowsIfNull()
        {
            Matrix m = new Matrix(1, 1), n = null;
            Assert.Throws<ArgumentNullException>(() => m - n);
            Assert.Throws<ArgumentNullException>(() => n - m);
        }

        [Theory]
        [InlineData(3, 2, 2, 3)]
        [InlineData(1, 1, 2, 2)]
        public void Subtraction_ThrowsIfDimensionsNotEqual(int m1, int n1, int m2, int n2)
        {
            var m = new Matrix(m1, n1);
            var n = new Matrix(m2, n2);

            Assert.Throws<ArgumentException>(() => m - n);
        }

        [Fact]
        public void Subtraction_ReturnsElementwiseResult()
        {
            var m = new Matrix(new double[,]
            {
                { 10, 20, 30 },
                { 40, 50, 60 },
                { 70, 80, 90 }
            });

            var n = new Matrix(new double[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            });

            var s = m - n;

            Assert.Equal(9, s[0, 0]);
            Assert.Equal(18, s[0, 1]);
            Assert.Equal(27, s[0, 2]);
            Assert.Equal(36, s[1, 0]);
            Assert.Equal(45, s[1, 1]);
            Assert.Equal(54, s[1, 2]);
            Assert.Equal(63, s[2, 0]);
            Assert.Equal(72, s[2, 1]);
            Assert.Equal(81, s[2, 2]);
        }

        [Fact]
        public void MatrixMultiplication_ThrowsIfNull()
        {
            Matrix m = new Matrix(1, 1), n = null;
            Assert.Throws<ArgumentNullException>(() => m * n);
            Assert.Throws<ArgumentNullException>(() => n * m);
        }


        [Theory]
        [InlineData(3, 2, 3, 2)]
        [InlineData(1, 1, 2, 2)]
        public void MatrixMultiplication_ThrowsIfDimensionsDotNotMatch(int m1, int n1, int m2, int n2)
        {
            var m = new Matrix(m1, n1);
            var n = new Matrix(m2, n2);

            Assert.Throws<ArgumentException>(() => m * n);
        }

        [Theory]
        [InlineData(3, 2, 2, 3)]
        [InlineData(2, 4, 4, 5)]
        public void MatrixMultiplication_ReturnsCorrectDimensions(int m1, int n1, int m2, int n2)
        {
            var a = new Matrix(m1, n1);
            var b = new Matrix(m2, n2);
            var product = a * b;

            Assert.Equal(m1, product.Rows);
            Assert.Equal(n2, product.Columns);
        }

        [Theory]
        [MemberData(nameof(MatrixProductExamples))]
        public void MatrixMultiplication_ReturnsCorrectValues(Matrix a, Matrix b, Matrix expected)
        {
            var product = a * b;

            for (var j = 0; j < expected.Columns; j++)
                for (var i = 0; i < expected.Rows; i++)
                    Assert.Equal(expected[i, j], product[i, j]);
        }

        #endregion

        [Fact]
        public void Transpose_ReturnsCorrectDimensions()
        {
            var m = new Matrix(3, 2);
            var t = m.Transpose();
            Assert.Equal(m.Columns, t.Rows);
            Assert.Equal(m.Rows, t.Columns);
        }

        [Fact]
        public void Transpose_ReturnsCorrectValues()
        {
            var m = new Matrix(new double[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 }
            });

            var t = m.Transpose();
            
            Assert.Equal(1, t[0, 0]);
            Assert.Equal(3, t[0, 1]);
            Assert.Equal(5, t[0, 2]);
            Assert.Equal(2, t[1, 0]);
            Assert.Equal(4, t[1, 1]);
            Assert.Equal(6, t[1, 2]);
        }

        public static IEnumerable<object[]> AllIndices(int rows, int columns)
        {
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    yield return new object[] { i, j };
        }

        public static IEnumerable<object[]> UnequalMatrixExamples()
        {
            yield return new object[]
            {
                new Matrix(new double[,] { {1, 2}, {3, 4} }),
                new Matrix(new double[,] { {1, 2}, {5, 4} })
            };
        }

        public static IEnumerable<object[]> EqualMatrixExamples()
        {
            yield return new object[]
            {
                new Matrix(new double[,] { {1, 2}, {3, 4} }),
                new Matrix(new double[,] { {1, 2}, {3, 4} })
            };
        }

        public static IEnumerable<object[]> MatrixProductExamples()
        {
            yield return new object[]
            {
                new Matrix(new double[,] { {1,2,3}, {4,5,6} }),
                new Matrix(new double[,] { {1,2,3,4}, {5,6,7,8}, {9,10,11,12} }),
                new Matrix(new double[,] { {38,44,50,56}, {83,98,113,128}})
            };
        }
    }
}
