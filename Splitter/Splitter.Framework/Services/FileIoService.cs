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
    }
}