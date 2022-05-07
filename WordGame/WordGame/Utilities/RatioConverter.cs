using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WordGame.Utilities
{
    /// <summary>
    /// This is a ratio converter used to scale our UI elements to be a portion of some value and return that value
    /// with a type that can be consumed by xaml control elements. It is used in our application to scale individual
    /// WPF controls to a portion of the user's screen size.
    /// </summary>
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
