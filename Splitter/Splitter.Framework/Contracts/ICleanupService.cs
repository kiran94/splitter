namespace Splitter.Framework
{
    /// <summary>
    /// Defines Behaviour for cleaning up files.
    /// </summary>
    public interface ICleanupService
    {
        /// <summary>
        /// Cleans up temp files from metadata.
        /// </summary>
        /// <param name="metadata">metadata to find files to clean up.</param>
        void CleanUp(Metadata metadata);
    }
}