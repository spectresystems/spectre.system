// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Spectre.System.IO;

namespace Spectre.System
{
    /// <summary>
    /// Contains extensions for <see cref="IDirectoryProvider"/>.
    /// </summary>
    public static class DirectoryProviderExtensions
    {
        /// <summary>
        /// Gets whether or not the specified directory exists.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The directory path.</param>
        /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
        public static bool Exists(this IDirectoryProvider provider, DirectoryPath path)
        {
            var directory = provider.Get(path);
            return directory.Exists;
        }

        /// <summary>
        /// Gets whether or not the specified directory is hidden.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The directory path.</param>
        /// <returns><c>true</c> if the directory is hidden; otherwise, <c>false</c>.</returns>
        public static bool IsHidden(this IDirectoryProvider provider, DirectoryPath path)
        {
            var directory = provider.Get(path);
            return directory.Hidden;
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The directory to be created.</param>
        public static void Create(this IDirectoryProvider provider, DirectoryPath path)
        {
            var directory = provider.Get(path);
            directory.Create();
        }

        /// <summary>
        /// Moves the source directory to the specified destination.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="source">The source path.</param>
        /// <param name="destination">The destination path.</param>
        public static void Move(this IDirectoryProvider provider, DirectoryPath source, DirectoryPath destination)
        {
            var directory = provider.Get(source);
            directory.Move(destination);
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The directory to be deleted.</param>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        public static void Delete(this IDirectoryProvider provider, DirectoryPath path, bool recursive)
        {
            var directory = provider.Get(path);
            directory.Delete(recursive);
        }

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The root directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        public static IEnumerable<IDirectory> GetDirectories(this IDirectoryProvider provider, DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = provider.Get(path);
            return directory.GetDirectories(filter, scope);
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="provider">The directory provider.</param>
        /// <param name="path">The root directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        public static IEnumerable<IFile> GetFiles(this IDirectoryProvider provider, DirectoryPath path, string filter, SearchScope scope)
        {
            var directory = provider.Get(path);
            return directory.GetFiles(filter, scope);
        }
    }
}