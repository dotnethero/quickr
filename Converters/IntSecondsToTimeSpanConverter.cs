using System;
using System.Globalization;
using System.Windows.Data;

namespace Quickr.Converters
{
    internal class IntSecondsToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int seconds ? TimeSpan.FromSeconds(seconds) : default(TimeSpan?);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TimeSpan ts ? (int)ts.TotalSeconds : default(int?);
        }
    }
}