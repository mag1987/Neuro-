using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
        public static void PrintList(List<IntPtr> children)
        {
            foreach (var child in children)
            {
                Console.WriteLine("Элемент {0}", child.ToString());
            }
        }
        
        static void Main(string[] args)
        {
            IntPtr ip = new IntPtr();
            ip = FindWindow(default(string), "ACD/ChemSketch - [noname01.sk2]");
            Console.WriteLine("Окно найдено {0}",ip.ToString());
            PrintList(GetChildWindows(ip));
        }

    }
}
