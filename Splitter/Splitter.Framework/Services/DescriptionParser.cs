namespace Splitter.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <inheritdoc />
    public class DescriptionParser : IDescriptionParser
    {
        /// <summary>
        /// Track Regular Expression
        /// </summary>
        private Regex trackRegularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionParser"/> class.
        /// </summary>
        /// <param name="descriptionRegex">Regular expression to parse the description.</param>
        public DescriptionParser(string descriptionRegex)
        {
            this.trackRegularExpression = new Regex(descriptionRegex, RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public IDictionary<string, string> ParseTracks(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException($"{nameof(description)} was null or empty");
            }

            var matches = this.trackRegularExpression.Matches(description);

            if (matches.Count == 0)
            {
                throw new InvalidOperationException("No Track Matches found");
            }

            var mapping = new Dictionary<string, string>(matches.Count);

            foreach(Match currentMatch in matches)
            {
                mapping.Add(currentMatch.Groups[3].Value, currentMatch.Groups[1].Value);
            }

            return mapping;
        }
    }
}