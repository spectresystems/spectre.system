// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Spectre.System.IO
{
    internal sealed class DirectoryProvider : IDirectoryProvider
    {
        public IDirectory Get(DirectoryPath path)
        {
            return new Directory(path);
        }
    }
}