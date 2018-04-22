namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;
    using System.IO;

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
            const string tempFile = "downloaded.tmp";

            var url = "https://www.youtube.com/watch?v=ppzcjw2Xq1Y";
            var descriptionRegex = @"(\d\d:\d\d)(\s|-)(.+)";

            var client = new YoutubeClient();
            var repository = new YoutubeRepository(client);
            var descriptionParser = new DescriptionParser(descriptionRegex);

            WriteLine("Getting Metadata");
            var metadata = repository.GetMetadata(url);

            WriteLine("Parsing Tracks");
            metadata.Tracks = descriptionParser.ParseTracks(metadata.Description);

            WriteLine($"Found {metadata.Tracks.Count} Tracks:");
            foreach(var currentTrack in metadata.Tracks)
            {
                Console.WriteLine(currentTrack.Key + " " + currentTrack.Value);
            }

            WriteLine("Extracting Audio: " + metadata.Url);
            var ext = string.Empty;
            using (var stream = new FileStream(tempFile, FileMode.OpenOrCreate))
            {
                ext = repository.GetAudio(metadata.Url, stream);
            }

            var completeTemp = $"downloaded.{ext}";
            WriteLine($"Renaming {tempFile} -> " + completeTemp);
            if (File.Exists(completeTemp))
            {
                File.Delete(completeTemp);
            }

            File.Move(tempFile, completeTemp);
            File.Delete(tempFile);
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