using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowTesting
{
    public class ChemShiftProvider
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);
        public List<string> GetChemShiftsACD()
        {
            List<string> chemShifts = new List<string>();
            const uint WM_COMMAND = 0x0111;
            IntPtr ip = new IntPtr();
            ip = FindWindow(default(string), "Table of peaks");
            //Console.WriteLine("Окно найдено {0}", ip.ToString());

            PostMessage(ip, WM_COMMAND, 0x3E8, 0);
            string s = Clipboard.GetText(TextDataFormat.Text);
            //Console.WriteLine("s = {0}", s);

            var _singleLines = from item in Regex.Split(s, @"\n")
                               where String.IsNullOrWhiteSpace(item) == false
                               select item;
            string[] singleLines = _singleLines.ToArray();
            //Console.WriteLine("Number of lines {0}", singleLines.Count());

            List<string[]> wordsInLines = new List<string[]>();
            int indexOfShift = -1;
            foreach (var singleLine in singleLines)
            {
                var _wordsInLine = from item in Regex.Split(singleLine, @"\s+")
                                   where String.IsNullOrWhiteSpace(item) == false
                                   select item;
                string[] wordsInLine = _wordsInLine.ToArray();
                wordsInLines.Add(wordsInLine);

                for (int i = 0; i < wordsInLine.Count(); i++)
                {
                    if (indexOfShift == -1 && wordsInLine[i].Contains("ppm"))
                    {
                        indexOfShift = i;
                    }
                }
                /*
                foreach (var item in wordsInLine)
                    Console.Write(" {0}", item);
                Console.Write(" --- Finally {0} elements\n", wordsInLine.Count());
                Console.WriteLine("ppm found at {0} position", indexOfShift);
                */
            }
            foreach (var wordsInLine in wordsInLines)
            {
                if (wordsInLine[indexOfShift].Contains("ppm") == false)
                {
                    chemShifts.Add(wordsInLine[indexOfShift]);
                }
            }
            return chemShifts;
        }
    }
}
