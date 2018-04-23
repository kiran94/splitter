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
        /// Initialises a new instance of the <see cref="SplitterService" /> class.
        /// </summary>
        /// <param name="fileIoService">injected file io service.</param>
        /// <param name="ffmpegService">injected ffmpeg service.</param>
        public SplitterService(IFileIoService fileIoService, IFFmpegService ffmpegService)
        {
            this.fileIoService = fileIoService;
            this.ffmpegService = ffmpegService;
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

            const string TimespanFormat = @"mm\:ss";
            var tracks = new List<string>(metadata.Tracks.Count);

            for (int i = 0; i < metadata.Tracks.Count - 1; i++)
            {
                var outputFile = metadata.Tracks.Keys.ElementAt(i).Dehumanize() + ".mp3";

                TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i).Value, TimespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var currentTrack);
                TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i+1).Value, TimespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var nextTrack);

                var diff = (int) System.Math.Ceiling((nextTrack - currentTrack).TotalSeconds);
                this.ffmpegService.Slice(metadata.tempFileLocation, (int)System.Math.Ceiling(currentTrack.TotalSeconds), diff, outputFile);
                tracks.Add(outputFile);
            }

            // logic for final track..

            return tracks;
        }
    }
}