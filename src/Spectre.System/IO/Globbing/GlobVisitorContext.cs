﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Spectre.System.IO.Globbing
{
    internal sealed class GlobVisitorContext
    {
        private readonly GlobberSettings _settings;
        private readonly LinkedList<string> _pathParts;

        public DirectoryPath Root { get; set; }
        public List<IFileSystemInfo> Results { get; }

        internal DirectoryPath Path { get; private set; }

        public GlobVisitorContext(
            IEnvironment environment,
            GlobberSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _pathParts = new LinkedList<string>();

            Results = new List<IFileSystemInfo>();

            Root = _settings.Root ?? environment.WorkingDirectory;
            Root = Root.MakeAbsolute(environment);
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
            return _settings.Predicate == null ||
                   _settings.Predicate(info);
        }
    }
}