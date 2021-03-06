﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using NSubstitute;
using Shouldly;
using Spectre.System.IO;
using Xunit;

namespace Spectre.System.Tests.Unit.IO
{
    public sealed class FilePathTests
    {
        public sealed class TheHasExtensionProperty
        {
            [Theory]
            [InlineData("assets/shaders/basic.txt", true)]
            [InlineData("assets/shaders/basic", false)]
            [InlineData("assets/shaders/basic/", false)]
            public void Can_See_If_A_Path_Has_An_Extension(string fullPath, bool expected)
            {
                // Given, When
                var path = new FilePath(fullPath);

                // Then
                path.HasExtension.ShouldBe(expected);
            }
        }

        public sealed class TheGetExtensionMethod
        {
            [Theory]
            [InlineData("foo/bar.baz", "baz")]
            [InlineData("foo/bar.baz/qux.flob", "flob")]
            [InlineData("foo/bar", "")]
            [InlineData("foo/bar.baz/qux", "")]
            public void Can_Get_Extension(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetExtension();

                // Then
                result.Name.ShouldBe(expected);
            }
        }

        public sealed class TheGetDirectoryMethod
        {
            [Fact]
            public void Can_Get_Directory_For_File_Path()
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = path.GetDirectory();

                // Then
                result.FullPath.ShouldBe("temp");
            }

            [Fact]
            public void Can_Get_Directory_For_File_Path_In_Root()
            {
                // Given
                var path = new FilePath("hello.txt");

                // When
                var result = path.GetDirectory();

                // Then
                result.FullPath.ShouldBe(string.Empty);
            }
        }

        public sealed class TheChangeExtensionMethod
        {
            [Fact]
            public void Can_Change_Extension_Of_Path()
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = path.ChangeExtension(new FileExtension(".dat"));

