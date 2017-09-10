// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.System.IO;

namespace Spectre.System
{
    /// <summary>
    /// Represents the environment the application operates in.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        DirectoryPath ApplicationRoot { get; }

        /// <summary>
        /// Gets the platform the application is running on.
        /// </summary>
        /// <value>The platform the application is running on.</value>
        IPlatform Platform { get; }
    }
}