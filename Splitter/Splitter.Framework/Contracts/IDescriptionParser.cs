namespace Splitter.Framework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines Behaviour to parse Description data.
    /// </summary>
    public interface IDescriptionParser
    {
        /// <summary>
        /// Parses Tracks from the given description into a dictionary mapping track name to timestamp.
        /// </summary>
        /// <returns>mapping between trackname and timestamp.</returns>
        IDictionary<string, TimeSpan> ParseTracks(string description);
    }
}