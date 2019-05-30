using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace WordInteraction
{
    public class PrintToWord
    {
        private Application app { get; set; }
        public PrintToWord()
        {
            app = (Application)Marshal.GetActiveObject("Word.Application");
        }
        public void AtCursor(params (string input, int bold, int italic)[] formatString)
        {
            foreach (var item in formatString)
            {
                Selection sel = app.Selection;
                sel.Font.Bold = item.bold;
                sel.Font.Italic = item.italic;
                sel.TypeText(item.input);

                Selection selClear = app.Selection;
                selClear.Font.Reset();
            }
        }
        public void AtCursor(string input, string regex, params (string groupName, int bold, int italic)[] formatString )
        {
            Regex _regex = new Regex(regex);
            GroupCollection gc = _regex.Match(input).Groups;
            var gcNames = _regex.GetGroupNames();

            foreach (var item in formatString)
            {
               // TO DO SMTH
            }
        }
        public void AtCursor(params (string input, RegexFormat format)[] formatStrings)
        {
            foreach (var itemInput in formatStrings)
            {
                Regex _regex = itemInput.format.Regex;
                GroupCollection gc = _regex.Match(itemInput.input).Groups;
                var gcNames = _regex.GetGroupNames();
                foreach (var itemGroups in itemInput.format.GroupsFormat)
                {
                    var isGroupName = from name in gcNames
                                      where name == itemGroups.groupName
                                      select name;
                    if (isGroupName.Count() != 0)
                    {
                        AtCursor((gc[isGroupName.First()].Value, itemGroups.bold, itemGroups.italic));
                    }
                    else
                    {
                        AtCursor((itemGroups.groupName, itemGroups.bold, itemGroups.italic));
                    }
                }
            }
        }
        public void AtCursor(params FormattedString[] formatStrings)
        {
            foreach (var itemInput in formatStrings)
            {
                Regex _regex = itemInput.Format.Regex;
                GroupCollection gc = _regex.Match(itemInput.String).Groups;
                var gcNames = _regex.GetGroupNames();
                foreach (var itemGroups in itemInput.Format.GroupsFormat)
                {
                    var isGroupName = from name in gcNames
                                      where name == itemGroups.GroupName
                                      select name;
                    if (isGroupName.Count() != 0)
                    {
                        AtCursor(
                            (gc[isGroupName.First()].Value, 
                            itemGroups.Bold? 1:0, 
                            itemGroups.Italic? 1:0)
                            );
                    }
                    else
                    {
                        AtCursor(
                            (itemGroups.GroupName,
                            itemGroups.Bold ? 1 : 0,
                            itemGroups.Italic ? 1 : 0)
                            );
                    }
                }
            }
        }
        public void AtCursor(params(List<string> input, RegexFormat format)[] formatStrings)
        {
            int firstInputLength = formatStrings[0].input.Count;
            for (int i =0; i<firstInputLength; i++)
            {
                foreach (var itemInput in formatStrings)
                {
                    AtCursor((itemInput.input[i], itemInput.format));
                }
            }
        }
        public void AtCursor(params FormattedStrings[] formattedStrings)
        {
            List<string> input = new List<string>();
            RegexFormat<FormattedPart> format = new RegexFormat<FormattedPart>();
            foreach (var item in formattedStrings)
            {
                input = item.Strings;
                format = item.Format;
            }
            foreach (var item in input)
            {
                FormattedString formattedString = new FormattedString()
                {
                    String = item,
                    Format= format
                };
                AtCursor(formattedString);
            }
        }
    }
}
