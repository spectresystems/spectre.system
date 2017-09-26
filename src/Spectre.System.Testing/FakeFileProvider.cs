// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
            return Get(path).Length;
        }

        public DateTime GetLastWriteTime(FilePath path)
        {
            return Get(path).LastWriteTime;
        }

        public FileAttributes GetAttributes(FilePath path)
        {
            return Get(path).Attributes;
        }

        public void SetAttributes(FilePath path, FileAttributes attributes)
        {
            Get(path).Attributes = attributes;
        }

        public void Copy(FilePath source, FilePath destination, bool overwrite)
        {
            Get(source).Copy(destination, overwrite);
        }

        public void Move(FilePath source, FilePath destination)
        {
            Get(source).Move(destination);
        }

        public void Delete(FilePath path)
        {
            Get(path).Delete();
        }

        public Stream Open(FilePath path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return Get(path).Open(fileMode, fileAccess, fileShare);
        }
    }
}