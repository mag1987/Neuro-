﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddNewItemOnClick(object sender, RoutedEventArgs e)
        {
            var t = (ObservableCollection<FormattedPart>)FormattingDataGrid.DataContext;
            t.Add(new FormattedPart("another", true, false));
            FormattingDataGrid.DataContext = t;
            ((ObservableCollection<FormattedPart>)FormattingDataGrid.DataContext).Add(
                new FormattedPart("noch einmal", false, true)
                );
        }
    }
}
