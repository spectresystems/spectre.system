// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    }
}