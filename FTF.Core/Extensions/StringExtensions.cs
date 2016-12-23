using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FTF.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex TagRegex = new Regex(@"#(\w*)");

        public static string[] ParseTagNames(this string text) => _ParseTagNames(text).Distinct().ToArray();

        private static IEnumerable<string> _ParseTagNames(string text)
        {
            var matches = TagRegex.Matches(text);

            for (var i = 0; i < matches.Count; i++)
                yield return matches[i].Groups[1].Value;
        }
    }
}