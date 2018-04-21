namespace Splitter.Framework
{
    using System;
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
        public string GetDescription(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL was null or empty");
            }

            url = YoutubeClient.ParseVideoId(url);
            return this.client.GetVideoAsync(url).Result.Description;
        }
    }
}