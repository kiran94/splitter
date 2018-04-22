namespace Splitter.Framework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulates Youtube Video metadata.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <returns>the title.</returns>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Author.
        /// </summary>
        /// <returns>the author.</returns>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <returns>the description</returns>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        /// <returns>the duration.</returns>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <returns>the url.</returns>
        public string Url { get; set;}

        /// <summary>
        /// Gets or sets the Tracks.
        /// </summary>
        /// <returns>the tracks.</returns>
        public IDictionary<string, string> Tracks { get; set; }
    }
}