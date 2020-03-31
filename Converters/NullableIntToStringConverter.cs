using System;
using System.Globalization;
using System.Windows.Data;

namespace Quickr.Converters
{
    internal class NullableIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int ts ? ts.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string text && int.TryParse(text, out var ts) ? ts : default(int?);
        }
    }
}