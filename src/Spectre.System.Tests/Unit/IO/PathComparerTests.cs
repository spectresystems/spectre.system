// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Shouldly;
using Spectre.System.IO;
using Xunit;

namespace Spectre.System.Tests.Unit.IO
{
    public sealed class PathComparerTests
    {
        public sealed class TheEqualsMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Asset_Instances_Is_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var path = new FilePath("shaders/basic.vert");
                
                // When
                var result = comparer.Equals(path, path);

                // Then
                result.ShouldBeTrue();
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Two_Null_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, null);

                // Then
                result.ShouldBeTrue();
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Paths_Are_Considered_Inequal_If_Any_Is_Null(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);

                // When
                var result = comparer.Equals(null, new FilePath("test.txt"));

                // Then
                result.ShouldBeFalse();
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Are_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");
                
                // When
                var result1 = comparer.Equals(first, second);
                var result2 = comparer.Equals(second, first);

                // Then
                result1.ShouldBeTrue();
                result2.ShouldBeTrue();
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Are_Not_Considered_Equal(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");
                
                // When
                var result1 = comparer.Equals(first, second);
                var result2 = comparer.Equals(second, first); 

                // Then
                result1.ShouldBeFalse();
                result2.ShouldBeFalse();
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Are_Considered_Equal_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");
                
                // When
                var result1 = comparer.Equals(first, second);
                var result2 = comparer.Equals(second, first);

                // Then
                result1.ShouldBe(expected);
                result2.ShouldBe(expected);
            }
        }

        public sealed class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Throw_If_Other_Path_Is_Null()
            {
                // Given
                var comparer = new PathComparer(true);

                // When
                var result = Record.Exception(() => comparer.GetHashCode(null));

                // Then
                result.ShouldBeArgumentNullException("obj");
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Same_Paths_Get_Same_Hash_Code(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.vert");
                
                // When
                var hash1 = comparer.GetHashCode(first);
                var hash2 = comparer.GetHashCode(second);

                // Then
                hash1.ShouldBe(hash2);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Different_Paths_Get_Different_Hash_Codes(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("shaders/basic.frag");

                // When
                var hash1 = comparer.GetHashCode(first);
                var hash2 = comparer.GetHashCode(second);

                // Then
                hash1.ShouldNotBe(hash2);
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Same_Paths_But_Different_Casing_Get_Same_Hash_Code_Depending_On_Case_Sensitivity(bool isCaseSensitive, bool expected)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                var first = new FilePath("shaders/basic.vert");
                var second = new FilePath("SHADERS/BASIC.VERT");

                // When
                var hash1 = comparer.GetHashCode(first);
                var hash2 = comparer.GetHashCode(second);
                var result = hash1 == hash2;

                // Then
                result.ShouldBe(expected);
            }
        }

        public sealed class TheDefaultProperty
        {
            [Fact]
            public void Should_Return_Correct_Comparer_Depending_On_Operative_System()
            {
                // Given
                var expected = Platform.IsUnix();

                // When
                var result = PathComparer.Default;

                // Then
                result.IsCaseSensitive.ShouldBe(expected);
            }
        }

        public sealed class TheIsCaseSensitiveProperty
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Return_Whether_Or_Not_The_Comparer_Is_Case_Sensitive(bool isCaseSensitive)
            {
                // Given
                var comparer = new PathComparer(isCaseSensitive);
                
                // When
                var result = comparer.IsCaseSensitive;

                // Then
                result.ShouldBe(isCaseSensitive);
            }
        }
    }
}