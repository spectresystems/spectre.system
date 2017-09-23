// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Shouldly;
using Spectre.System.IO;
using Spectre.System.Testing;
using Xunit;

namespace Spectre.System.Tests.Unit.Testing
{
    public sealed class FakeFileSystemTests
    {
        [Fact]
        public void Should_Be_Able_To_Create_Directory()
        {
            // Given
            var environment = new FakeEnvironment(PlatformFamily.Linux);
            var fileSystem = new FakeFileSystem(environment);

            // When
            fileSystem.CreateDirectory(new DirectoryPath("/working"));

            // Then
            fileSystem
                .GetDirectory(new DirectoryPath("/working"))
                .Exists.ShouldBeTrue();
        }

        [Fact]
        public void Should_Be_Able_To_Create_Directory_Using_Directory_Provider()
        {
            // Given
            var environment = new FakeEnvironment(PlatformFamily.Linux);
            var fileSystem = new FakeFileSystem(environment);

            // When
            fileSystem.Directory.Create(new DirectoryPath("/working"));

            // Then
            fileSystem.Directory
                .Exists(new DirectoryPath("/working"))
                .ShouldBeTrue();
        }

        [Fact]
        public void Should_Be_Able_To_Create_File()
        {
            // Given
            var environment = new FakeEnvironment(PlatformFamily.Linux);
            var fileSystem = new FakeFileSystem(environment);

            // When
            fileSystem.CreateFile(new FilePath("/working/foo.bar"));

            // Then
            fileSystem
                .GetFile(new FilePath("/working/foo.bar"))
                .Exists.ShouldBeTrue();
        }

        [Fact]
        public void Should_Create_Directory_When_Creating_File_If_Missing()
        {
            // Given
            var environment = new FakeEnvironment(PlatformFamily.Linux);
            var fileSystem = new FakeFileSystem(environment);

            // When
            fileSystem.CreateFile(new FilePath("/working/foo.bar"));

            // Then
            fileSystem
                .GetDirectory(new DirectoryPath("/working"))
                .Exists.ShouldBeTrue();
        }
    }
}