using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Reflection;

namespace UIChemShift
{
    class ViewModel : BindableBase
    {
        private Model _model = new Model();
        public ObservableCollection<ChemShift> ChemShifts => _model.ChemShifts;
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
        public ObservableCollection<(string a, int b, double c)> testTuples
        {
            get
            {
                ObservableCollection<(string a, int b, double c)> output = new ObservableCollection<(string a, int b, double c)>
                {
                    ("www",2,3.0),
                    ("asa", 3, 5.0),
                    ("sss", 4, 4.5)
                };
                //output.First().a;
                return output;
            }
        }
        public ObservableCollection<SomeClass> testCollection
        {
            get
            {
                ObservableCollection<SomeClass> output = new ObservableCollection<SomeClass>
                {
                    new SomeClass
                    {
                        a =12,
                        s ="er",
                        list =
                        {
                            "string 1",
                            "string 2"
                        },
                        tuple =
                        {
                            (a: "ww", b : 23),
                            (a:"aa",b:1)
                        }
                    },
                    new SomeClass
                    {
                        a =32,
                        s ="rtet",
                        list =
                        {
                            "string 3",
                            "string 4"
                        },
                        tuple =
                        {
                            (a: "ww", b : 23),
                            (a:"aa",b:1)
                        }
                    }
                };
                //output.First().a;
                return output;
            }
        }
        public DelegateCommand<DataGrid> GetDataACD { get; }
        public DelegateCommand<DataGrid> SaveData { get; }
        public DelegateCommand<DataGrid> LoadData { get; }
        public DelegateCommand<DataGrid> TestMethod { get; }
        public ViewModel()
        {
            //_model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            GetDataACD = new DelegateCommand<DataGrid>(dg => {
                _model.TestMethod();
            });
            SaveData = new DelegateCommand<DataGrid>(dg => {
                _model.SaveDataFileDialog();
            });
            LoadData = new DelegateCommand<DataGrid>(dg => {
                _model.LoadDataFileDialog();
                RaisePropertyChanged("ChemShifts");
            });

            TestMethod = new DelegateCommand<DataGrid>(dg => {
                ChemShift cs = new ChemShift();
                string str = "";
                foreach (var item in cs.GetType().GetProperties())
                {
                    str += item.Name + " ";
                }
                MessageBox.Show(str);
            });
        }
    }
    public class SomeClass
    {
        public int a { get; set; }
        public string s { get; set; }
        public List<string> list { get; set; }
        public List<(string a, int b)> tuple { get; set; }
        public List<Properties> properties
        {
            get;set;
        }
        public SomeClass()
        {
            a = 0;
            s = "";
            list = new List<string>();
            tuple = new List<(string a, int b)>();
            properties = new List<Properties>()
            {
                new Properties()
                { a="rrr", b=2},
                new Properties() { a= "ttt", b=44}
            };
        }
        public class Properties
        {
            public string a { get; set; }
            public int b { get; set; }
        }
    }
}
