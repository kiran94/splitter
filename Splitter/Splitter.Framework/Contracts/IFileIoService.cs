namespace Splitter.Framework
{
    using System.IO;

    /// <summary>
    /// Encapsulates File IO operations.
    /// </summary>
    public interface IFileIoService
    {
        /// <summary>
        /// Opens a stream to the given File.
        /// </summary>
        /// <param name="filePath">path to file</param>
        /// <param name="mode">mode of open</param>
        /// <param name="access">access mode.</param>
        /// <param name="share">defines how other processes can access the file.</param>
        /// <returns>Stream to the file.</returns>
        Stream Open(string filePath, FileMode mode, FileAccess access, FileShare share);

        /// <summary>
        /// Deletes a File.
        /// </summary>
        /// <param name="filePath">path of file to delete.</param>
        void Delete(string filePath);
    }
}