using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using Serialization;

using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

using System.Data;


namespace WpfTesting
{
    class Model
    {
        List<string> list1 = new List<string> { "12", "34" };
        List<string> list2 = new List<string> { "56", "789" };
        List<string> list3 = new List<string> { "aaa", "sss" };
        int n = 22;
        public TestClass tc;
        public Binding dataGridBinding;
        public Model()
        {
            tc = new TestClass(list1, list2, n, new InnerClass(list3));
            dataGridBinding = new Binding();
            dataGridBinding.Source = tc;
            GetDataConverter getDataConverter = new GetDataConverter();
            dataGridBinding.Converter = getDataConverter;
        }
        public void GetData(DataGrid dataGrid)
        {
            //dataGrid.ItemsSource = tc.Inner; // -- не умеет DataGrid отображать списки

        }
    }
    public class GetDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            TestClass tc = (TestClass)value;
            var row = dt.NewRow();
            row.ItemArray = new string[]
            {
                //tc.List1[0], tc.List2[0], tc.Number.ToString()
                "12"
            };
            dt.Rows.Add(row);
            return ds;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
