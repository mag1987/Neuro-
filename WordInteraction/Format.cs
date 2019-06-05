namespace WordInteraction
{
    public class Format
    {
        public RegexFormatObservable ValuesFormat { get; set; }
        public RegexFormatObservable AssignmentFormat { get; set; }
        public Format()
        {
            ValuesFormat = new RegexFormatObservable();
            AssignmentFormat = new RegexFormatObservable();
        }
        public Format(RegexFormatObservable valuesFormat, RegexFormatObservable assignmentFormat)
        {
            ValuesFormat = valuesFormat;
            AssignmentFormat = assignmentFormat;
        }
    }
}
