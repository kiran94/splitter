namespace Splitter.Framework
{
    using System.IO;

    /// <inheritdoc />
    public class DownloadService : IDownloadService
    {
        /// <summary>
        /// The Youtube Repository.
        /// </summary>
        private readonly IYoutubeRepository repository;

         /// <summary>
        /// Initializes a new instance of the <see cref="DownloadService"/> class.
        /// </summary>
        /// <param name="repository">injected repo.</param>

        public DownloadService(IYoutubeRepository repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc />
        public void Download(Metadata metadata)
        {
            var ext = string.Empty;
            using (var stream = new FileStream(metadata.tempFileLocation, FileMode.OpenOrCreate))
            {
                ext = repository.GetAudio(metadata.Url, stream);
            }

            var completeTemp = $"{Path.GetFileNameWithoutExtension(metadata.tempFileLocation)}.{ext}";
            if (File.Exists(completeTemp))
            {
                File.Delete(completeTemp);
            }

            File.Move(metadata.tempFileLocation, completeTemp);
            File.Delete(metadata.tempFileLocation);

            metadata.tempFileLocation = completeTemp;
        }
    }
}