using System;
using System.Collections.Generic;
using System.Text;
using DotLearning.Mathematics.LinearAlgebra;
using Xunit;

namespace DotLearning.Tests.Mathematics.LinearAlgebra
{
    public class ArrayFunctionsTests
    {
        [Fact]
        public void ContentHash_Throws_ForNull1dArray()
        {
            double[] a = null;
            Assert.Throws<ArgumentNullException>(() => ArrayFunctions.ContentHash(a));
        }

        [Theory]
        [MemberData(nameof(Equal1dArrayExamples), false)]
        public void ContentHash_ReturnsSameValue_ForEqual1dArrays(double[] a, double[] b)
        {
            var hashA = ArrayFunctions.ContentHash(a);
            var hashB = ArrayFunctions.ContentHash(b);

            Assert.Equal(hashA, hashB);
        }

        [Fact]
        public void ContentHash_Throws_ForNull2dArray()
        {
            double[,] a = null;
            Assert.Throws<ArgumentNullException>(() => ArrayFunctions.ContentHash(a));
        }

        [Theory]
        [MemberData(nameof(Equal2dArrayExamples), false)]
        public void ContentHash_ReturnsSameValue_ForEqual2dArrays(double[,] a, double[,] b)
        {
            var hashA = ArrayFunctions.ContentHash(a);
            var hashB = ArrayFunctions.ContentHash(b);

            Assert.Equal(hashA, hashB);
        }

        [Theory]
        [MemberData(nameof(Equal1dArrayExamples), true)]
        public void ContentEqual_ReturnsTrue_ForEqual1dArrays(double[] a, double[] b)
        {
            var areEqual = ArrayFunctions.ContentEqual(a, b);
            Assert.True(areEqual);
        }

        [Theory]
        [MemberData(nameof(Unequal1dArrayExamples))]
        public void ContentEqual_ReturnsFalse_ForUnequal1dArrays(double[] a, double[] b)
        {
            var areEqual = ArrayFunctions.ContentEqual(a, b);
            Assert.False(areEqual);
        }

        [Theory]
        [MemberData(nameof(Equal2dArrayExamples), true)]
        public void ContentEqual_ReturnsTrue_ForEqual2dArrays(double[,] a, double[,] b)
        {
            var areEqual = ArrayFunctions.ContentEqual(a, b);
            Assert.True(areEqual);
        }

        [Theory]
        [MemberData(nameof(Unequal2dArrayExamples))]
        public void ContentEqual_ReturnsFalse_ForUnequal2dArrays(double[,] a, double[,] b)
        {
            var areEqual = ArrayFunctions.ContentEqual(a, b);
            Assert.False(areEqual);
        }

        public static IEnumerable<object[]> Equal1dArrayExamples(bool includeNull)
        {
            if (includeNull)
            {
                // both arrays null
                yield return new[] { (double[])null, (double[])null };
            }

            // equal length and content
            yield return new[] { new[] { 0.001d }, new[] { 0.001d } };
            yield return new[] { new[] { 1d, 2d, 3d }, new[] { 1d, 2d, 3d } };
        }
        
        public static IEnumerable<object[]> Unequal1dArrayExamples()
        {
            // either array is null
            yield return new[] { (double[])null, new[] { 1d } };
            yield return new[] { new[] { 1d }, (double[])null };

            // length mismatch
            yield return new[] { new[] { 1d, 2d, 3d }, new[] { 1d, 2d } };

            // content mismatch
            yield return new[] { new[] { 1d, 0d, 3d }, new[] { 1d, 2d, 3d } };
            yield return new[] { new[] { 1d, 2d, 3d }, new[] { 1d, 2d, 0d } };
        }

        public static IEnumerable<object[]> Equal2dArrayExamples(bool includeNull)
        {
            if (includeNull)
            {
                // both arrays null
                yield return new[] { (double[,])null, (double[,])null };
            }

            // equal length and content
            yield return new[]
            {
                new[,] { { 0.001d } },
                new[,] { { 0.001d } }
            };
            yield return new[]
            {
                new[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } },
                new[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } }
            };
        }

        public static IEnumerable<object[]> Unequal2dArrayExamples()
        {
            // either array is null
            yield return new[] { (double[,])null, new[,] { { 1d } } };
            yield return new[] { new[,] { { 1d } }, (double[,])null };

            // length mismatch
            yield return new[] // 2x3 vs 2x2
            {
                new[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } },
                new[,] { { 1d, 2d }, { 3d, 4d } }
            };
            yield return new[] // 2x3 vs 3x2 - same total elements
            {
                new[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } },
                new[,] { { 1d, 2d }, { 3d, 4d }, { 5d, 6d } }
            };

            // content mismatch
            yield return new[]
            {
                new[,] { { 1d, 2d, 3d }, { 4d, 5d, 6d } },
                new[,] { { 1d, 2d, 3d }, { 4d, 0d, 6d } }
            };
        }
    }
}
