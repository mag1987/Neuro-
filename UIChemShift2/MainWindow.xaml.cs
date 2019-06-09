using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordInteraction;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace UIChemShift2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            InitializeComponent();

            _model = new Model();
            ChemShiftsDataGrid.DataContext = _model.ChemShifts;
            ChemShiftsDataGrid.ItemsSource = _model.ChemShifts;
            //FormattingDataGrid.DataContext = _model.Format;
            FormattingDataGrid.ItemsSource = _model.Format.ValuesFormat.GroupsFormat;
            RegexTextBox.Text = _model.Format.ValuesFormat.RegexPattern;
            PropertyComboBox.ItemsSource = _model.ChemShiftProperties;

            GetDataButton.Click += GetDataOnClick;
            SaveDataButton.Click += SaveDataOnClick;
            LoadDataButton.Click += LoadDataButton_Click;
            SaveFormatButton.Click += SaveFormatOnClick;
            LoadFormatButton.Click += (s, e) => 
            {
                Type t = _model.Format.GetType();
                _model.Format = (Format)_model.LoadFileDialog(t);
                RegexTextBox.Text = _model.Format.ValuesFormat.RegexPattern;
            };
            RegexTextBox.LostFocus += (s,e) => 
            {
                _model.Format.ValuesFormat.RegexPattern = RegexTextBox.Text;
            };
            PropertyComboBox.SelectionChanged += (s, e) => 
            {
                switch (PropertyComboBox.SelectedItem)
                {
                    case "Value":
                        FormattingDataGrid.ItemsSource = _model.Format.ValuesFormat.GroupsFormat;
                        RegexTextBox.Text = _model.Format.ValuesFormat.RegexPattern;
                        break;
                    case "Assignment":
                        FormattingDataGrid.ItemsSource = _model.Format.AssignmentFormat.GroupsFormat;
                        RegexTextBox.Text = _model.Format.AssignmentFormat.RegexPattern;
                        break;
                }
            };
            InsertButton.Click += (s, e) => 
            {
                _model.InsertToWord();
            };
        }

        private void SaveFormatOnClick(object sender, RoutedEventArgs e)
        {
            _model.SaveFileDialog(_model.Format);
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            Type t = _model.ChemShifts.GetType();
            _model.ChemShifts = (ObservableCollection<ChemShift>)_model.LoadFileDialog(t);
            ChemShiftsDataGrid.ItemsSource = _model.ChemShifts;
        }

        private void GetDataOnClick(object sender, RoutedEventArgs e)
        {
            _model.GetChemShifts();
        }
        private void SaveDataOnClick(object sender, RoutedEventArgs e)
        {
            _model.SaveFileDialog(_model.ChemShifts);
        }
    }
}
