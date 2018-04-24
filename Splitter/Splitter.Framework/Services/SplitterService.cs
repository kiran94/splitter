namespace Splitter.Framework
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Humanizer;

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

            if (metadata.Tracks == null || metadata.Tracks.Count == 0)
            {
                throw new ArgumentException($"No Tracks found");
            }

            /// Add final entry for the last track.
            metadata.Tracks.Add(string.Empty, metadata.Duration);

            var tracks = new List<string>(metadata.Tracks.Count);
            for (int i = 0; i < metadata.Tracks.Count - 1; i++)
            {
                var outputFile = metadata.Tracks.Keys.ElementAt(i).Dehumanize() + ".mp3";

                var currentTrack = metadata.Tracks.ElementAt(i).Value;
                var nextTrack = metadata.Tracks.ElementAt(i+1).Value;

                var diff = (int) Math.Ceiling((nextTrack - currentTrack).TotalSeconds);
                this.ffmpegService.Slice(metadata.tempFileLocation, (int) Math.Ceiling(currentTrack.TotalSeconds), diff, outputFile);

                this.fileIoService.AddMeta(outputFile, metadata.Tracks.Keys.ElementAt(i), metadata.Title, metadata.Author, i+1, metadata.Tracks.Count, metadata.Thumbnail);
                tracks.Add(outputFile);
            }

            return tracks;
        }
    }
}