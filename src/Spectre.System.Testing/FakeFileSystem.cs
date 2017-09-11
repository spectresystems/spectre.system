// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.System.IO;

namespace Spectre.System.Testing
{
    /// <summary>
    /// Represents a fake file system.
    /// </summary>
    public sealed class FakeFileSystem : IFileSystem
    {
        private readonly FakeFileProvider _fileProvider;
        private readonly FakeDirectoryProvider _directoryProvider;

        public IFileProvider File => _fileProvider;
        public IDirectoryProvider Directory => _directoryProvider;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeFileSystem"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public FakeFileSystem(IEnvironment environment)
        {
            var tree = new FileSystemTree(environment);
            
            _fileProvider = new FakeFileProvider(tree);
            _directoryProvider = new FakeDirectoryProvider(tree);
        }

        /// <summary>
        /// Gets a <see cref="FakeFile"/> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="FakeFile"/> instance representing the specified path.</returns>
        public FakeFile GetFile(FilePath path)
        {
            return _fileProvider.Get(path);
        }

        /// <summary>
        /// Gets a <see cref="FakeDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="FakeDirectory" /> instance representing the specified path.</returns>
        public FakeDirectory GetDirectory(DirectoryPath path)
        {
            return _directoryProvider.Get(path);
        }

        /// <summary>
        /// Gets a <see cref="IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory" /> instance representing the specified path.</returns>
        IDirectory IFileSystem.GetDirectory(DirectoryPath path)
        {
            return GetDirectory(path);
        }

        /// <summary>
        /// Gets a <see cref="IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile" /> instance representing the specified path.</returns>
        IFile IFileSystem.GetFile(FilePath path)
        {
            return GetFile(path);
        }
    }
}