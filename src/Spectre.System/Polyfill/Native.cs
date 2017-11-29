// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/////////////////////////////////////////////////////////////////////////////////////////////////////
// This code was taken and adapted from the MonoDevelop project.
// https://github.com/mono/monodevelop/blob/master/main/src/core/Mono.Texteditor/Mono.TextEditor/Platform.cs
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
/////////////////////////////////////////////////////////////////////////////////////////////////////

#if NET461
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Spectre.System
{
    internal static partial class Native
    {
        public static class MacOSX
        {
            [DllImport("libc")]
#pragma warning disable IDE1006 // Naming Styles
            internal static extern int uname(IntPtr buf);
#pragma warning restore IDE1006 // Naming Styles

            public static bool IsRunningOnMac()
            {
                try
                {
                    var buf = IntPtr.Zero;
                    try
                    {
                        buf = Marshal.AllocHGlobal(8192);
                        if (uname(buf) == 0)
                        {
                            var os = Marshal.PtrToStringAnsi(buf);
                            if (os == "Darwin")
                            {
                                return true;
                            }
                        }
                    }
                    finally
                    {
                        if (buf != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(buf);
                        }
                    }
                }
                catch
                {
                    // Ignore any other possible failures on non-OSX platforms
                }

                return false;
            }
        }
    }
}
#endif