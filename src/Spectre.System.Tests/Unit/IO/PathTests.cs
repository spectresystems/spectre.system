// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Shouldly;
using Spectre.System.IO;
using Xunit;

namespace Spectre.System.Tests.Unit.IO
{
    public sealed class PathTests
    {
        #region Private Test Classes

        private sealed class TestingPath : Path
        {
            public TestingPath(string path)
                : base(path)
            {
            }
        }

        #endregion

        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TestingPath(null));

                // Then
                result.ShouldBeArgumentNullException("path");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t ")]
            public void Should_Throw_If_Path_Is_Empty(string fullPath)
            {
                // Given, When
                var result = Record.Exception(() => new TestingPath(fullPath));

                // Then
                result.ShouldBeArgumentException("path", "Path cannot be empty.");
            }

            [Fact]
            public void Current_Directory_Returns_Empty_Path()
            {
                // Given, When
                var path = new TestingPath("./");

                // Then
                path.FullPath.ShouldBe(string.Empty);
            }

            [Fact]
            public void Will_Normalize_Path_Separators()
            {
                // Given, When
                var path = new TestingPath("shaders\\basic");

                // Then
                path.FullPath.ShouldBe("shaders/basic");
            }

            [Fact]
            public void Will_Trim_WhiteSpace_From_Path()
            {
                // Given, When
                var path = new TestingPath(" shaders/basic ");

                // Then
                path.FullPath.ShouldBe("shaders/basic");
            }

            [Fact]
            public void Will_Not_Remove_WhiteSpace_Within_Path()
            {
                // Given, When
                var path = new TestingPath("my awesome shaders/basic");

                // Then
                path.FullPath.ShouldBe("my awesome shaders/basic");
            }

            [Theory]
            [InlineData("/Hello/World/", "/Hello/World")]
            [InlineData("\\Hello\\World\\", "/Hello/World")]
            [InlineData("file.txt/", "file.txt")]
            [InlineData("file.txt\\", "file.txt")]
            [InlineData("Temp/file.txt/", "Temp/file.txt")]
            [InlineData("Temp\\file.txt\\", "Temp/file.txt")]
            public void Should_Remove_Trailing_Slashes(string value, string expected)
            {
                // Given, When
                var path = new TestingPath(value);

                // Then
                path.FullPath.ShouldBe(expected);
            }
        }

        public sealed class TheSegmentsProperty
        {
            [Theory]
            [InlineData("Hello/World")]
            [InlineData("./Hello/World/")]
            public void Should_Return_Segments_Of_Path(string pathName)
            {
                // Given, When
                var result = new TestingPath(pathName);

                // Then
                result.Segments.Length.ShouldBe(2);
                result.Segments[0].ShouldBe("Hello");
                result.Segments[1].ShouldBe("World");
            }

            [Theory]
            [InlineData("/Hello/World")]
            [InlineData("/Hello/World/")]
            public void Should_Return_Segments_Of_Path_And_Leave_Absolute_Directory_Separator_Intact(string pathName)
            {
                // Given, When
                var result = new TestingPath(pathName);

                // Then
                result.Segments.Length.ShouldBe(2);
                result.Segments[0].ShouldBe("/Hello");
                result.Segments[1].ShouldBe("World");
            }
        }

        public sealed class TheFullPathProperty
        {
            [Fact]
            public void Should_Return_Full_Path()
            {
                // Given, When
                var path = new TestingPath("shaders/basic");

                // Then
                path.FullPath.ShouldBe("shaders/basic");
            }
        }

        public sealed class TheIsRelativeProperty
        {
            [Theory]
            [InlineData("assets/shaders", true)]
            [InlineData("assets/shaders/basic.frag", true)]
            [InlineData("/assets/shaders", false)]
            [InlineData("/assets/shaders/basic.frag", false)]
            public void Should_Return_Whether_Or_Not_A_Path_Is_Relative(string fullPath, bool expected)
            {
                // Given, When
                var result = new TestingPath(fullPath);

                // Then
                result.IsRelative.ShouldBe(expected);
            }

            [WindowsTheory]
            [InlineData("c:/assets/shaders", false)]
            [InlineData("c:/assets/shaders/basic.frag", false)]
            [InlineData("c:/", false)]
            [InlineData("c:", false)]
            public void Should_Return_Whether_Or_Not_A_Path_Is_Relative_On_Windows(string fullPath, bool expected)
            {
                // Given, When
                var result = new TestingPath(fullPath);

                // Then
                result.IsRelative.ShouldBe(expected);
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Return_The_Full_Path()
            {
                // Given, When
                var path = new TestingPath("temp/hello");

                // Then
                path.ToString().ShouldBe("temp/hello");
            }
        }
    }
}