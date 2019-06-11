namespace WordInteraction
{
    public class FormattedString
    {
        public string String { get; set; }
        public RegexFormat<FormattedPart> Format { get; set; }

        public FormattedString()
        {
            String = "";
            Format = new RegexFormat<FormattedPart>();
        }
        public FormattedString(string aString, RegexFormat<FormattedPart> format)
        {
            String = aString;
            Format = format;
        }
    }
}
