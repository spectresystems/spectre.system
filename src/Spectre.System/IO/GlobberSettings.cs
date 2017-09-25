// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Spectre.System.IO
{
    public sealed class GlobberSettings
    {
        public DirectoryPath Root { get; set; }
        public Func<IDirectory, bool> Predicate { get; set; }
        public PathComparer Comparer { get; set; }
    }
}