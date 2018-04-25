namespace Splitter.ConsoleApp
{
    using CommandLine;

    /// <summary>
    /// Command Line Arguments Option.
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Gets or sets the Youtube URL to process.
        /// </summary>
        /// <returns>the url.</returns>
        [Option(
            'u',
            "url",
            Required = true,
            HelpText = "YouTube URL to process.")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ffmpeg executable path. By default assmes the executable is in the user's path.
        /// </summary>
        /// <returns>the ffmpeg location.</returns>
        [Option(
            'f',
            "ffmpeg",
            Required = false,
            Default = "ffmpeg",
            HelpText = "Location of ffmpeg executable. Default assumes it is in your path."
        )]
        public string ffmpegLocation { get; set; }

        /// <summary>
        /// Gets or sets the ffmpeg timeout. Used to determine the maximum wait time on a ffmpeg sub process.
        /// </summary>
        /// <returns></returns>
        [Option(
            't',
            "ffmpeg-timeout",
            Required = false,
            Default = 30_000,
            HelpText = "Maximum wait time to wait for a ffmpeg sub process to complete."
        )]
        public int ffmpegTimeout { get; set; }

        /// <summary>
        /// Gets or sets whether to be Verbose or not.
        /// </summary>
        /// <returns>flag indicating to be verbose.</returns>
        [Option(
            'v',
            "verbose",
            Default = false,
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
    }
}