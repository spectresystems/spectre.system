// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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