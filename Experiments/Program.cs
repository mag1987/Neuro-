using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIChemShift;

namespace Experiments
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMethod();
        }
        static public void TestMethod()
        {
            ChemShift cs = new ChemShift();
            foreach (var item in cs.GetType().GetProperties())
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine( cs.GetType().GetProperty(nameof(cs.Value)).Name);
        }
    }
}
