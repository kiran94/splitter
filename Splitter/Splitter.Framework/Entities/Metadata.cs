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
        public IDictionary<string, TimeSpan> Tracks { get; set; }

        /// <summary>
        /// Gets or sets the temporary file location for downloading files.
        /// </summary>
        /// <returns>temp file location.</returns>
        public string tempFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <returns>the file extension.</returns>
        public string fileExtension { get; set; }

        /// <summary>
        /// Gets or sets the Thumbnail.
        /// </summary>
        /// <returns></returns>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Prints Tracks to Console.
        /// </summary>
        public void PrintTracks()
        {
            if (this.Tracks == null || this.Tracks.Count == 0)
            {
                Console.WriteLine("No Tracks.");
                return;
            }

            foreach(var currentTrack in this.Tracks)
            {
                Console.WriteLine($"{currentTrack.Key} {currentTrack.Value}");
            }
        }
    }
}