using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


namespace WordInteraction
{
    public class RegexFormatObservable 
    {
        private string _regexPattern;
        public string RegexPattern
        {
            get { return _regexPattern; }
            set
            {
                _regexPattern = value;
                Regex = new Regex(_regexPattern);
            }
        }
        public Regex Regex { get;  set; }
        public ObservableCollection<FormattedPart> GroupsFormat { get; set; }

        public RegexFormatObservable()
        {
            Regex = new Regex("");
            GroupsFormat = new ObservableCollection<FormattedPart>();
        }
        public RegexFormatObservable(string regex, params FormattedPart[] formatString)
        {
            RegexPattern = regex;
            GroupsFormat = new ObservableCollection<FormattedPart>();
            foreach (var item in formatString)
            {
                GroupsFormat.Add(item);
            }
        }
        public void Swap(int indexA, int indexB)
        {
            int max = GroupsFormat.Count;
            if ((0 <= indexA && indexA < max)
                && (0 <= indexB && indexB < max))
            {
                var t = GroupsFormat[indexA];
                GroupsFormat[indexA] = GroupsFormat[indexB];
                GroupsFormat[indexB] = t;
            }
        }
        public void Up(int index)
        {
            if (index > 0)
            {
                Swap(index, index - 1);
            }
        }
        public void Down(int index)
        {
            if (index < (GroupsFormat.Count - 1))
            {
                Swap(index, index + 1);
            }
        }
    }
}
