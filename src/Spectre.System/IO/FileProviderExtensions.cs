// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Spectre.System.IO;

namespace Spectre.System
{
    /// <summary>
    /// Contains extensions for <see cref="IFileProvider"/>.
    /// </summary>
    public static class FileProviderExtensions
    {
        /// <summary>
        /// Gets whether or not the specified file exists.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
        public static bool Exists(this IFileProvider provider, FilePath path)
        {
            var file = provider.Get(path);
            return file.Exists;
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <value>The length of the file.</value>
        /// <returns>The file size in bytes.</returns>
        public static long GetLength(this IFileProvider provider, FilePath path)
        {
            var file = provider.Get(path);
            return file.Length;
        }

        /// <summary>
        /// Gets the last write time of the file.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <value>The last write time of the file.</value>
        /// <returns>The last write time of the file.</returns>
        public static DateTime GetLastWriteTime(this IFileProvider provider, FilePath path)
        {
            var file = provider.Get(path);
            return file.LastWriteTime;
        }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <value>The file attributes.</value>
        /// <returns>The file attributes.</returns>
        public static FileAttributes GetAttributes(this IFileProvider provider, FilePath path)
        {
            var file = provider.Get(path);
            return file.Attributes;
        }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <param name="attributes">The file attributes.</param>
        /// <value>The file attributes.</value>
        public static void SetAttributes(this IFileProvider provider, FilePath path, FileAttributes attributes)
        {
            var file = provider.Get(path);
            file.Attributes = attributes;
        }

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        public static void Copy(this IFileProvider provider, FilePath source, FilePath destination, bool overwrite)
        {
            var file = provider.Get(source);
            file.Copy(destination, overwrite);
        }

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        public static void Move(this IFileProvider provider, FilePath source, FilePath destination)
        {
            var file = provider.Get(source);
            file.Move(destination);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file to delete.</param>
        public static void Delete(this IFileProvider provider, FilePath path)
        {
            var file = provider.Get(path);
            file.Delete();
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path.</param>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream Open(this IFileProvider provider, FilePath path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            var file = provider.Get(path);
            return file.Open(fileMode, fileAccess, fileShare);
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path to be opened.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream Open(this IFileProvider provider, FilePath path, FileMode mode)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            return provider.Get(path).Open(mode,
                mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite,
                FileShare.None);
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path to be opened.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream Open(this IFileProvider provider, FilePath path, FileMode mode, FileAccess access)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            return provider.Get(path).Open(mode, access, FileShare.None);
        }

        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path to be opened.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream OpenRead(this IFileProvider provider, FilePath path)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            return provider.Get(path).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens the file for writing.
        /// If the file already exists, it will be overwritten.
        /// </summary>
        /// <param name="provider">The file provider.</param>
        /// <param name="path">The file path to be opened.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream OpenWrite(this IFileProvider provider, FilePath path)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            return provider.Get(path).Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }
    }
}