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

namespace UIChemShift
{
    public class Model : BindableBase
    {
        private ChemShiftProvider _provider;
        private ObservableCollection<ChemShift> _chemShifts { get; set; }
        public ObservableCollection<ChemShift> ChemShifts { get; set; }
        public ObservableCollection<FormattedStrings> FormattedValues { get; set; }

        private FormattedStrings testFormatStrings = new FormattedStrings();
        public ObservableCollection<FormattedPart> FormattedParts
        {
            get
            {
                return new ObservableCollection<FormattedPart>(testFormatStrings.Format.GroupsFormat);
            }
            set
            {
                testFormatStrings.Format.GroupsFormat = value.ToList();
            }
        }

        public RegexFormat<FormattedPart> defaultFormat = new RegexFormat<FormattedPart>
        {
            Regex = new Regex(@"(?<entier>\d+)\.(?<fraction>\d+)"),
            GroupsFormat = new List<FormattedPart>()
            {
                new FormattedPart(){ GroupName = "entier" , Bold = true, Italic = true},
                new FormattedPart(){ GroupName = "." , Bold = false, Italic = false},
                new FormattedPart(){ GroupName = "fraction" , Bold = false, Italic = true},
            }
        };
        /*
        public void TestMethod()
        {
            MessageBox.Show("Hello");
        }
        */
        public Model()
        {
            _provider = new ChemShiftProvider();
            _chemShifts = new ObservableCollection<ChemShift>();
            ChemShifts = new ObservableCollection<ChemShift>();
            FormattedValues = new ObservableCollection<FormattedStrings>();

            testFormatStrings.Format = defaultFormat;

            /*
            ChemShifts = new ObservableCollection<ChemShift>()
            {
                new ChemShift("1.23","C1" ),
                new ChemShift("2.56","C43" ),
                new ChemShift("0.54","C6" )
            };
            */
        }
        public void GenerateForamttedValues()
        {
            List<string> sValues = new List<string>();
            foreach (var item in ChemShifts)
            {
                sValues.Add(item.Value);
            }
            FormattedValues[0].Format = defaultFormat;
            FormattedValues[0].Strings = sValues;
            RaisePropertyChanged("FormattedValues");
        }
        public void UpdateChemShifts()
        {
            /*
            if (_chemShifts != null)
            {
                _chemShifts.Clear();
            }
            */
            _chemShifts.Clear();
            foreach (var shift in _provider.GetChemShiftsACD())
            {
                _chemShifts.Add(
                    new ChemShift(shift)
                    );
            }
            ChemShifts = _chemShifts;
            RaisePropertyChanged("ChemShifts");
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
        public ChemShift(string value) : this(value, assignment:"")
        { }
        public ChemShift(string value, string assignment)
        {
            Value = value;
            Assignment = assignment;
        }
    }
}