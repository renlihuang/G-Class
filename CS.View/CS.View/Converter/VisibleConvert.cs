using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CS.View.Convert
{
    internal class VisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            if (int.TryParse(value.ToString(), out int ivalue))
            {
                visibility = ivalue == 0 ? Visibility.Visible : Visibility.Hidden;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}