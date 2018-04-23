namespace Splitter.Framework.Tests
{
    using NUnit.Framework;
    using Moq;
    using System;
    using System.IO;

    /// <summary>
    /// Tests for the DownloadService
    /// </summary>
    [TestFixture]
    public class DownloadServiceTests
    {
        /// <summary>
        /// Youtube Repository.
        /// </summary>
        private Mock<IYoutubeRepository> youtubeRepository;

        /// <summary>
        /// File IO Service.
        /// </summary>
        private Mock<IFileIoService> fileIoService;

        /// <summary>
        /// The set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.youtubeRepository = new Mock<IYoutubeRepository>();
            this.fileIoService = new Mock<IFileIoService>();
        }

        /// <summary>
        /// Ensures when a null meta data is passed, an exception is thrown.
        /// </summary>
        [Test]
        public void Download_NullMetaData_Exception()
        {
            Metadata meta = null;

            var service = this.GetInstance();
            Assert.Throws<ArgumentNullException>(delegate
            {
                service.Download(meta);
            });
        }

        /// <summary>
        /// Ensures when the temp file location in metadata is null, an exception is thrown
        /// </summary>
        [Test]
        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Download_NullTempFileLocation_Exception(string location)
        {
            Metadata meta = new Metadata();
            meta.tempFileLocation = location;

            var service = this.GetInstance();
            Assert.Throws<ArgumentNullException>(delegate
            {
                service.Download(meta);
            });
        }

        /// <summary>
        /// Ensures when the temp file location is provided,
        /// get audio is called on the repository
        /// and the temp file is switched.
        /// </summary>
        [Test]
        public void Download_TempFileLocationProvided_GetAudioCalled_TempFileSwitched()
        {
            Metadata meta = new Metadata();
            meta.tempFileLocation = "tempfile.tmp";

            using (var stream = new MemoryStream())
            {
                this.fileIoService.Setup(x => x.Open(meta.tempFileLocation, FileMode.OpenOrCreate)).Returns(stream);
                this.fileIoService.Setup(x => x.GetDirectory(meta.tempFileLocation)).Returns("\\directory");
                this.fileIoService.Setup(x => x.GetFileWithoutExt(meta.tempFileLocation)).Returns("downloaded");

                this.youtubeRepository
                    .Setup(x => x.GetAudio(meta, stream))
                    .Returns("mp3");

                var completeTemp = "\\directory\\downloaded.mp3";

                this.fileIoService.Setup(x => x.Exists(completeTemp)).Returns(true);
                this.fileIoService.Setup(x => x.Delete(completeTemp)).Verifiable();

                this.fileIoService.Setup(x => x.Move(meta.tempFileLocation, completeTemp)).Verifiable();
                this.fileIoService.Setup(x => x.Delete(meta.tempFileLocation));

                var service = this.GetInstance();
                service.Download(meta);

                Assert.AreEqual(completeTemp, meta.tempFileLocation);

                this.fileIoService.VerifyAll();
                this.youtubeRepository.VerifyAll();
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>the instance.</returns>
        private IDownloadService GetInstance()
        {
            return new DownloadService(this.youtubeRepository.Object, this.fileIoService.Object);
        }
    }
}