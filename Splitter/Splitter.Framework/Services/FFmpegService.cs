namespace Splitter.Framework
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc />
    public class FFmpegService : IFFmpegService
    {
        /// <summary>
        /// The ffmpeg executable file location.
        /// </summary>
        private readonly string ffmpegLocation;

        /// <summary>
        /// The timeout time to wait for the ffmpeg process to complete.
        /// </summary>
        private readonly int processWaitBeforeTimeout;

        /// <summary>
        /// Flag indicating if the ffmpeg output should be verbose or not.
        /// </summary>
        private readonly bool verbose;

        /// <summary>
        /// Initialises a new instance of the <see cref="FFmpegService" /> class.
        /// </summary>
        /// <param name="ffmpegLocation">injected ffmpeg location</param>
        /// <param name="processWaitBeforeTimeoutMs">injected time to wait before timeout.</param>
        /// <param name="verbose">verbosity flag</param>
        public FFmpegService(string ffmpegLocation, int processWaitBeforeTimeoutMs = 30_000, bool verbose = false)
        {
            this.ffmpegLocation = ffmpegLocation;
            this.processWaitBeforeTimeout = processWaitBeforeTimeoutMs;
            this.verbose = verbose;
        }

        /// <inheritdoc />
        public void Slice(string inputFile, int start, int length, string outputFile)
        {
            string logLevel = "panic";
            if (this.verbose)
            {
                logLevel = "info";
            }

            var process = new Process();
            process.StartInfo.FileName = this.ffmpegLocation;
            process.StartInfo.Arguments = $"-loglevel {logLevel} -hide_banner -y -i {inputFile} -ss {start} -t {length} -strict -2 {outputFile}";
            process.Start();

            process.WaitForExit(this.processWaitBeforeTimeout);

            if (process.ExitCode != 0)
            {
                throw new Exception($"{this.ffmpegLocation} process completed with exit code {process.ExitCode}");
            }
        }
    }
}