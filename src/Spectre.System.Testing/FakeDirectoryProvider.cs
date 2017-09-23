// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Spectre.System.IO;

namespace Spectre.System.Testing
{
    internal sealed class FakeDirectoryProvider : IDirectoryProvider
    {
        private readonly FileSystemTree _tree;

        public FakeDirectoryProvider(FileSystemTree tree)
        {
            _tree = tree;
        }

        public FakeDirectory Get(DirectoryPath path)
        {
            return _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
        }

        IDirectory IDirectoryProvider.Get(DirectoryPath path)
        {
            return Get(path);
        }

        public bool Exists(DirectoryPath path)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            return directory.Exists;
        }

        public bool IsHidden(DirectoryPath path)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            return directory.Hidden;
        }

        public void Create(DirectoryPath path)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            directory.Create();
        }

        public void Move(DirectoryPath source, DirectoryPath destination)
        {
            var directory = _tree.FindDirectory(source) ?? new FakeDirectory(_tree, source);
            directory.Move(destination);
        }

        public void Delete(DirectoryPath path, bool recursive)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            directory.Delete(recursive);
        }

        public IEnumerable<IDirectory> GetDirectories(DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            return directory.GetDirectories(filter, scope);
        }

        public IEnumerable<IFile> GetFiles(DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = _tree.FindDirectory(path) ?? new FakeDirectory(_tree, path);
            return directory.GetFiles(filter, scope);
        }
    }
}