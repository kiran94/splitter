namespace Splitter.Framework
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Humanizer;

    using System.Globalization;

    /// <inheritdoc />
    public class SplitterService : ISplitterService
    {
        /// <summary>
        /// The file IO service.
        /// </summary>
        private readonly IFileIoService fileIoService;

        /// <summary>
        /// The ffmpeg service.
        /// </summary>
        private readonly IFFmpegService ffmpegService;

        /// <summary>
        /// The timespan format, timestamps are in.
        /// </summary>
        private readonly string timeSpanFormat; 

        /// <summary>
        /// Initialises a new instance of the <see cref="SplitterService" /> class.
        /// </summary>
        /// <param name="fileIoService">injected file io service.</param>
        /// <param name="ffmpegService">injected ffmpeg service.</param>
        public SplitterService(IFileIoService fileIoService, IFFmpegService ffmpegService)
        {
            this.fileIoService = fileIoService;
            this.ffmpegService = ffmpegService;

            this.timeSpanFormat = @"mm\:ss"; 
        }

        /// <inheritdoc />
        public IList<string> Split(Metadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentException($"{metadata} was null");
            }

            if (metadata.Tracks.Count == 0)
            {
                throw new ArgumentException($"No Tracks found");
            }

            /// Add final entry for the last track.
            metadata.Tracks.Add(string.Empty, metadata.Duration.ToString(this.timeSpanFormat));

            var tracks = new List<string>(metadata.Tracks.Count);
            for (int i = 0; i < metadata.Tracks.Count - 1; i++)
            {
                var outputFile = metadata.Tracks.Keys.ElementAt(i).Dehumanize() + ".mp3";

                TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i).Value, this.timeSpanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var currentTrack);
                TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i+1).Value, this.timeSpanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var nextTrack);

                var diff = (int) System.Math.Ceiling((nextTrack - currentTrack).TotalSeconds);
                this.ffmpegService.Slice(metadata.tempFileLocation, (int)System.Math.Ceiling(currentTrack.TotalSeconds), diff, outputFile);
               

                // Add meta information.
                var file = TagLib.File.Create(outputFile);
                file.Tag.Title = metadata.Tracks.Keys.ElementAt(i);
                file.Tag.Album = metadata.Title;
                file.Tag.AlbumArtists = new[] { metadata.Author };
                file.Save();

                tracks.Add(outputFile);
            }

            return tracks;
        }
    }
}