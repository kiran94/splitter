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

            var description = repository.GetDescription(url);
            Console.WriteLine(description);
        }
    }
}
