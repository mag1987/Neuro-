using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Text.RegularExpressions;

using WordInteraction;

namespace WindowTesting
{
    class Program
    {
        /*
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

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
            return true;
        }
        
        public static void PrintList(List<IntPtr> children)
        {
            foreach (var child in children)
            {
                Console.WriteLine("Элемент {0}", child.ToString());
            }
        }
        */
        [STAThread]
        static void Main(string[] args)
        {
            var provider = new ChemShiftProvider();
            List<string> chemShifts = provider.GetChemShiftsACD();
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
        
    }
}
