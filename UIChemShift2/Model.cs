using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WindowTesting;
using WordInteraction;

namespace UIChemShift2
{
    public class Model : BindableBase
    {
        private ChemShiftProvider _provider;

        public ObservableCollection<ChemShift> ChemShifts { get; set; }
        public Format Format { get; set; }

        private FormattedStrings _formattedValues;
        private FormattedStrings _formattedAssignments;
        
        public RegexFormatObservable defaultFormat = new RegexFormatObservable
        {
            Regex = new Regex(@"(?<entier>\d+)\.(?<fraction>\d+)"),
            GroupsFormat = new ObservableCollection<FormattedPart>()
            {
                new FormattedPart(){ GroupName = "entier" , Bold = true, Italic = true},
                new FormattedPart(){ GroupName = "." , Bold = false, Italic = false},
                new FormattedPart(){ GroupName = "fraction" , Bold = false, Italic = true},
            }
        };
        public Model()
        {
            _provider = new ChemShiftProvider();
            ChemShifts = new ObservableCollection<ChemShift>();
            Format = new Format(defaultFormat,defaultFormat);
        }
        public void GetChemShifts()
        {
            ChemShifts.Clear();
            foreach (var shift in _provider.GetChemShiftsACD())
            {
                ChemShifts.Add(
                    new ChemShift(shift)
                    );
            }
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
        public void SaveFormatFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Format));
            Stream stream;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream = saveFileDialog.OpenFile();
                if (stream != null)
                {
                    xmlSerializer.Serialize(stream, Format);
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
        public ChemShift(string value) : this(value, assignment:"")
        { }
        public ChemShift(string value, string assignment)
        {
            Value = value;
            Assignment = assignment;
        }
    }
}