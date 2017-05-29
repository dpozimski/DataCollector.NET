using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DataCollector.Client.UI.Users;

namespace DataCollector.Client.UI.Converters
{
    /// <summary>
    /// Klasa implementująca konwersję uprawnienia użytkownika do widoczności.
    /// </summary>
    class CurrentUserToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Konwersja wartości.
        /// </summary>
        /// <param name="value">User</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">minimalny UserRole</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var requiredRole = (UserRole)Enum.Parse(typeof(UserRole), parameter.ToString());
            var user = value as User;

            return (user != null && requiredRole.HasFlag(user.Role)) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
