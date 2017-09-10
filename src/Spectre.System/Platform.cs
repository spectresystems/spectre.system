// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

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
            Family = GetPlatformFamily();
            Is64Bit = Is64BitOperativeSystem();
        }

        public static bool IsUnix()
        {
            return IsUnix(GetPlatformFamily());
        }

        public static bool IsUnix(PlatformFamily family)
        {
            return family == PlatformFamily.Linux
                   || family == PlatformFamily.OSX;
        }

        public static bool Is64BitOperativeSystem()
        {
            return RuntimeInformation.OSArchitecture == Architecture.X64
                   || RuntimeInformation.OSArchitecture == Architecture.Arm64;
        }

        public static PlatformFamily GetPlatformFamily()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return PlatformFamily.OSX;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return PlatformFamily.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return PlatformFamily.Windows;
            }
            return PlatformFamily.Unknown;
        }
    }
}