namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            const string tempFile = "downloaded.tmp";

            var url = "https://www.youtube.com/watch?v=ppzcjw2Xq1Y";
            var client = new YoutubeClient();
            var repository = new YoutubeRepository(client);
            var descriptionParser = new DescriptionParser();

            WriteLine("Getting Metadata");
            var description = repository.GetDescription(url);

            WriteLine("Parsing Tracks");
            var tracks = descriptionParser.ParseTracks(description);

            WriteLine($"Found {tracks.Count} Tracks:");
            foreach(var currentTrack in tracks)
            {
                Console.WriteLine(currentTrack.Key + " " + currentTrack.Value);
            }

            WriteLine("Extracting Audio: " + url);
            string ext = string.Empty;
            using (var stream = new FileStream(tempFile, FileMode.OpenOrCreate))
            {
                ext = repository.GetAudio(url, stream);
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

        private static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
