namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;
    using System.Diagnostics;
    using System.IO;
    using CommandLine;

    /// <summary>
    /// Main Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">command line arguments.</param>
        static void Main(string[] args)
        {
            var options = ParseArguments(args);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var url = options.Url;
            var ffmpegLocation = options.ffmpegLocation;
            var ffmpegTimeout = options.ffmpegTimeout;
            var verbose = options.Verbose;

            var descriptionRegex = @"(?<time>\d{1,2}:\d{2}|\d{1,2}:\d{2}:\d{2})(\s|-)(?<title>.+)";
            var tempFile = "downloaded.tmp";

            var client = new YoutubeClient();
            var repository = new YoutubeRepository(client);
            var descriptionParser = new DescriptionParser(descriptionRegex);
            var fileIo = new FileIoService();
            var downloadService = new DownloadService(repository, fileIo);
            var ffmpegService = new FFmpegService(ffmpegLocation, ffmpegTimeout);
            var splitterService = new SplitterService(fileIo, ffmpegService);

            WriteLine("Getting Metadata", verbose);
            var metadata = repository.GetMetadata(url);

            WriteLine("Found " + metadata.Title, true);

            WriteLine("Getting Thumbnail", verbose);
            repository.GetThumbnail(metadata);

            WriteLine("Parsing Tracks", verbose);
            metadata.Tracks = descriptionParser.ParseTracks(metadata.Description);

            if (verbose)
            {
                WriteLine($"Found {metadata.Tracks.Count} Tracks:", verbose);
                foreach(var currentTrack in metadata.Tracks)
                {
                    Console.WriteLine($"{currentTrack.Key} {currentTrack.Value}");
                }
            }

            WriteLine("Extracting Audio: " + metadata.Url, true);
            metadata.tempFileLocation = tempFile;
            downloadService.Download(metadata);

            WriteLine($"Splitting Audio into {metadata.Tracks.Count} tracks", true);
            var tracks =  splitterService.Split(metadata);

            WriteLine("Output:", true);
            foreach (var track in tracks)
            {
                Console.WriteLine(track);
            }

            WriteLine("Cleaning temp files", verbose);
            fileIo.Delete(metadata.tempFileLocation);

            if (!string.IsNullOrWhiteSpace(metadata.Thumbnail))
            {
                fileIo.Delete(metadata.Thumbnail);
            }

            if (verbose)
            {
                Console.WriteLine("Done");
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " ms.");
            }
        }

        /// <summary>
        /// Writes user output.
        /// </summary>
        /// <param name="text">output to write.</param>
        private static void WriteLine(string text, bool show)
        {
            if (show)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Parses the given command line arguments.
        /// </summary>
        /// <param name="args">arguments to parse</param>
        /// <returns>Parsed Arguments or if there was a failure, writes help.</returns>
        private static Arguments ParseArguments(string[] args)
        {
            var option = default(Arguments);
            var writer = new StringWriter();
            var parser = new Parser(conf => conf.HelpWriter = writer);

            var result = parser.ParseArguments<Arguments>(args);
            result.WithNotParsed(_ =>
            {
                Console.WriteLine(writer.ToString());
                System.Environment.Exit(1);
            })
            .WithParsed(opt =>
            {
                option = opt;
            });

            return option;
        }
    }
}