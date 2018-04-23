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
        /// Initialises a new instance of the <see cref="FFmpegService" /> class.
        /// </summary>
        /// <param name="ffmpegLocation">injected ffmpeg location</param>
        /// <param name="processWaitBeforeTimeout">injected time to wait before timeout.</param>
        public FFmpegService(string ffmpegLocation, int processWaitBeforeTimeout)
        {
            this.ffmpegLocation = ffmpegLocation;
            this.processWaitBeforeTimeout = processWaitBeforeTimeout;
        }

        /// <inheritdoc />
        public void Slice(string inputFile, int start, int length, string outputFile)
        {
            var process = new Process();
            process.StartInfo.FileName = this.ffmpegLocation;
            process.StartInfo.Arguments = $"-y -i {inputFile} -ss {start} -t {length} -strict -2 {outputFile}";
            process.Start();

            process.WaitForExit(30_000);

            if (process.ExitCode != 0)
            {
                throw new Exception($"{this.ffmpegLocation} process completed with exit code {process.ExitCode}");
            }
        }
    }
}