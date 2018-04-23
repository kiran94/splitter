namespace Splitter.Framework
{
    using System.IO;

    /// <inheritdoc />
    public class FileIoService : IFileIoService
    {
        /// <inheritdoc />
        public Stream Open(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(filePath, mode, access, share);
        }

        /// <inheritdoc />
        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }

         /// <inheritdoc />
        public void Move(string sourcePath, string targetPath)
        {
            File.Move(sourcePath, targetPath);
        }

        /// <inheritdoc />
        public FileStream Open(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }

        /// <inheritdoc />
        public string GetDirectory(string path)
        {
            return Path.GetDirectoryName(path);
        }

         /// <inheritdoc />
        public string GetFileWithoutExt(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}