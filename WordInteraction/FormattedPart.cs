namespace WordInteraction
{
    public class FormattedPart
    {
        public string GroupName { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public FormattedPart()
        {
            GroupName = "";
            Bold = false;
            Italic = false;
        }
        public FormattedPart(string groupName, bool bold, bool italic)
        {
            GroupName = groupName;
            Bold = bold;
            Italic = italic;
        }
    }
}
