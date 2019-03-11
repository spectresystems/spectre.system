// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
    }
}