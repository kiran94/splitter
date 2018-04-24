namespace Splitter.Framework
{
    using System;
    using System.Linq;
    using System.IO;
    using YoutubeExplode;

    /// <inheritdoc />
    public class YoutubeRepository : IYoutubeRepository
    {
        /// <summary>
        /// The Youtube Client.
        /// </summary>
        private readonly IYoutubeClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="YoutubeRepository"/> class.
        /// </summary>
        /// <param name="client">injected client.</param>
        public YoutubeRepository(IYoutubeClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public Metadata GetMetadata(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL was null or empty");
            }

            string Id = YoutubeClient.ParseVideoId(url);
            var video = this.client.GetVideoAsync(Id).Result;

            var metadata = new Metadata()
            {
                Title = video.Title,
                Author = video.Author,
                Description = video.Description,
                Duration = video.Duration,
                Url = url
            };

            return metadata;
        }

        /// <inheritdoc />
        public string GetAudio(Metadata metadata, Stream output)
        {
            if (string.IsNullOrWhiteSpace(metadata.Url))
            {
                throw new ArgumentException("URL was null or empty");
            }

            string id = YoutubeClient.ParseVideoId(metadata.Url);
            var streamInfoSet = this.client.GetVideoMediaStreamInfosAsync(id).Result;

            var streamInfo = streamInfoSet.Audio.First();

            this.client.DownloadMediaStreamAsync(streamInfo, output).Wait();
            return streamInfo.Container.ToString();
        }
    }
}