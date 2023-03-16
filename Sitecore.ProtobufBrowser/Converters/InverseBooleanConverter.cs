using System;
using System.Globalization;
using System.Windows.Data;

namespace Sitecore.ProtobufBrowser.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value is null) return true;

            if (bool.TryParse(value.ToString(), out var result)) return !result;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        #endregion
    }
}