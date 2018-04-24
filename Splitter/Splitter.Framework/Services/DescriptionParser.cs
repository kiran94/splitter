namespace Splitter.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
          /// <param name="timeSpanFormat">Format of timestamps.</param>
        public DescriptionParser(string descriptionRegex)
        {
            this.trackRegularExpression = new Regex(descriptionRegex, RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public IDictionary<string, TimeSpan> ParseTracks(string description)
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

            var mapping = new Dictionary<string, TimeSpan>(matches.Count);
            string timeSpanFormat = @"mm\:ss";

            foreach (Match currentMatch in matches)
            {
                var rawTimestamp = currentMatch.Groups["time"].Value;

                if (rawTimestamp.Length == 5)
                {
                    timeSpanFormat = @"mm\:ss";
                }
                else if (rawTimestamp.Length == 7)
                {
                    timeSpanFormat = @"h\:mm\:ss";
                }

                if (!TimeSpan.TryParseExact(rawTimestamp, timeSpanFormat, CultureInfo.CurrentCulture, TimeSpanStyles.None, out var currentTrack))
                {
                    throw new Exception("Could not parse timestamp: " + rawTimestamp);
                }

                // Track Name -> Track Timestamp
                mapping.Add(currentMatch.Groups["title"].Value, currentTrack);
            }

            return mapping;
        }
    }
}