// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Spectre.System.IO;

namespace Spectre.System
{
    /// <summary>
    /// Contains extensions for <see cref="IFileSystem"/>.
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Determines if a specified <see cref="FilePath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified file exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, FilePath path)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            return fileSystem.File.Exists(path);
        }

        /// <summary>
        /// Determines if a specified <see cref="DirectoryPath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified directory exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, DirectoryPath path)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            return fileSystem.Directory.Exists(path);
        }

        /// <summary>
        /// Gets a <see cref="IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile" /> instance representing the specified path.</returns>
        public static IFile GetFile(this IFileSystem fileSystem, FilePath path)
        {
            return fileSystem.File.Get(path);
        }

        /// <summary>
        /// Gets a <see cref="IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory" /> instance representing the specified path.</returns>
        public static IDirectory GetDirectory(this IFileSystem fileSystem, DirectoryPath path)
        {
            return fileSystem.Directory.Get(path);
        }
    }
}