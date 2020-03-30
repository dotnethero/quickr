using System;
using System.Globalization;
using System.Windows.Data;

namespace Quickr.Converters
{
    internal class NullableTimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TimeSpan ts ? ts.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string text && TimeSpan.TryParse(text, out var ts) ? ts : default(TimeSpan?);
        }
    }
}