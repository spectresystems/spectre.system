using System;
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
            var environment = new FakeEnvironment(PlatformFamily.Linux, true);
            var fileSystem = new FakeFileSystem(environment);
            
            // When
            fileSystem.CreateDirectory(new DirectoryPath("/working"));
            
            // Then
            fileSystem
                .GetDirectory(new DirectoryPath("/working"))
                .Exists.ShouldBeTrue();
        }

        [Fact]
        public void Should_Be_Able_To_Create_File_With_Content()
        {
            // Given
            var environment = new FakeEnvironment(PlatformFamily.Linux, true);
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
            var environment = new FakeEnvironment(PlatformFamily.Linux, true);
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