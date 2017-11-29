// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NET461
using System.Runtime.InteropServices;
#else
using System;
#endif

namespace Spectre.System.Polyfill
{
    internal static class EnvironmentHelper
    {
#if NET461
        private static bool? _isRunningOnMac;
#endif

        public static bool Is64BitOperativeSystem()
        {
#if !NET461
            return RuntimeInformation.OSArchitecture == Architecture.X64
                   || RuntimeInformation.OSArchitecture == Architecture.Arm64;
#else
            return global::System.Environment.Is64BitOperatingSystem;
#endif
        }

        public static PlatformFamily GetPlatformFamily()
        {
#if !NET461
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
#else
            var platform = (int)global::System.Environment.OSVersion.Platform;
            if (platform <= 3 || platform == 5)
            {
                return PlatformFamily.Windows;
            }
            if (!_isRunningOnMac.HasValue)
            {
                _isRunningOnMac = Native.MacOSX.IsRunningOnMac();
            }
            if (_isRunningOnMac ?? false || platform == (int)PlatformID.MacOSX)
            {
                return PlatformFamily.OSX;
            }
            if (platform == 4 || platform == 6 || platform == 128)
            {
                return PlatformFamily.Linux;
            }
#endif
            return PlatformFamily.Unknown;
        }
    }
}
