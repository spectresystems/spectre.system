// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.System.Polyfill;

namespace Spectre.System
{
    /// <summary>
    /// Represents the platform that the application is running on.
    /// </summary>
    public sealed class Platform : IPlatform
    {
        public PlatformFamily Family { get; }
        public bool Is64Bit { get; }

        public Platform()
        {
            Family = EnvironmentHelper.GetPlatformFamily();
            Is64Bit = EnvironmentHelper.Is64BitOperativeSystem();
        }

        public static bool IsUnix()
        {
            return IsUnix(EnvironmentHelper.GetPlatformFamily());
        }

        public static bool IsUnix(PlatformFamily family)
        {
            return family == PlatformFamily.Linux
                   || family == PlatformFamily.OSX;
        }

        public static bool Is64BitOperativeSystem()
        {
            return EnvironmentHelper.Is64BitOperativeSystem();
        }

        public static PlatformFamily GetPlatformFamily()
        {
            return EnvironmentHelper.GetPlatformFamily();
        }
    }
}