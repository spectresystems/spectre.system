using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Spectre.System.IO;

// ReSharper disable once CheckNamespace
namespace Spectre.System.Tests
{
    public static class PathAssertions
    {
        private static readonly PathComparer Comparer = new PathComparer(false);
        
        public static void ShouldContainFilePath(this IEnumerable<Path> result, string path)
        {
            ContainsPath(result, new FilePath(path));
        }

        public static void ShouldContainDirectoryPath(this IEnumerable<Path> result, string path)
        {
            ContainsPath(result, new DirectoryPath(path));
        }
        
        public static void ContainsPath<T>(IEnumerable<Path> paths, T expected)
            where T : Path
        {
            // Find the path.
            var path = paths.FirstOrDefault(x => Comparer.Equals(x, expected));

            // Assert
            path.ShouldNotBeNull();
            path.ShouldBeOfType<T>();
        }
    }
}