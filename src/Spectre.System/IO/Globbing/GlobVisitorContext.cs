// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Spectre.System.IO.Globbing
{
    internal sealed class GlobVisitorContext
    {
        private readonly LinkedList<string> _pathParts;

        internal DirectoryPath Path { get; private set; }

        public IFileSystem FileSystem { get; }
        public IEnvironment Environment { get; }
        public GlobberSettings Settings { get; }
        public List<IFileSystemInfo> Results { get; }

        public GlobVisitorContext(
            IFileSystem fileSystem,
            IEnvironment environment,
            GlobberSettings settings)
        {
            _pathParts = new LinkedList<string>();

            FileSystem = fileSystem;
            Environment = environment;
            Settings = settings;
            Results = new List<IFileSystemInfo>();
        }

        public void AddResult(IFileSystemInfo path)
        {
            Results.Add(path);
        }

        public void Push(string path)
        {
            _pathParts.AddLast(path);
            Path = GenerateFullPath();
        }

        public string Pop()
        {
            var last = _pathParts.Last;
            _pathParts.RemoveLast();
            Path = GenerateFullPath();
            return last.Value;
        }

        private DirectoryPath GenerateFullPath()
        {
            var path = string.Join("/", _pathParts);
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "./";
            }
            return new DirectoryPath(path);
        }

        public bool ShouldTraverse(IDirectory info)
        {
            return Settings.Predicate == null ||
                   Settings.Predicate(info);
        }
    }
}