namespace Splitter.Framework
{
    using System;

    /// <inheritdoc />
    public class CleanupService : ICleanupService
    {
        /// <summary>
        /// The File IO Service.
        /// </summary>
        private readonly IFileIoService fileIoService;

        /// <summary>
        /// Initialises a new instance of <see cref="CleanupService" />
        /// </summary>
        /// <param name="fileIoService">file io service</param>
        public CleanupService(IFileIoService fileIoService)
        {
            this.fileIoService = fileIoService;
        }

        /// <inheritdoc />
        public void CleanUp(Metadata metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata.tempFileLocation) || !this.fileIoService.Exists(metadata.tempFileLocation))
            {
                throw new ArgumentException($"{metadata.tempFileLocation} could not be found.");
            }

            // It is possible that we may get no thumbnail for a video, however it if does exist
            // then we want to check the file has been downloaded.
            if (this.fileIoService.Exists(metadata.Thumbnail))
            {
                 this.fileIoService.Delete(metadata.Thumbnail);
            }

            this.fileIoService.Delete(metadata.tempFileLocation);
        }
    }
}