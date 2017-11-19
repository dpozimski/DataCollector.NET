using DataCollector.Client.Translation;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.Extensions;
using DataCollector.Client.UI.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataCollector.Client.UI.Converters
{
    /// <summary>
    /// Class which converts the enum value to string representation
    /// from translation resources.
    /// </summary>
    public class EnumToStringDescription : IValueConverter
    {
        #region [Public Methods]
        /// <summary>
        /// Gets the description fo the enum value.
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns></returns>
        public string ToDescription(Enum value)
        {
            return TranslationExtension.GetString(value.ToString());
        }
        /// <summary>
        /// Converts the enum value to its unit.
        /// </summary>
        /// <param name="value">The enum value</param>
        /// <returns></returns>
        public string ToUnit(Enum value)
        {
            return "";
        }
        #endregion

        #region IValueConverter        
        /// <summary>
        /// Converts the specified value to description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <CreatedOn>19.11.2017 12:11</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
                return ToDescription((Enum)value);
            else
                return value;
        }
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <CreatedOn>19.11.2017 12:12</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
