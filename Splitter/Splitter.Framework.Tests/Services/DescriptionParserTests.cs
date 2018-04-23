namespace Splitter.Framework.Tests
{
    using System;
    using Splitter.Framework;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the DescriptionParser class.
    /// </summary>
    public class DescriptionParserTests
    {
        /// <summary>
        /// Ensures when the passed description is null or empty, an argument exception is thrown.
        /// </summary>
        [Test]
        [TestCase("")]
        [TestCase((string)null)]
        [TestCase("     ")]
        public void ParseTracks_NullEmptyDescription_ArgumentException(string desc)
        {
            var service = this.GetInstance();

            Assert.Throws<ArgumentException>(() =>
            {
                service.ParseTracks(desc);
            });
        }

        /// <summary>
        /// Ensures when there are no tracks found, an invalid exception is thrown.
        /// </summary>
        [Test]
        public void ParseTracks_NoTracksFound_InvalidOperationException()
        {
            var desc = "this is a description with no tracks";
            var service = this.GetInstance();

            Assert.Throws<InvalidOperationException>(() =>
            {
                service.ParseTracks(desc);
            });
        }

        /// <summary>
        /// Ensures when there are tracks which are space delimited, they are parsed.
        /// </summary>
        [Test]
        public void ParseTracks_TracksWithSpaceDelimited_Parsed()
        {
            var desc = "1.- 00:00 Salamander\n2.- 02:13 Mahou Hatsudou\n3.- 03:21 Dragon Force";
            var service = this.GetInstance();

            var tracks = service.ParseTracks(desc);

            Assert.AreEqual(3, tracks.Count);

            Assert.That(tracks.ContainsKey("Salamander"));
            Assert.That(tracks.ContainsKey("Mahou Hatsudou"));
            Assert.That(tracks.ContainsKey("Dragon Force"));

            Assert.AreEqual("00:00:00", tracks["Salamander"].ToString());
            Assert.AreEqual("00:02:13", tracks["Mahou Hatsudou"].ToString());
            Assert.AreEqual("00:03:21", tracks["Dragon Force"].ToString());
        }

        /// <summary>
        /// Ensures when there are tracks which are dash delimited, they are parsed.
        /// </summary>
        [Test]
        public void ParseTracks_TracksWithDashDelimited_Parsed()
        {
            var desc = "1.- 00:00-Salamander\n2.- 02:13-Mahou Hatsudou\n3.- 03:21-Dragon Force";
            var service = this.GetInstance();

            var tracks = service.ParseTracks(desc);

            Assert.AreEqual(3, tracks.Count);

            Assert.That(tracks.ContainsKey("Salamander"));
            Assert.That(tracks.ContainsKey("Mahou Hatsudou"));
            Assert.That(tracks.ContainsKey("Dragon Force"));

            Assert.AreEqual("00:00:00", tracks["Salamander"].ToString());
            Assert.AreEqual("00:02:13", tracks["Mahou Hatsudou"].ToString());
            Assert.AreEqual("00:03:21", tracks["Dragon Force"].ToString());
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>the instance.</returns>
        private DescriptionParser GetInstance()
        {
            return new DescriptionParser(@"(\d\d:\d\d)(\s|-)(.+)", @"mm\:ss");
        }
    }
}