namespace Splitter.Framework.Tests
{
    using System;
    using NUnit.Framework;
    using Moq;

    /// <summary>
    /// Tests for the cleanup service.
    /// </summary>
    public class CleanupServiceTests
    {
        /// <summary>
        /// The File io service.
        /// </summary>
        private Mock<IFileIoService> fileIoService;

        /// <summary>
        /// Sets up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.fileIoService = new Mock<IFileIoService>();
        }

        /// <summary>
        /// Ensures when the temp file location is null, empty or whitespace then an exception is raised.
        /// </summary>
        [Test]
        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("    ")]
        public void CleanUp_TempFileLocationNullOrEmpty_Exception(string tempFile)
        {
            var metadata = new Metadata();
            metadata.tempFileLocation = tempFile;

            var service = this.GetInstance();

            Assert.Throws<ArgumentException>(delegate
            {
                service.CleanUp(metadata);
            });
        }

        /// <summary>
        /// Ensures when the temp file location does not exist, an exception is raised.
        /// </summary>
        [Test]
        public void CleanUp_TempFileLocationDoesNotExist_Exception()
        {
            var metadata = new Metadata();
            metadata.tempFileLocation = "downloaded.tmp";

            this.fileIoService.Setup(x => x.Exists(metadata.tempFileLocation)).Returns(false);

            var service = this.GetInstance();
            Assert.Throws<ArgumentException>(delegate
            {
                service.CleanUp(metadata);
            });
        }

        /// <summary>
        /// Ensures when the thumbnail and temp file exist, they are both deleted.
        /// </summary>
        [Test]
        public void CleanUp_ThumbnailAndTempFileExists_Deleted()
        {
            var metadata = new Metadata();
            metadata.tempFileLocation = "downloaded.tmp";
            metadata.Thumbnail = "thumbnail.jpg";

            this.fileIoService.Setup(x => x.Exists(metadata.tempFileLocation)).Returns(true);
            this.fileIoService.Setup(x => x.Exists(metadata.Thumbnail)).Returns(true);

            var service = this.GetInstance();
            service.CleanUp(metadata);

            this.fileIoService.Verify(x => x.Delete(metadata.tempFileLocation));
            this.fileIoService.Verify(x => x.Delete(metadata.Thumbnail));
        }

        /// <summary>
        /// Ensures when the thumbnail does not exist, only the temp file location is deleted.
        /// </summary>
        [Test]
        public void CleanUp_ThumbnailDoesNotExist_NoDeleteOnThumbnail()
        {
            var metadata = new Metadata();
            metadata.tempFileLocation = "downloaded.tmp";
            metadata.Thumbnail = null;

            this.fileIoService.Setup(x => x.Exists(metadata.tempFileLocation)).Returns(true);
            this.fileIoService.Setup(x => x.Exists(metadata.Thumbnail)).Returns(false);

            var service = this.GetInstance();
            service.CleanUp(metadata);

            this.fileIoService.Verify(x => x.Delete(metadata.tempFileLocation));
            this.fileIoService.Verify(x => x.Delete(metadata.Thumbnail), Times.Never);
        }

        /// <summary>
        /// Gets an instance.
        /// </summary>
        /// <returns>the instance.</returns>
        private CleanupService GetInstance()
        {
            return new CleanupService(this.fileIoService.Object);
        }
    }
}