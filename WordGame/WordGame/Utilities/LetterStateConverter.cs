using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

using WordGame.Objects;

namespace WordGame
{
    public class LetterStateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = values[0] as LetterStateCollection;
            int idx = System.Convert.ToInt32(values[1]);
            Trace.WriteLine("idx: " + idx + " collection item: " + collection?[idx]);
            return collection?[idx];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
