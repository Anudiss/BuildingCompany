using BuildingCompany.Connection;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildingCompany.Converters
{
    public class SystemImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            SystemImage.GetImageByName((parameter as string).Trim().ToLower());

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
