using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CS.View.Convert
{
    /// <summary>
    /// 可见类型转换器
    /// </summary>
    class VisibilityConvertcs : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;


            if (value != null && bool.TryParse(value.ToString(), out bool result))
            {
                visibility = result == false ? Visibility.Visible : Visibility.Hidden;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
