using System.IO;
using Spectre.System.IO;

namespace Spectre.System.Testing
{
    internal sealed class FakeFileProvider : IFileProvider
    {
        private readonly FileSystemTree _tree;

        public FakeFileProvider(FileSystemTree tree)
        {
            _tree = tree;
        }

        public FakeFile Get(FilePath path)
        {
            return _tree.FindFile(path) ?? new FakeFile(_tree, path);
        }

        IFile IFileProvider.Get(FilePath path)
        {
            return Get(path);
        }

        public long GetLength(FilePath path)
        {
            var file = Get(path);
            return file.Length;
        }

        public FileAttributes GetAttributes(FilePath path)
        {
            var file = Get(path);
            return file.Attributes;
        }

        public void SetAttributes(FilePath path, FileAttributes attributes)
        {
            var file = Get(path);
            file.Attributes = attributes;
        }

        public void Copy(FilePath source, FilePath destination, bool overwrite)
        {
            var file = Get(source);
            file.Copy(destination, overwrite);
        }

        public void Move(FilePath source, FilePath destination)
        {
            var file = Get(source);
            file.Move(destination);
        }

        public void Delete(FilePath path)
        {
            var file = Get(path);
            file.Delete();
        }

        public Stream Open(FilePath path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var file = Get(path);
            return file.Open(fileMode, fileAccess, fileShare);
        }
    }
}