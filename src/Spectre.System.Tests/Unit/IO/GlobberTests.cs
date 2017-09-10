// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using NSubstitute;
using Shouldly;
using Spectre.System.IO;
using Spectre.System.Testing;
using Xunit;

namespace Spectre.System.Tests.Unit.IO
{
    public sealed class GlobberTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<IEnvironment>();

                // When
                var result = Record.Exception(() => new Globber(null, environment));

                // Then
                result.ShouldBeArgumentNullException("fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new Globber(fileSystem, null));

                // Then
                result.ShouldBeArgumentNullException("environment");
            }
        }

        public sealed class TheMatchMethod
        {
            public sealed class WindowsSpecific
            {
                [WindowsFact]
                public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("/Working/Foo/Bar/Qux.c");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Working/Foo/Bar/Qux.c");
                }

                [WindowsFact]
                public void Should_Throw_If_Unc_Root_Was_Encountered()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = Record.Exception(() =>
                        fixture.Match("//Foo/Bar/Qux.c"));

                    // Then
                    Assert.IsType<NotSupportedException>(result);
                    Assert.Equal("UNC paths are not supported.", result.Message);
                }

                [WindowsFact]
                public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Working/**/qux.c");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Working/Foo/Bar/Qux.c");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Parenthesis_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Program Files (x86)/Foo.*");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Program Files (x86)/Foo.c");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Ampersand_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Tools & Services/*.dll");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Tools & Services/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Plus_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Tools + Services/*.dll");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Tools + Services/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Percent_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Some %2F Directory/*.dll");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Some %2F Directory/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Exclamation_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Some ! Directory/*.dll");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Some ! Directory/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_AtSign_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Some@Directory/*.dll");

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("C:/Some@Directory/MyTool.dll");
                }
            }

            public sealed class WithPredicate
            {
                [Fact]
                public void Should_Return_Paths_Not_Affected_By_Walker_Hints()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i => i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("./**/Qux.h", predicate);

                    // Then
                    result.Length.ShouldBe(1);
                    result.ShouldContainFilePath("/Working/Foo/Bar/Qux.h");
                }

                [Fact]
                public void Should_Not_Return_Path_If_Walker_Hint_Matches_Part_Of_Pattern()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i => i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("/Working/Bar/Qux.h", predicate);

                    // Then
                    result.Length.ShouldBe(0);
                }

                [Fact]
                public void Should_Not_Return_Path_If_Walker_Hint_Exactly_Match_Pattern()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i => i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("/Working/Bar", predicate);

                    // Then
                    result.Length.ShouldBe(0);
                }
            }

            [Fact]
            public void Should_Throw_If_Pattern_Is_Null()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = Record.Exception(() => fixture.Match(null));

                // Then
                result.ShouldBeArgumentNullException("pattern");
            }

            [Fact]
            public void Should_Return_Empty_Result_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match(string.Empty);

                // Then
                result.Length.ShouldBe(0);
            }

            [Fact]
            public void Can_Traverse_Recursively()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*.c");

                // Then
                result.Length.ShouldBe(5);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qex.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Bar/Qux.c");
            }

            [Fact]
            public void
                Will_Append_Relative_Root_With_Implicit_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("Foo/Bar/Qux.c");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Be_Able_To_Visit_Parent_Using_Double_Dots()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/../Foo/Bar/Qux.c");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Throw_If_Visiting_Parent_That_Is_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = Record.Exception(() => fixture.Match("/Working/Foo/**/../Foo/Bar/Qux.c"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<NotSupportedException>(result);
                Assert.Equal(
                    "Visiting a parent that is a recursive wildcard is not supported.",
                    result.Message);
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar/Qux.c");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainDirectoryPath("/Working/Foo/Bar");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Working/Foo");

                // When
                var result = fixture.Match("./Bar/Qux.c");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Working/Foo");

                // When
                var result = fixture.Match("./Bar");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainDirectoryPath("/Working/Foo/Bar");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*");

                // Then
                result.Length.ShouldBe(15);
                result.ShouldContainDirectoryPath("/Working/Foo");
                result.ShouldContainDirectoryPath("/Working/Foo/Bar");
                result.ShouldContainDirectoryPath("/Working/Foo/Baz");
                result.ShouldContainDirectoryPath("/Working/Foo/Bar/Baz");
                result.ShouldContainDirectoryPath("/Working/Bar");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qex.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.h");
                result.ShouldContainFilePath("/Working/Foo/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Foo.Bar.Test.dll");
                result.ShouldContainFilePath("/Working/Bar.Qux.Test.dll");
                result.ShouldContainFilePath("/Working/Quz.FooTest.dll");
                result.ShouldContainFilePath("/Working/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Bar/Qux.h");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/*/Qux.c");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Baz/Qux.c");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar/Q?x.c");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qex.c");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Ba?/Qux.c");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Baz/Qux.c");
            }

            [Fact]
            public void Should_Return_Files_For_Pattern_Ending_With_Character_Wildcard_And_Dot()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/*.Test.dll");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("/Working/Foo.Bar.Test.dll");
                result.ShouldContainFilePath("/Working/Bar.Qux.Test.dll");
            }

            [WindowsFact]
            public void Should_Return_Files_For_Pattern_Ending_With_Character_Wildcard_And_Dot_On_Windows()
            {
                // Given
                var fixture = new GlobberFixture(true);

                // When
                var result = fixture.Match("C:/Working/*.Test.dll");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("C:/Working/Project.A.Test.dll");
                result.ShouldContainFilePath("C:/Working/Project.B.Test.dll");
            }

            [Fact]
            public void Should_Return_File_For_Recursive_Wildcard_Pattern_Ending_With_Wildcard_Regex()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*.c");

                // Then
                result.Length.ShouldBe(5);
                result.ShouldContainFilePath("/Working/Foo/Bar/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Qex.c");
                result.ShouldContainFilePath("/Working/Foo/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Foo/Bar/Baz/Qux.c");
                result.ShouldContainFilePath("/Working/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Only_Folders_For_Pattern_Ending_With_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**");

                // Then
                result.Length.ShouldBe(6);
                result.ShouldContainDirectoryPath("/Working");
                result.ShouldContainDirectoryPath("/Working/Foo");
                result.ShouldContainDirectoryPath("/Working/Foo/Bar");
                result.ShouldContainDirectoryPath("/Working/Foo/Baz");
                result.ShouldContainDirectoryPath("/Working/Foo/Bar/Baz");
                result.ShouldContainDirectoryPath("/Working/Bar");
            }

            [Fact]
            public void Should_Include_Files_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo/**/Bar.baz");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Foo/Bar.baz");
            }

            [Fact]
            public void Should_Include_Folder_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo/**/Bar");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainDirectoryPath("/Foo/Bar");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Parenthesis_In_Them()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo (Bar)/Baz.*");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Foo (Bar)/Baz.c");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_AtSign_In_Them()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo@Bar/Baz.*");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/Foo@Bar/Baz.c");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Relative_Directory_Not_At_The_Beginning()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/./*.Test.dll");

                // Then
                result.Length.ShouldBe(2);
                result.ShouldContainFilePath("/Working/Foo.Bar.Test.dll");
                result.ShouldContainFilePath("/Working/Bar.Qux.Test.dll");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Unicode_Characters_And_Ending_With_Identifier()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/嵌套/**/文件.延期");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/嵌套/目录/文件.延期");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Unicode_Characters_And_Not_Ending_With_Identifier()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/嵌套/**/文件.*");

                // Then
                result.Length.ShouldBe(1);
                result.ShouldContainFilePath("/嵌套/目录/文件.延期");
            }
        }

        internal sealed class GlobberFixture
        {
            public FakeFileSystem FileSystem { get; set; }
            public IEnvironment Environment { get; set; }

            public GlobberFixture(bool windows = false)
            {
                if (windows)
                {
                    PrepareWindowsFixture();
                }
                else
                {
                    PrepareUnixFixture();
                }
            }

            private void PrepareWindowsFixture()
            {
                Environment = FakeEnvironment.CreateWindowsEnvironment();
                FileSystem = new FakeFileSystem(Environment);

                // Directories
                FileSystem.CreateDirectory("C://Working");
                FileSystem.CreateDirectory("C://Working/Foo");
                FileSystem.CreateDirectory("C://Working/Foo/Bar");
                FileSystem.CreateDirectory("C:");
                FileSystem.CreateDirectory("C:/Program Files (x86)");

                // Files
                FileSystem.CreateFile("C:/Working/Foo/Bar/Qux.c");
                FileSystem.CreateFile("C:/Program Files (x86)/Foo.c");
                FileSystem.CreateFile("C:/Working/Project.A.Test.dll");
                FileSystem.CreateFile("C:/Working/Project.B.Test.dll");
                FileSystem.CreateFile("C:/Working/Project.IntegrationTest.dll");
                FileSystem.CreateFile("C:/Tools & Services/MyTool.dll");
                FileSystem.CreateFile("C:/Tools + Services/MyTool.dll");
                FileSystem.CreateFile("C:/Some %2F Directory/MyTool.dll");
                FileSystem.CreateFile("C:/Some ! Directory/MyTool.dll");
                FileSystem.CreateFile("C:/Some@Directory/MyTool.dll");
            }

            private void PrepareUnixFixture()
            {
                Environment = FakeEnvironment.CreateUnixEnvironment();
                FileSystem = new FakeFileSystem(Environment);

                // Directories
                FileSystem.CreateDirectory("/Working");
                FileSystem.CreateDirectory("/Working/Foo");
                FileSystem.CreateDirectory("/Working/Foo/Bar");
                FileSystem.CreateDirectory("/Working/Bar");
                FileSystem.CreateDirectory("/Foo/Bar");
                FileSystem.CreateDirectory("/Foo (Bar)");
                FileSystem.CreateDirectory("/Foo@Bar/");
                FileSystem.CreateDirectory("/嵌套");
                FileSystem.CreateDirectory("/嵌套/目录");

                // Files
                FileSystem.CreateFile("/Working/Foo/Bar/Qux.c");
                FileSystem.CreateFile("/Working/Foo/Bar/Qex.c");
                FileSystem.CreateFile("/Working/Foo/Bar/Qux.h");
                FileSystem.CreateFile("/Working/Foo/Baz/Qux.c");
                FileSystem.CreateFile("/Working/Foo/Bar/Baz/Qux.c");
                FileSystem.CreateFile("/Working/Bar/Qux.c");
                FileSystem.CreateFile("/Working/Bar/Qux.h");
                FileSystem.CreateFile("/Working/Foo.Bar.Test.dll");
                FileSystem.CreateFile("/Working/Bar.Qux.Test.dll");
                FileSystem.CreateFile("/Working/Quz.FooTest.dll");
                FileSystem.CreateFile("/Foo/Bar.baz");
                FileSystem.CreateFile("/Foo (Bar)/Baz.c");
                FileSystem.CreateFile("/Foo@Bar/Baz.c");
                FileSystem.CreateFile("/嵌套/目录/文件.延期");
            }

            public void SetWorkingDirectory(DirectoryPath path)
            {
                Environment.WorkingDirectory = path;
            }

            public Path[] Match(string pattern)
            {
                return Match(pattern, null);
            }

            public Path[] Match(string pattern,
                Func<IFileSystemInfo, bool> predicate)
            {
                return new Globber(FileSystem, Environment)
                    .Match(pattern, predicate)
                    .ToArray();
            }
        }
    }
}