namespace Splitter.Framework
{
    using System; 
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
                metadata.fileExtension = repository.GetAudio(metadata, stream);
            }

            Console.WriteLine();
            var completeTemp = $"{Path.GetDirectoryName(metadata.tempFileLocation)}\\{Path.GetFileNameWithoutExtension(metadata.tempFileLocation)}.{metadata.fileExtension}";
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