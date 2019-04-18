using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Text.RegularExpressions;

using WordInteraction;

namespace WindowTesting
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        // parameter used for a pointer to list
        // True to continue enumerating, false to bail

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr parent, EnumWindowProc callback, IntPtr i);
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }
        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);
        public static void PrintList(List<IntPtr> children)
        {
            foreach (var child in children)
            {
                Console.WriteLine("Элемент {0}", child.ToString());
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            List<string> chemShifts = GetChemShiftsACD();
            PrintArray(chemShifts.ToArray());
            // --------------------- ChemShifts obtained --------------
            List<string> annotations = new List<string>();
            annotations = NewProperty(chemShifts);
            PrintArray(annotations.ToArray());

            //--------------- Proceeding with regex -------
            
            Console.Write("\nThe string of regex: ");
            string regexp = Console.ReadLine();
            Console.Write("\nThe string for replacemrnt to regex: ");
            string replacement = Console.ReadLine();
            
            ProcessProperty(chemShifts,regexp,replacement);
            PrintArray(chemShifts.ToArray());

            //------------------- Formatting to Word in cursor position ---------------
            RegexFormat numberFormat = new RegexFormat(
                @"(?<entier>\d+)\.(?<fraction>\d+)",
                ("entier", 1, 1),
                (".", 0, 0),
                ("fraction", 0, 1)
                );
            RegexFormat annotationFormat = new RegexFormat(
                @"(?<letter>[a-zA-Z]+)(?<number>\d+)",
                (" (", 0, 0),
                ("letter", 1, 1),
                ("number", 0, 1),
                ("), ", 0,0)
                );
            PrintToWord ToWord = new PrintToWord();
            ToWord.AtCursor(
                (chemShifts, numberFormat),
                (annotations, annotationFormat)
                );

            /*
            PrintList(GetChildWindows(ip));
            */
        }
        public static void PrintArray(object[] objects)
        {
            foreach (var obj in objects)
            {
                Console.WriteLine(" {0}", (string)obj);
            }
        }
        public static List<string> NewProperty(List<string> input)
        {
            List<string> output = new List<string>();
            foreach (var inputItem in input)
            {
                Console.Write("For [{0}] item, value \"{1}\" new property is: ", input.IndexOf(inputItem), inputItem);
                output.Add(Console.ReadLine());
            }
            return output;
        }
        public static void ProcessProperty(List<string> input, string regex, string pattern)
        {
            for(int i =0; i <input.Count; i++)
            {
                input[i] = Regex.Replace(input[i], regex, pattern);
            }
        }
        public static List<string> GetChemShiftsACD()
        {
            List<string> chemShifts = new List<string>();
            const uint WM_COMMAND = 0x0111;
            IntPtr ip = new IntPtr();
            ip = FindWindow(default(string), "Table of peaks");
            Console.WriteLine("Окно найдено {0}", ip.ToString());

            PostMessage(ip, WM_COMMAND, 0x3E8, 0);
            string s = Clipboard.GetText(TextDataFormat.Text);
            Console.WriteLine("s = {0}", s);

            var _singleLines = from item in Regex.Split(s, @"\n")
                               where String.IsNullOrWhiteSpace(item) == false
                               select item;
            string[] singleLines = _singleLines.ToArray();
            Console.WriteLine("Number of lines {0}", singleLines.Count());

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
                foreach (var item in wordsInLine)
                    Console.Write(" {0}", item);
                Console.Write(" --- Finally {0} elements\n", wordsInLine.Count());
                Console.WriteLine("ppm found at {0} position", indexOfShift);
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
