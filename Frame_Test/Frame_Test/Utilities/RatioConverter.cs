using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Word_Game.Tools
{
    // This is a helpful tool code snippet I found on stackoverflow to resolve the issue of being able to bind ratios of the given value.
    // This will ease the process of making controls relative to a given value by a certain ratio directly in the xaml markup code.
    [ValueConversion(typeof(string), typeof(string))]
    public class RatioConverter : MarkupExtension, IValueConverter
    {
        private static RatioConverter _instance;

        public RatioConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do not let the culture default to local to prevent variable outcome re decimal syntax
            double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // read only converter...
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Use of null-coalescing assignment operator.
            // This returns the left hand side if it isn't null. If it's null it assigns the value to the right hand side and then returns the value.
            return _instance ??= new RatioConverter();
        }
    }
}
