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

        public ObservableCollection<string> ChemShiftProperties
        {
            get
            {
                ChemShift cs = new ChemShift();
                ObservableCollection<string> output = new ObservableCollection<string>();
                foreach (var item in cs.GetType().GetProperties())
                {
                    output.Add(item.Name);
                }
                return output;
            }
        }
        public ObservableCollection<ChemShift> ChemShifts { get; set; }
        public Format Format { get; set; }

        private FormattedStrings _formattedValues
        {
            get
            {
                List<string> strings = new List<string>();
                foreach (var item in ChemShifts)
                {
                    strings.Add(item.Value);
                }
                return new FormattedStrings(
                    strings,
                    new RegexFormat<FormattedPart>(
                        Format.ValuesFormat.RegexPattern,
                        Format.ValuesFormat.GroupsFormat.ToArray()
                        )
                    );
            }
        }
        private FormattedStrings _formattedAssignments
        {
            get
            {
                List<string> strings = new List<string>();
                foreach (var item in ChemShifts)
                {
                    strings.Add(item.Assignment);
                }
                return new FormattedStrings(
                    strings,
                    new RegexFormat<FormattedPart>(
                        Format.AssignmentFormat.RegexPattern,
                        Format.AssignmentFormat.GroupsFormat.ToArray()
                        )
                    );
            }
        }

        public RegexFormatObservable defaultFormatValue = new RegexFormatObservable
        {
            RegexPattern = @"(?<entier>\d+)\.(?<fraction>\d+)",
            GroupsFormat = new ObservableCollection<FormattedPart>()
            {
                new FormattedPart(){ GroupName = "entier" , Bold = false, Italic = false},
                new FormattedPart(){ GroupName = "." , Bold = false, Italic = false},
                new FormattedPart(){ GroupName = "fraction" , Bold = false, Italic = false},
            }
        };
        public RegexFormatObservable defaultFormatAssignment = new RegexFormatObservable
        {
            RegexPattern = @"(?<letters>\d+)(?<numbers>\d+)",
            GroupsFormat = new ObservableCollection<FormattedPart>()
            {
                new FormattedPart(){ GroupName = "letters" , Bold = false, Italic = true},
                new FormattedPart(){ GroupName = "numbers" , Bold = false, Italic = false},
            }
        };
        public Model()
        {
            _provider = new ChemShiftProvider();
            ChemShifts = new ObservableCollection<ChemShift>();
            Format = new Format(defaultFormatValue, defaultFormatAssignment);
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
        public void SaveFileDialog(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            Stream stream;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream = saveFileDialog.OpenFile();
                if (stream != null)
                {
                    xmlSerializer.Serialize(stream, obj);
                    stream.Close();
                }
            }
        }
        public object LoadFileDialog(Type type)
        {
            object obj = new object();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            Stream stream;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                stream = openFileDialog.OpenFile();
                if (stream != null)
                {
                    obj = xmlSerializer.Deserialize(stream);
                    stream.Close();
                }
            }
            return obj;
        }
        public void InsertToWord()
        {
            PrintToWord printToWord = new PrintToWord();
            printToWord.AtCursor(_formattedValues, _formattedAssignments);
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