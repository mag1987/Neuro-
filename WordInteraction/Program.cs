using System;
using System.Collections.Generic;
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
            ToWord.AtCursor(
                new FormattedStrings()
                {
                    Strings = new List<string>() {"111.133" },
                    Format = new RegexFormat<FormattedPart>()
                    {
                        Regex = new Regex(@"(?<entier>\d+)\.(?<fraction>\d+)"),
                        GroupsFormat = new List<FormattedPart>()
                        {
                            new FormattedPart(){ GroupName = "entier" , Bold = true, Italic = true},
                            new FormattedPart(){ GroupName = "--" , Bold = false, Italic = false},
                            new FormattedPart(){ GroupName = "++" , Bold = true, Italic = true},
                            new FormattedPart(){ GroupName = "fraction" , Bold = false, Italic = false}
                        }
                    }
                });
        }
        public static void TestMethod(GroupCollection gc, string str)
        {
            Console.WriteLine(gc[str].Value);
        }
    }
}
