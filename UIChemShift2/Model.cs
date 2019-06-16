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
using System.Windows.Documents;
using System.Windows;

using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;

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
            foreach (var cs in ChemShifts)
            {
                printToWord.AtCursor(
                    new FormattedString(cs.Value,new RegexFormat<FormattedPart>()
                    {
                        Regex = Format.ValuesFormat.Regex,
                        GroupsFormat = Format.ValuesFormat.GroupsFormat.ToList()
                    }),
                    new FormattedString(cs.Assignment, new RegexFormat<FormattedPart>()
                    {
                        Regex = Format.AssignmentFormat.Regex,
                        GroupsFormat = Format.AssignmentFormat.GroupsFormat.ToList()
                    })
                    );
            }
        }
        public void SaveWord()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                word.Visible = false;
                word.Application.Documents.Add();
                PrintToWord printer = new PrintToWord(word);
                foreach (var cs in ChemShifts)
                {
                    printer.AtCursor(
                        new FormattedString(cs.Value, new RegexFormat<FormattedPart>()
                        {
                            Regex = Format.ValuesFormat.Regex,
                            GroupsFormat = Format.ValuesFormat.GroupsFormat.ToList()
                        }),
                        new FormattedString(cs.Assignment, new RegexFormat<FormattedPart>()
                        {
                            Regex = Format.AssignmentFormat.Regex,
                            GroupsFormat = Format.AssignmentFormat.GroupsFormat.ToList()
                        })
                        );
                }
                word.Application.ActiveDocument.SaveAs(saveFileDialog.FileName);
                word.Application.ActiveDocument.Close();
                word.Quit();
            }
        }
        public System.Windows.Documents.Paragraph Preview(params FormattedString[] formatStrings)
        {
            System.Windows.Documents.Paragraph p = new System.Windows.Documents.Paragraph();
            foreach (var itemInput in formatStrings)
            {
                Regex _regex = itemInput.Format.Regex;
                GroupCollection gc = _regex.Match(itemInput.String).Groups;
                var gcNames = _regex.GetGroupNames();
                foreach (var itemGroups in itemInput.Format.GroupsFormat)
                {
                    var isGroupName = from name in gcNames
                                      where name == itemGroups.GroupName
                                      select name;
                    if (isGroupName.Count() != 0)
                    {
                        Run run = new Run(gc[isGroupName.First()].Value);
                        run.FontStyle = itemGroups.Italic ? FontStyles.Italic : FontStyles.Normal;
                        run.FontWeight = itemGroups.Bold ? FontWeights.Bold : FontWeights.Normal;
                        p.Inlines.Add(run);
                    }
                    else
                    {
                        Run run = new Run(itemGroups.GroupName);
                        run.FontStyle = itemGroups.Italic ? FontStyles.Italic : FontStyles.Normal;
                        run.FontWeight = itemGroups.Bold ? FontWeights.Bold : FontWeights.Normal;
                        p.Inlines.Add(run);
                    }
                }
            }
            return p;
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