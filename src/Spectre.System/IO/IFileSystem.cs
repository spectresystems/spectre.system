// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.System.IO
{
    /// <summary>
    /// Represents a file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets the <see cref="IFileProvider"/> belonging to this <see cref="IFileSystem"/>>.
        /// </summary>
        /// <returns>An <see cref="IFileProvider"/> instance.</returns>
        IFileProvider File { get; }

        /// <summary>
        /// Gets the <see cref="IDirectoryProvider"/> belonging to this <see cref="IFileSystem"/>>.
        /// </summary>
        /// <returns>An <see cref="IDirectoryProvider"/> instance.</returns>
        IDirectoryProvider Directory { get; }

        /// <summary>
        /// Gets a <see cref="IFile"/> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile GetFile(FilePath path);

        /// <summary>
        /// Gets a <see cref="IDirectory"/> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory"/> instance representing the specified path.</returns>
        IDirectory GetDirectory(DirectoryPath path);
    }
}