                // Then
                result.FullPath.ShouldBe("temp/hello.dat");
            }
        }

        public sealed class TheAppendExtensionMethod
        {
            [Fact]
            public void Should_Return_Same_Path_If_Providing_Null_Extension()
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = path.AppendExtension(null);

                // Then
                result.FullPath.ShouldBe("temp/hello.txt");
            }

            [Theory]
            [InlineData("")]
            [InlineData(".")]
            public void Should_Return_Same_Path_If_Providing_Empty_Extension(string extension)
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = path.AppendExtension(new FileExtension(extension));

                // Then
                result.FullPath.ShouldBe("temp/hello.txt");
            }

            [Theory]
            [InlineData("dat", "temp/hello.txt.dat")]
            [InlineData(".dat", "temp/hello.txt.dat")]
            public void Should_Append_Extension_To_Path(string input, string expected)
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = path.AppendExtension(new FileExtension(input));

                // Then
                result.FullPath.ShouldBe(expected);
            }
        }

        public sealed class TheRemoveExtensionMethod
        {
            [Theory]
            [InlineData("temp/hello", "temp/hello")]
            [InlineData("temp/hello.txt", "temp/hello")]
            [InlineData("temp/hello.txt.dat", "temp/hello.txt")]
            public void Can_Append_Extension_To_Path(string input, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                var result = path.RemoveExtension();

                // Then
                result.FullPath.ShouldBe(expected);
            }
        }

        public sealed class TheGetFilenameMethod
        {
            [Fact]
            public void Can_Get_Filename_From_Path()
            {
                // Given
                var path = new FilePath("/input/test.txt");

                // When
                var result = path.GetFilename();

                // Then
                result.FullPath.ShouldBe("test.txt");
            }
        }

        public sealed class TheGetFilenameWithoutExtensionMethod
        {
            [Theory]
            [InlineData("/input/test.txt", "test")]
            [InlineData("/input/test", "test")]
            public void Should_Return_Filename_Without_Extension_From_Path(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetFilenameWithoutExtension();

                // Then
                result.FullPath.ShouldBe(expected);
            }
        }

        public sealed class TheMakeAbsoluteMethod
        {
            public sealed class WithEnvironment
            {
                [Fact]
                public void Should_Throw_If_Environment_Is_Null()
                {
                    // Given
                    var path = new FilePath("temp/hello.txt");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute((IEnvironment)null));

                    // Then
                    result.ShouldBeArgumentNullException("environment");
                }

                [Fact]
                public void Should_Return_A_Absolute_File_Path_If_File_Path_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var environment = Substitute.For<IEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    result.FullPath.ShouldBe("/absolute/test.txt");
                }

                [Fact]
                public void Should_Return_Same_File_Path_If_File_Path_Is_Absolute()
                {
                    // Given
                    var path = new FilePath("/test.txt");
                    var environment = Substitute.For<IEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    result.FullPath.ShouldBe("/test.txt");
                }
            }

            public sealed class WithDirectoryPath
            {
                [Fact]
                public void Should_Throw_If_Provided_Directory_Is_Null()
                {
                    // Given
                    var path = new FilePath("./test.txt");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute((DirectoryPath)null));

                    // Then
                    result.ShouldBeArgumentNullException("path");
                }

                [Fact]
                public void Should_Throw_If_Provided_Directory_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var directory = new DirectoryPath("./relative");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute(directory));

                    // Then
                    result.ShouldBeOfType<InvalidOperationException>()
                          .And(ex => ex.Message.ShouldBe(
                                   "Cannot make a file path absolute with a relative directory path."));
                }

                [Fact]
                public void Should_Return_A_Absolute_File_Path_If_File_Path_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var directory = new DirectoryPath("/absolute");

                    // When
                    var result = path.MakeAbsolute(directory);

                    // Then
                    result.FullPath.ShouldBe("/absolute/test.txt");
                }

                [Fact]
                public void Should_Return_Same_File_Path_If_File_Path_Is_Absolute()
                {
                    // Given
                    var path = new FilePath("/test.txt");
                    var directory = new DirectoryPath("/absolute");

                    // When
                    var result = path.MakeAbsolute(directory);

                    // Then
                    result.FullPath.ShouldBe("/test.txt");
                }
            }
        }

        public sealed class TheGetRelativePathMethod
        {
            public sealed class WithDirectoryPath
            {
                public sealed class InWindowsFormat
                {
                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C", ".")]
                    [InlineData("C:/hello.txt", "C:/", ".")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/D/E", "../../D/E")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/", "../../..")]
                    [InlineData("C:/A/B/C/D/E/F/hello.txt", "C:/A/B/C", "../../..")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/D/E/F", "D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        result.FullPath.ShouldBe(expected);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "D:/A/B/C")]
                    [InlineData("C:/A/B/hello.txt", "D:/E/")]
                    [InlineData("C:/hello.txt", "B:/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath(to)));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Paths must share a common prefix."));
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Null()
                    {
                        // Given
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        result.ShouldBeArgumentNullException("to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("C:/D/E/F")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Source path must be an absolute path."));
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("D")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Target path must be an absolute path."));
                    }
                }

                public sealed class InUnixFormat
                {
                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C", ".")]
                    [InlineData("/C/hello.txt", "/C/", ".")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/D/E", "../../D/E")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/", "../../..")]
                    [InlineData("/C/A/B/C/D/E/F/hello.txt", "/C/A/B/C", "../../..")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/D/E/F", "D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        result.FullPath.ShouldBe(expected);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/D/A/B/C")]
                    [InlineData("/C/A/B/hello.txt", "/D/E/")]
                    [InlineData("/C/hello.txt", "/B/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath(to)));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Paths must share a common prefix."));
                    }

                    [Fact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Null()
                    {
                        // Given
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        result.ShouldBeArgumentNullException("to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("/C/D/E/F")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Source path must be an absolute path."));
                    }

                    [Fact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("D")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Target path must be an absolute path."));
                    }
                }
            }

            public sealed class WithFilePath
            {
                public sealed class InWindowsFormat
                {
                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("C:/hello.txt", "C:/hello.txt", "hello.txt")]
                    [InlineData("C:/hello.txt", "C:/world.txt", "world.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C/D/E/F/hello.txt", "C:/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = path.GetRelativePath(new FilePath(to));

                        // Then
                        result.FullPath.ShouldBe(expected);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "D:/A/B/C/hello.txt")]
                    [InlineData("C:/A/B/hello.txt", "D:/E/hello.txt")]
                    [InlineData("C:/hello.txt", "B:/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath(to)));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Paths must share a common prefix."));
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_FilePath_Is_Null()
                    {
                        // Given
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        result.ShouldBeArgumentNullException("to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("C:/D/E/F/hello.txt")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Source path must be an absolute path."));
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_FilePath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("D/hello.txt")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Target path must be an absolute path."));
                    }
                }

                public sealed class InUnixFormat
                {
                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("/C/hello.txt", "/C/hello.txt", "hello.txt")]
                    [InlineData("/C/hello.txt", "/C/world.txt", "world.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C/D/E/F/hello.txt", "/C/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = path.GetRelativePath(new FilePath(to));

                        // Then
                        result.FullPath.ShouldBe(expected);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/D/A/B/C/hello.txt")]
                    [InlineData("/C/A/B/hello.txt", "/D/E/hello.txt")]
                    [InlineData("/C/hello.txt", "/B/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath(to)));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Paths must share a common prefix."));
                    }

                    [Fact]
                    public void Should_Throw_If_Target_FilePath_Is_Null()
                    {
                        // Given
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        result.ShouldBeArgumentNullException("to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("/C/D/E/F/hello.txt")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Source path must be an absolute path."));
                    }

                    [Fact]
                    public void Should_Throw_If_Target_FilePath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("D/hello.txt")));

                        // Then
                        result.ShouldBeOfType<InvalidOperationException>()
                              .And(ex => ex.Message.ShouldBe("Target path must be an absolute path."));
                    }
                }
            }
        }
    }
}