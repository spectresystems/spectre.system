using System;

namespace Spectre.System
{
    public static class PlatformExtensions
    {
        public static bool IsUnix(this IPlatform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            return Platform.IsUnix(platform.Family);
        }
    }
}