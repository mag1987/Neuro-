using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace WordInteraction
{
    public class RegexFormat
    {
        public Regex Regex { get; set; }
        public List<(string groupName, int bold, int italic)> GroupsFormat { get; set; }

        public RegexFormat()
        {
            Regex = new Regex("");
            GroupsFormat = new List<(string groupName, int bold, int italic)>();
        }
        public RegexFormat(string regex, params (string groupName, int bold, int italic)[] formatString)
        {
            Regex = new Regex(regex);
            GroupsFormat = new List<(string groupName, int bold, int italic)>();
            foreach (var item in formatString)
            {
                GroupsFormat.Add(item);
            }
        }
    }

    public class RegexFormat<T> where T : FormattedPart
    {
        public Regex Regex { get; set; }
        public List<T> GroupsFormat { get; set; }

        public RegexFormat()
        {
            Regex = new Regex("");
            GroupsFormat = new List<T>();
        }
        public RegexFormat(string regex, params T[] formatString)
        {
            Regex = new Regex(regex);
            GroupsFormat = new List<T>();
            foreach (var item in formatString)
            {
                GroupsFormat.Add(item);
            }
        }
    }
}
