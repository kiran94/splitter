namespace Splitter.Framework
{
    /// <summary>
    /// Defines the behaviour for downloading files.
    /// </summary>
    public interface IDownloadService
    {
        /// <summary>
        /// Downloads the files given the Meta data object.
        /// </summary>
        /// <param name="metadata">meta data to use for download.</param>
        void Download(Metadata metadata);
    }
}