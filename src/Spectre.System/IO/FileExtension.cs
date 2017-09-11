using System;

namespace Spectre.System.IO
{
    public sealed class FileExtension
    {
        public string Name { get; }

        public FileExtension(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name.Trim().TrimStart('.') ?? string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public static FileExtension Parse(FilePath path)
        {
            return path == null 
                ? new FileExtension(string.Empty) 
                : new FileExtension(global::System.IO.Path.GetExtension(path.FullPath));
        }

        internal static string Change(string path, FileExtension extension)
        {
            return Append(Remove(path), extension);
        }
        
        internal static string Append(string path, FileExtension extension)
        {
            if (extension == null)
            {
                return path;
            }
            if (string.IsNullOrWhiteSpace(extension.Name))
            {
                return path;
            }
            return string.Concat(path, ".", extension.Name);
        }
        
        internal static string Remove(string path)
        {
            return global::System.IO.Path.ChangeExtension(path, string.Empty).TrimEnd('.');
        }
    }
}