namespace Splitter.Framework
{
    /// <summary>
    /// Defines behaviour for interacting with Youtube.
    /// </summary>
    public interface IYoutubeRepository
    {
        /// <summary>
        /// Gets the Description of a Youtube Video.
        /// </summary>
        /// <param name="url">URL of the youtube video to retrieve</param>
        /// <returns>string representation of the description.</returns>
        string GetDescription(string url);
    }
}