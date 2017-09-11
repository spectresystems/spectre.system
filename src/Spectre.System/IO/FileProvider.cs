using System.IO;

namespace Spectre.System.IO
{
    internal sealed class FileProvider : IFileProvider
    {
        public IFile Get(FilePath path)
        {
            return new File(path);
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