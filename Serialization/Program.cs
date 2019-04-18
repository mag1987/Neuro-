using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using WordInteraction;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list1 = new List<string> {"12","34" };
            List<string> list2 = new List<string> { "56", "789" };
            List<string> list3 = new List<string> { "aaa", "sss" };
            int n = 22;
            TestClass tc1 = new TestClass(list1, list2, n, new InnerClass(list3));

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TestClass));
            StreamWriter streamWriter = new StreamWriter("testFile.xml");

            tc1.List1[1] = "ttte";
            xmlSerializer.Serialize(streamWriter, tc1);
            streamWriter.Close();

            FileStream fileStream = new FileStream("testFile.xml", FileMode.Open);
            TestClass tc2 = (TestClass)xmlSerializer.Deserialize(fileStream);

            Console.WriteLine(tc2.Number);
            fileStream.Close();
        }
    }
    public class InnerClass
    {
        public List<string> InnerList { get; set; }
        public InnerClass()
        {
            InnerList = new List<string>();
        }
        public InnerClass(List<string> strings)
        {
            InnerList = new List<string>(strings);
        }
        public RegexFormat InnerRegex { get; set; }
    }
    public class TestClass
    {
        public List<string> List1 { get; set; }
        public List<string> List2 { get; set; }
        public int Number { get; set; }
        public List<InnerClass> Inner { get; set; }

        private int SecretNumber { get; set; }

        public TestClass()
        {
            List1 = new List<string>();
            List2 = new List<string>();
            Number = 0;
            SecretNumber = 44;
            Inner = new List<InnerClass>();
        }
        public TestClass(List<string> list1, List<string> list2, int num, InnerClass inner)
        {
            List1 = new List<string>(list1);
            List2 = new List<string>(list2);
            Number = num;
            SecretNumber = 44;
            Inner = new List<InnerClass>();
            Inner.Add(inner);
            Inner.First().InnerRegex = new RegexFormat("test", ("pattern", 1, 1));
        }
    }
}
