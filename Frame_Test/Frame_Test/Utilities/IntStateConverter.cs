using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
//using System.Windows.Markup;

namespace WordGame
{
    public class IntStateConverter : IMultiValueConverter
    {
        //private static IntStateConverter _instance;

        //public IntStateConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<int> collection = values[0] as ObservableCollection<int>;
            //Trace.Assert(collection != null);
            int idx = System.Convert.ToInt32(values[1]);
            Trace.WriteLine("idx: " + idx + " collection item: " + collection?[idx]);
            return collection?[idx];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public override object ProvideValue(IServiceProvider serviceProvider) => _instance ??= new IntStateConverter();
    }
}
