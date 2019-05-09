using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Mvvm;
//using System.Windows;

using Prism.Ioc;
using Prism.Modularity;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

using System.Windows.Forms;

namespace UIChemShift
{
    public class Model : BindableBase
    {
        public ObservableCollection<ChemShift> ChemShifts { get; set; }
        public void TestMethod()
        {
            MessageBox.Show("Hello");
        }
        public Model()
        {
            ChemShifts = new ObservableCollection<ChemShift>()
            {
                new ChemShift("1.23","C1" ),
                new ChemShift("2.56","C43" ),
                new ChemShift("0.54","C6" )
            };
        }
        public void SaveData()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<ChemShift>));
            FileStream fileStream = new FileStream("TestXML.xml",FileMode.Create);
            xmlSerializer.Serialize(fileStream, ChemShifts);
            fileStream.Close();
        }
        public void SaveDataFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<ChemShift>));
            Stream stream;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream = saveFileDialog.OpenFile();
                if (stream != null)
                {
                    xmlSerializer.Serialize(stream, ChemShifts);
                    stream.Close();
                }
            }
        }
        public void LoadDataFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<ChemShift>));
            Stream stream;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream = openFileDialog.OpenFile();
                if (stream != null)
                {
                    ChemShifts = (ObservableCollection<ChemShift>)xmlSerializer.Deserialize(stream);
                    stream.Close();
                }
            }
        }
    }
    public class ChemShift
    {
        public string Value { get; set; }
        public string Assignment { get; set; }

        public ChemShift() : this ("0.0","")
        { }
        public ChemShift(string value, string assignment)
        {
            Value = value;
            Assignment = assignment;
        }
    }
}