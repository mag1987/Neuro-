using System.Collections.Generic;


namespace WordInteraction
{
    public class FormattedStrings
    {
        public List<string> Strings { get; set; }
        public RegexFormat<FormattedPart> Format { get; set; }
        public FormattedStrings()
        {
            Strings = new List<string>();
            Format = new RegexFormat<FormattedPart>();
        }
        public FormattedStrings(List<string> strings)
        {
            Strings = strings;
            Format = new RegexFormat<FormattedPart>();
        }
        public FormattedStrings(List<string> strings, RegexFormat<FormattedPart> format)
        {
            Strings = strings;
            Format = format;
        }
    }
}
