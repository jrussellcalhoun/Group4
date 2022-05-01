using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
//using System.Windows.Markup;

namespace WordGame
{
    public class IntToBoolStateConverter : IMultiValueConverter
    {
        //private static IntToBoolStateConverter _instance;

        //public IntToBoolStateConverter() { }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<int> collection = values[0] as ObservableCollection<int>;
            int idx = System.Convert.ToInt32(values[1]);
            bool conversion = (collection?[idx] == 0);
            return conversion;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public override object ProvideValue(IServiceProvider serviceProvider) => _instance ??= new IntToBoolStateConverter();
    }
}