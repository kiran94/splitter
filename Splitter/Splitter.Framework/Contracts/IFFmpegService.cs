namespace Splitter.Framework
{
    /// <summary>
    /// Defines contract for ffmpeg operations.
    /// </summary>
    public interface IFFmpegService
    {
        /// <summary>
        /// Slices the input file according the to start and length.
        /// </summary>
        /// <param name="inputFile">input file to slice.</param>
        /// <param name="start">starting position in seconds.</param>
        /// <param name="length">length to slice from the starting position in seconds.</param>
        /// <param name="outputFile">output file to write too.</param>
        void Slice(string inputFile, int start, int length, string outputFile);
    }
}