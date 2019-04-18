using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;


namespace WordInteraction
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Range - doesn't move cursor. If many go in backward order
             * 
             * Selection - puts text and cursor is moved to the end of the text,
             *    but need Font.Reset() to clear all formatting
             * 
             * Works OK with 1 Word opened, insertion apears in focused document
             *    (if many)
             */

            Application app = (Application)Marshal.GetActiveObject("Word.Application");

            Selection sel1 = app.Selection;
            sel1.Font.Bold = 1;
            sel1.Font.Italic = 1;
            sel1.TypeText("Text1");

            Selection selClear = app.Selection;
            selClear.Font.Reset();

            Selection sel2 = app.Selection;
            sel2.Font.Bold = 0;
            sel2.TypeText("Text2");

            string testString = "18.36";
            Regex regex = new Regex(@"(?<entier>\d+)\.(?<fraction>\d+)");
            Match match = regex.Match(testString);
            GroupCollection groupCollection = match.Groups;

            PrintToWord ToWord = new PrintToWord();
            ToWord.AtCursor(
                ("(", 0, 0),
                (groupCollection["entier"].Value, 1, 1),
                (".", 0, 0),
                (groupCollection["fraction"].Value, 0, 1),
                (")", 0, 0)
                );
            RegexFormat regForm = new RegexFormat(
                @"(?<entier>\d+)\.(?<fraction>\d+)",
                ("(", 0, 0),
                ("entier", 1, 1),
                (".", 0, 0),
                ("fraction", 0, 1),
                (")", 0, 0)
                );
            ToWord.AtCursor(
                (testString, regForm),
                ("50.211", regForm)
                );
            List<string> list1 = new List<string> {"12.12","34.12" };
            List<string> list2 = new List<string> { "56.66", "78.66" };
            ToWord.AtCursor(
                (list1, regForm),
                (list2, regForm)
                );
        }
        public static void TestMethod(GroupCollection gc, string str)
        {
            Console.WriteLine(gc[str].Value);
        }
    }
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
    }

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
}
