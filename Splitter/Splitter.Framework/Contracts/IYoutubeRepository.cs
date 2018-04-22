namespace Splitter.Framework
{
    using System.IO;

    /// <summary>
    /// Defines behaviour for interacting with Youtube.
    /// </summary>
    public interface IYoutubeRepository
    {
        /// <summary>
        /// Gets the Metadata of a Youtube Video.
        /// </summary>
        /// <param name="url">URL of the youtube video to retrieve</param>
        /// <returns>metadata object of the video.</returns>
        Metadata GetMetadata(string url);

        /// <summary>
        /// Gets the Audio of a Youtube Video in the form of a Stream.
        /// </summary>
        /// <param name="metadata">Metadata object.</param>
        /// <param name="output">stream to write data too.</param>
        /// <returns>File extension of downloaded data</returns>
        string GetAudio(Metadata metadata, Stream output);
    }
}