namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;

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
            var url = "https://www.youtube.com/watch?v=ppzcjw2Xq1Y";
            var descriptionRegex = @"(\d\d:\d\d)(\s|-)(.+)";
            var tempFile = "downloaded.tmp";

            var client = new YoutubeClient();
            var repository = new YoutubeRepository(client);
            var descriptionParser = new DescriptionParser(descriptionRegex);
            var downloadService = new DownloadService(repository);

            WriteLine("Getting Metadata");
            var metadata = repository.GetMetadata(url);

            WriteLine("Parsing Tracks");
            metadata.Tracks = descriptionParser.ParseTracks(metadata.Description);

            WriteLine($"Found {metadata.Tracks.Count} Tracks:");
            foreach(var currentTrack in metadata.Tracks)
            {
                Console.WriteLine($"{currentTrack.Key} {currentTrack.Value}");
            }

            WriteLine("Extracting Audio: " + metadata.Url);
            metadata.tempFileLocation = tempFile;
            downloadService.Download(metadata);

            WriteLine("Complete: " + metadata.tempFileLocation);
        }

        /// <summary>
        /// Writes user output.
        /// </summary>
        /// <param name="text">output to write.</param>
        private static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}