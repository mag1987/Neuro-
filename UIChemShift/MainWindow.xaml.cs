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
using System.Windows.Forms;

namespace UIChemShift
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Button1.Click += (s,e)=>
            {
                HelpWindow help = new HelpWindow();
                help.Show();
                Button1.IsEnabled = false;
                help.Closing += (se, ev) =>
                {
                    Button1.IsEnabled = true;
                };
            };
        }
        private void AddNewItemOnClick(object sender, RoutedEventArgs e)
        {
            var t = (ObservableCollection<FormattedPart>)FormattingDataGrid.DataContext;
            t.Add(new FormattedPart("another",true,false));
            FormattingDataGrid.DataContext = t;
            ((ObservableCollection<FormattedPart>)FormattingDataGrid.DataContext).Add(
                new FormattedPart("noch einmal",false,true)
                );
        }
        
    }
}
