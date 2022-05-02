using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
//using System.Windows.Markup;

using WordGame.Objects;

namespace WordGame
{
    public class LetterStateToBoolStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = values[0] as LetterStateCollection;
            int idx = System.Convert.ToInt32(values[1]);
            var is_selected_zero_times = collection?[idx].Count != 0;
            Trace.WriteLine($"Letter {collection?[idx].Letter} is selected {collection?[idx].Count} so the result is {is_selected_zero_times}");
            return is_selected_zero_times;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}