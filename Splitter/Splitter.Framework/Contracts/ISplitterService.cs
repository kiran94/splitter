namespace Splitter.Framework
{
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates logic for splitting an audio file into distinct chunks.
    /// </summary>
    public interface ISplitterService
    {
        /// <summary>
        /// Splits the given temporary file according to the meta data into distinct tracks according to the metadata tracks.
        ///
        /// The Byte rate is used along with the difference in seconds between each track to determine how to split the information.
        /// </summary>
        /// <param name="metadata">The metadata used to split the information.</param>
        /// <returns>A list of file locations in which data has been split.</returns>
        IList<string> Split(Metadata metadata);
    }
}