// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Spectre.System.IO;

namespace Spectre.System
{
    /// <summary>
    /// Represents the environment the application operates in.
    /// </summary>
    public sealed class Environment : IEnvironment
    {
        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory
        {
            get => new DirectoryPath(global::System.IO.Directory.GetCurrentDirectory());
            set => SetWorkingDirectory(value);
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        public DirectoryPath ApplicationRoot { get; }

        /// <summary>
        /// Gets the platform the application is running on.
        /// </summary>
        /// <value>The platform the application is running on.</value>
        public IPlatform Platform { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Environment" /> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        public Environment(IPlatform platform)
        {
            Platform = platform;

            // Get the application root.
            var assembly = Assembly.GetExecutingAssembly();
            var path = global::System.IO.Path.GetDirectoryName(assembly.Location);
            ApplicationRoot = new DirectoryPath(path);

            // Get the working directory.
            WorkingDirectory = new DirectoryPath(global::System.IO.Directory.GetCurrentDirectory());
        }

        private static void SetWorkingDirectory(DirectoryPath path)
        {
            if (path.IsRelative)
            {
                throw new InvalidOperationException("Working directory can not be set to a relative path.");
            }
            global::System.IO.Directory.SetCurrentDirectory(path.FullPath);
        }
    }
}