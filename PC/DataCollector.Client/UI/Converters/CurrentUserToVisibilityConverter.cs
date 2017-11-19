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
    /// Class which has ability to convert the actual user state to visibility.
    /// </summary>
    class CurrentUserToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// The value conversion.
        /// </summary>
        /// <param name="value">User</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">required role</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var requiredRole = (UserRole)Enum.Parse(typeof(UserRole), parameter.ToString());
            var user = value as User;

            return (user != null && requiredRole.HasFlag(user.Role)) ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <CreatedOn>19.11.2017 12:09</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
