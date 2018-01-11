// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spectre.System.IO
{
    internal sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;

        public DirectoryPath Path { get; }

        Path IFileSystemInfo.Path => Path;

        public bool Exists => _directory.Exists;

        public bool Hidden => (_directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        public Directory(DirectoryPath path)
        {
            Path = path;
            _directory = new DirectoryInfo(Path.FullPath);
        }

        public void Create()
        {
            _directory.Create();
            Refresh();
        }

        public void Move(DirectoryPath destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            _directory.MoveTo(destination.FullPath);
            Refresh();
        }

        public void Delete(bool recursive)
        {
            _directory.Delete(recursive);
            Refresh();
        }

        public void Refresh()
        {
            _directory.Refresh();
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetDirectories(filter, option)
                .Select(directory => new Directory(new DirectoryPath(directory.FullName)));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            IEnumerable<IFile> files = _directory.GetFiles(filter, SearchOption.TopDirectoryOnly)
                .Select(file => new File(new FilePath(file.FullName)));

            if (option == SearchOption.TopDirectoryOnly)
            {
                return files;
            }

            var directories = _directory.GetDirectories().Where(d => d.Attributes.HasFlag(FileAttributes.ReparsePoint) == false)
                .Select(dir => new Directory(new DirectoryPath(dir.FullName)));

            foreach (var directory in directories)
            {
                files = files.Concat(directory.GetFiles(filter, scope));
            }

            return files;
        }
    }
}