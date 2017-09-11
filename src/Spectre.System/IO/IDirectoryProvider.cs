using System.Collections.Generic;

namespace Spectre.System.IO
{
    public interface IDirectoryProvider
    {
        /// <summary>
        /// Gets a <see cref="IDirectory"/> instance.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <returns>An <see cref="IDirectory"/> instance.</returns>
        IDirectory Get(DirectoryPath path);

        /// <summary>
        /// Gets whether or not the specified directory exists.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
        bool Exists(DirectoryPath path);

        /// <summary>
        /// Gets whether or not the specified directory is hidden.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <returns><c>true</c> if the directory is hidden; otherwise, <c>false</c>.</returns>
        bool IsHidden(DirectoryPath path);

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="path">The directory to be created.</param>
        void Create(DirectoryPath path);

        /// <summary>
        /// Moves the source directory to the specified destination.
        /// </summary>
        /// <param name="source">The source path.</param>
        /// <param name="destination">The destination path.</param>
        void Move(DirectoryPath source, DirectoryPath destination);

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="path">The directory to be deleted.</param>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        void Delete(DirectoryPath path, bool recursive);

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="path">The root directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(DirectoryPath path, string filter, SearchScope scope);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="path">The root directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(DirectoryPath path, string filter, SearchScope scope);
    }
}