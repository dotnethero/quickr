using System;
using System.Globalization;
using System.Windows.Data;
using StackExchange.Redis;

namespace Quickr.Converters
{
    internal class SlowlogArgumentsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is RedisValue[] arguments ? string.Join(" ", arguments) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}