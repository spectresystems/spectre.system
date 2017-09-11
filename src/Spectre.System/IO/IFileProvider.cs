using System.IO;

namespace Spectre.System.IO
{
    public interface IFileProvider
    {
        /// <summary>
        /// Gets a <see cref="IFile"/> instance.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>An <see cref="IFile"/> instance.</returns>
        IFile Get(FilePath path);

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <value>The length of the file.</value>
        long GetLength(FilePath path);

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <value>The file attributes.</value>
        FileAttributes GetAttributes(FilePath path);

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="attributes">The file attributes.</param>
        /// <value>The file attributes.</value>
        void SetAttributes(FilePath path, FileAttributes attributes);

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        void Copy(FilePath source, FilePath destination, bool overwrite);

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="source">The source file path.</param>
        /// <param name="destination">The destination file path.</param>
        void Move(FilePath source, FilePath destination);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The file to delete.</param>
        void Delete(FilePath path);

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        Stream Open(FilePath path, FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }
}