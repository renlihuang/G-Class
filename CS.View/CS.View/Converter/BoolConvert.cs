using System;
using System.Globalization;
using System.Windows.Data;

namespace CS.View.Convert
{
    internal class BoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (int.TryParse(value.ToString(), out int ivalue))
            {
                result = ivalue == 1 ? "是" : "否";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}