using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace WordInteraction
{
    public class RegexFormatObservable 
    {
        public Regex Regex { get; set; }
        public ObservableCollection<FormattedPart> GroupsFormat { get; set; }

        public RegexFormatObservable()
        {
            Regex = new Regex("");
            GroupsFormat = new ObservableCollection<FormattedPart>();
        }
        public RegexFormatObservable(string regex, params FormattedPart[] formatString)
        {
            Regex = new Regex(regex);
            GroupsFormat = new ObservableCollection<FormattedPart>();
            foreach (var item in formatString)
            {
                GroupsFormat.Add(item);
            }
        }
    }
}
