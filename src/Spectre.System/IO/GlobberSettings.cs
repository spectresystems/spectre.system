using System;

namespace Spectre.System.IO
{
    public sealed class GlobberSettings
    {
        public Func<IDirectory, bool> Predicate { get; set; }
        public PathComparer Comparer { get; set; }
    }
}