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
        /// The File IO Service.
        /// </summary>
        private readonly IFileIoService fileIoService;

         /// <summary>
        /// Initializes a new instance of the <see cref="DownloadService"/> class.
        /// </summary>
        /// <param name="repository">injected repo.</param>
        /// <param name="fileIoService">injected file io.</param>

        public DownloadService(IYoutubeRepository repository, IFileIoService fileIoService)
        {
            this.repository = repository;
            this.fileIoService = fileIoService;
        }

        /// <inheritdoc />
        public void Download(Metadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (string.IsNullOrWhiteSpace(metadata.tempFileLocation))
            {
                throw new ArgumentNullException(nameof(metadata.tempFileLocation));
            }

            using (var stream = this.fileIoService.Open(metadata.tempFileLocation, FileMode.OpenOrCreate))
            {
                metadata.fileExtension = repository.GetAudio(metadata, stream);
            }

            var directory = this.fileIoService.GetDirectory(metadata.tempFileLocation);
            var tempFileWithoutExt = this.fileIoService.GetFileWithoutExt(metadata.tempFileLocation);
            var extension = metadata.fileExtension;

            var completeTemp = $"{directory}\\{tempFileWithoutExt}.{extension}";

            if (this.fileIoService.Exists(completeTemp))
            {
                this.fileIoService.Delete(completeTemp);
            }

            this.fileIoService.Move(metadata.tempFileLocation, completeTemp);
            this.fileIoService.Delete(metadata.tempFileLocation);

            metadata.tempFileLocation = completeTemp;
        }
    }
}