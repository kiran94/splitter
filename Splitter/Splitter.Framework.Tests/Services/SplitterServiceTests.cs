namespace Splitter.Framework
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Moq;
    using Humanizer;

    /// <summary>
    /// Tests for the SplitterService.
    /// </summary>
    [TestFixture]
    public class SplitterServiceTests
    {
        /// <summary>
        /// The File IO service.
        /// </summary>
        private Mock<IFileIoService> fileIoService;

        /// <summary>
        /// The FFMpeg Service.
        /// </summary>
        private Mock<IFFmpegService> ffmpegService;

        /// <summary>
        /// Sets up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.fileIoService = new Mock<IFileIoService>();
            this.ffmpegService = new Mock<IFFmpegService>();
        }

        /// <summary>
        /// Ensures when the metadata passed is null, then an exception is raised.
        /// </summary>
        [Test]
        public void Split_NullMetadata_Exception()
        {
            Metadata metadata = null;

            Assert.Throws<ArgumentException>(delegate
            {
                this.GetInstance().Split(metadata, (string s) => {return;});
            });
        }

        /// <summary>
        /// Ensures when there are null tracks in the metadata, then an exception is raised.
        /// </summary>
        [Test]
        public void Spit_NullTracks_Exception()
        {
            Metadata metadata = new Metadata();
            metadata.Tracks = null;

            Assert.Throws<ArgumentException>(delegate
            {
                this.GetInstance().Split(metadata, (string s) => {return;});
            });
        }

         /// Ensures when there are no tracks in the metadata, then an exception is raised.
        /// </summary>
        [Test]
        public void Spit_NoTracks_Exception()
        {
            Metadata metadata = new Metadata();
            metadata.Tracks =new Dictionary<string, TimeSpan>();

            Assert.Throws<ArgumentException>(delegate
            {
                this.GetInstance().Split(metadata, (string s) => {return;});
            });
        }

        /// <summary>
        /// Ensures when there are tracks provided, then the tracks are
        /// sliced, the meta data is added and the file locations are returned.
        /// </summary>
        [Test]
        public void Split_TracksFound_Sliced_MetaDataAdded_FileLocationsReturned()
        {
            var metadata = new Metadata();
            metadata.tempFileLocation = "temp.WebM";
            metadata.Title = "Title";
            metadata.Author = "Author";
            metadata.Duration = new TimeSpan(0, 4, 0);
            metadata.Thumbnail = "thumbnail.jpg";

            metadata.Tracks = new Dictionary<string, TimeSpan>();
            metadata.Tracks.Add("Track 1", new TimeSpan(0, 0, 0));
            metadata.Tracks.Add("Track 2", new TimeSpan(0, 1, 30));
            metadata.Tracks.Add("Track 3", new TimeSpan(0, 2, 45));
            metadata.Tracks.Add("Track 4", new TimeSpan(0, 3, 0));

            var service = this.GetInstance();
            var tracks = service.Split(metadata, (string s) => { return; });

            Assert.AreEqual(4, tracks.Count);
            Assert.That(tracks.Contains("Track1.mp3"));
            Assert.That(tracks.Contains("Track2.mp3"));
            Assert.That(tracks.Contains("Track3.mp3"));
            Assert.That(tracks.Contains("Track4.mp3"));

            this.ffmpegService
                .Verify(x => x.Slice(
                                metadata.tempFileLocation,
                                It.IsIn(metadata.Tracks.Values.Select(y => (int) Math.Ceiling(y.TotalSeconds))),
                                It.IsAny<int>(),
                                It.IsIn(metadata.Tracks.Keys.Select(y => y.Dehumanize() + ".mp3"))));

            this.fileIoService
                .Verify(x => x.AddMeta(
                                It.IsIn(metadata.Tracks.Keys.Select(y => y.Dehumanize() + ".mp3")),
                                It.IsIn(metadata.Tracks.Keys.Select(y => y)),
                                metadata.Title,
                                metadata.Author,
                                It.IsIn(1, 2, 4, 4),
                                metadata.Tracks.Count, 
                                metadata.Thumbnail));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>the instance.</returns>
        private ISplitterService GetInstance()
        {
            return new SplitterService(this.fileIoService.Object, this.ffmpegService.Object);
        }
    }
}