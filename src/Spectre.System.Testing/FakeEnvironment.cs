// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.System.IO;

namespace Spectre.System.Testing
{
    /// <summary>
    /// Represents a fake environment.
    /// </summary>
    public sealed class FakeEnvironment : IEnvironment
    {
        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        public DirectoryPath ApplicationRoot { get; set; }

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        IPlatform IEnvironment.Platform => Platform;

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        public FakePlatform Platform { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeEnvironment"/> class.
        /// </summary>
        /// <param name="family">The platform family.</param>
        /// <param name="is64Bit">if set to <c>true</c>, the platform is 64 bit.</param>
        public FakeEnvironment(PlatformFamily family, bool is64Bit = true)
        {
            Platform = new FakePlatform(family, is64Bit);
            if (Platform.IsUnix())
            {
                WorkingDirectory = new DirectoryPath("/Working");
                ApplicationRoot = new DirectoryPath("/working/bin");
            }
            else
            {
                WorkingDirectory = new DirectoryPath("C:/Working");
                ApplicationRoot = new DirectoryPath("C:/Working/bin");
            }
        }

        /// <summary>
        /// Changes the operative system bitness.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c>, this is a 64-bit operative system.</param>
        public void ChangeOperativeSystemBitness(bool is64Bit)
        {
            Platform.Is64Bit = is64Bit;
        }

        /// <summary>
        /// Change the operating system platform family.
        /// </summary>
        /// <param name="family">The platform family.</param>
        public void ChangeOperatingSystemFamily(PlatformFamily family)
        {
            Platform.Family = family;
        }
    }
}