// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Spectre.System.Tests
{
    public sealed class WindowsFact : FactAttribute
    {
        private static readonly PlatformFamily Family;

        static WindowsFact()
        {
            Family = Platform.GetPlatformFamily();
        }

        // ReSharper disable once UnusedParameter.Local
        public WindowsFact(string reason = null)
        {
            if (Family != PlatformFamily.Windows)
            {
                Skip = reason ?? "Windows test.";
            }
        }
    }
}