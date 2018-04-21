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
        public DescriptionParser()
        {
            this.trackRegularExpression = new Regex(@"(\d\d:\d\d)(\s|-)(.+)", RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public IDictionary<string, string> ParseTracks(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description was null or empty");
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