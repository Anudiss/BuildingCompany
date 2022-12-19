using BuildingCompany.Permissions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace BuildingCompany.Converters
{
    public class PermissionToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object obj in values)
            {
                if (obj is Permission permission && !UserData.UserData.Instance.User.HasPermission(permission))
                    return Visibility.Collapsed;
                if (obj is bool b && !b)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        private bool HasPermission(IEnumerable<Permission> permissions) =>
            permissions.All(permission => UserData.UserData.Instance.User.HasPermission(permission));

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;
    }
}
