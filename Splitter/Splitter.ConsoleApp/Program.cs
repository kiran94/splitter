namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;

    class Program
    {
        static void Main(string[] args)
        {
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
        }

        static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
