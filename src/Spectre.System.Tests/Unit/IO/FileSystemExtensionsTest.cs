// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using NSubstitute;
using Shouldly;
using Spectre.System.IO;
using Spectre.System.Testing;
using Xunit;

namespace Spectre.System.Tests.Unit.IO
{
    public sealed class FileSystemExtensionsTest
    {
        public sealed class TheExistMethod
        {
            public sealed class WithFilePath
            {
                [Fact]
                public void Should_Return_False_If_File_Do_Not_Exist()
                {
                    // Given
                    var fileSystem = new FakeFileSystem(new FakeEnvironment(PlatformFamily.Linux));
                    fileSystem.EnsureFileDoesNotExist("file.txt");

                    // When
                    var result = fileSystem.Exist((FilePath)"file.txt");

                    // Then
                    result.ShouldBeFalse();
                }

                [Fact]
                public void Should_Return_True_If_File_Exist()
                {
                    // Given
                    var fileSystem = new FakeFileSystem(new FakeEnvironment(PlatformFamily.Linux));
                    fileSystem.CreateFile("file.txt");

                    // When
                    var result = fileSystem.Exist((FilePath)"file.txt");

                    // Then
                    result.ShouldBeTrue();
                }
            }

            public sealed class WithDirectoryPath
            {
                [Fact]
                public void Should_Return_False_If_Directory_Do_Not_Exist()
                {
                    // Given
                    var fileSystem = new FakeFileSystem(new FakeEnvironment(PlatformFamily.Linux));
                    fileSystem.EnsureDirectoryDoesNotExist("/Target");

                    // When
                    var result = fileSystem.Exist((DirectoryPath)"/Target");

                    // Then
                    result.ShouldBeFalse();
                }

                [Fact]
                public void Should_Return_True_If_Directory_Exist()
                {
                    // Given
                    var fileSystem = new FakeFileSystem(new FakeEnvironment(PlatformFamily.Linux));
                    fileSystem.CreateDirectory("/Target");

                    // When
                    var result = fileSystem.Exist((DirectoryPath)"/Target");

                    // Then
                    result.ShouldBeTrue();
                }
            }
        }
    }
}