using System.Collections.Generic;

namespace Spectre.System.IO
{
    internal sealed class DirectoryProvider : IDirectoryProvider
    {
        public IDirectory Get(DirectoryPath path)
        {
            return new Directory(path);
        }

        public bool Exists(DirectoryPath path)
        {
            var directory = new Directory(path);
            return directory.Exists;
        }

        public bool IsHidden(DirectoryPath path)
        {
            var directory = new Directory(path);
            return directory.Hidden;
        }

        public void Create(DirectoryPath path)
        {
            var directory = new Directory(path);
            directory.Create();
        }

        public void Move(DirectoryPath source, DirectoryPath destination)
        {
            var directory = new Directory(source);
            directory.Move(destination);
        }

        public void Delete(DirectoryPath path, bool recursive)
        {
            var directory = new Directory(path);
            directory.Delete(recursive);
        }

        public IEnumerable<IDirectory> GetDirectories(DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = new Directory(path);
            return directory.GetDirectories(filter, scope);
        }

        public IEnumerable<IFile> GetFiles(DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = new Directory(path);
            return directory.GetFiles(filter, scope);
        }
    }
}