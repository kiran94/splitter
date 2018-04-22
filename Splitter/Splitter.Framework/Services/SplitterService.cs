namespace Splitter.Framework
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using System.Globalization;
    using MediaToolkit;
    using MediaToolkit.Model;
    using MediaToolkit.Options;

    /// <inheritdoc />
    public class SplitterService : ISplitterService
    {
        /// <summary>
        /// The file IO service.
        /// </summary>
        private readonly IFileIoService fileIoService;

        /// <summary>
        /// Initialises a new instance of the <see cref="FileIoService" /> class.
        /// </summary>
        /// <param name="fileIoService"></param>
        public SplitterService(IFileIoService fileIoService)
        {
            this.fileIoService = fileIoService;
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


            var inputFile = new MediaFile(metadata.tempFileLocation);
            for (int i = 0; i < metadata.Tracks.Count - 1; i++)
            {
                var name = metadata.Tracks.Keys.ElementAt(i) + ".mp3";
                var outputFile = new MediaFile(name);

                using (Engine engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    var options = new ConversionOptions();

                    TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i).Value, TimespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var currentTrack);
                    TimeSpan.TryParseExact(metadata.Tracks.ElementAt(i+1).Value, TimespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var nextTrack);

                    options.CutMedia(currentTrack, nextTrack);

                    engine.Convert(inputFile, outputFile, options);
                    tracks.Add(name);
                }

                if (i == 2) break;
            }

            return tracks;
        }

        /// <summary>
        /// Computes the number of bytes to extract for the current track based on
        /// the current timestamp, next timestamp and the bytes per seconds.
        /// </summary>
        /// <param name="currentTrackTimestamp">current time track timestamp in timespan format.</param>
        /// <param name="nextTrackTimestamp">next time track timestamp in timespan format.</param>
        /// <param name="byteRate">bytes per second</param>
        /// <param name="timespanFormat">format that the timespan is being passed.</param>
        /// <returns>The total number of bytes to extract.</returns>
        private long ComputeBytesToExtract(string currentTrackTimestamp, string nextTrackTimestamp, long byteRate, string timespanFormat)
        {
            if (!TimeSpan.TryParseExact(currentTrackTimestamp, timespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var currentTrack))
            {
                throw new InvalidOperationException($"Could not parse {currentTrackTimestamp} into the timspan: {timespanFormat}");
            }

            if (!TimeSpan.TryParseExact(nextTrackTimestamp, timespanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var nextTrack))
            {
                throw new InvalidOperationException($"Could not parse {nextTrackTimestamp} into the timspan: {timespanFormat}");
            }

            var diff = nextTrack - currentTrack;
            return (long) diff.TotalSeconds * byteRate;
        }

        // /// <summary>
        // /// Extracts the track from the passed complete file stream into a distinct file.
        // /// </summary>
        // /// <param name="completeFile">stream of the complete file.</param>
        // /// <param name="metadata">metadata object</param>
        // /// <param name="trackName">current track we are extracting.</param>
        // /// <param name="startingPosition">starting position in the complete file we are extracting</param>
        // /// <param name="bytesToExtract">number of bytes to extract.</param>
        // /// <returns>file output location of generated file.</returns>
        // private string ExtractTrack(Stream completeFile, Metadata metadata, string trackName, long startingPosition, long bytesToExtract)
        // {
        //     var trackLocation = $"{trackName}.{metadata.fileExtension}";
        //     using (var baseStream = this.fileIoService.Open(trackLocation, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
        //     {
        //         var buffer = new byte[bytesToExtract];

        //         //completeFile.Position = startingPosition;

        //         completeFile.Read(buffer, 0, (int)bytesToExtract);
        //         baseStream.Write(buffer, 0, buffer.Length);
        //     }

        //     return trackLocation;
        // }
    }
}