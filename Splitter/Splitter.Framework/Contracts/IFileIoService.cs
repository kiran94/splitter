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

        /// <summary>
        /// Moves a file from the source to taget.
        /// </summary>
        /// <param name="sourcePath">to move from.</param>
        /// <param name="targetPath">to move too.</param>
        void Move(string sourcePath, string targetPath);

        /// <summary>
        /// Opens a File stream to the path with the given mode.
        /// </summary>
        /// <param name="path">path of file to open.</param>
        /// <param name="mode">mode of open.</param>
        /// <returns></returns>
        Stream Open(string path, FileMode mode);

        /// <summary>
        /// Gets the Directory of the path.
        /// </summary>
        /// <param name="path">path to get directory.</param>
        /// <returns>the directory.</returns>
        string GetDirectory(string path);

        /// <summary>
        /// Gets the File name without the extension.
        /// </summary>
        /// <param name="path">file path of file</param>
        /// <returns>file name</returns>
        string GetFileWithoutExt(string path);

        /// <summary>
        /// Determines if a file exists or not.
        /// </summary>
        /// <param name="path">file path to check</param>
        /// <returns>flag if it exists.</returns>
        bool Exists(string path);

        /// <summary>
        /// Adds Audio Meta information to the given file.
        /// </summary>
        /// <param name="path">path to audio file to modify.</param>
        /// <param name="title">audio title meta</param>
        /// <param name="album">audio album meta</param>
        /// <param name="author">audio author meta</param>
        /// <param name="trackNo">audio track no meta</param>
        /// <param name="trackCount">audio track total count meta</param>
        /// <param name="thumbnailPath">path to thumbnail image.</param>
        void AddMeta(string path, string title, string album, string author, int trackNo, int trackCount, string thumbnailPath);
    }
}