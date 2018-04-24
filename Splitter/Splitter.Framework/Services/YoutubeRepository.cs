namespace Splitter.Framework
{
    using System;
    using System.Linq;
    using System.IO;
    using YoutubeExplode;
    using System.Net;

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
                Url = url,
                Thumbnail = video.Thumbnails.StandardResUrl
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

        /// <inheritdoc />
        public void GetThumbnail(Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata.Thumbnail))
            {
                return;
            }

            const string path = "thumbnail.jpg";

            using (var client = new WebClient())
            {
                var thumbnailBytes = client.DownloadData(metadata.Thumbnail);
                using (var stream = new MemoryStream(thumbnailBytes))
                using (var writer = new FileStream(path, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(writer);
                    metadata.Thumbnail = path;
                }
            }
        }
    }
}