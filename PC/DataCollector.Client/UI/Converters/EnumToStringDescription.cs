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
    /// Konwersja typu wyliczeniowego na reprezentację tekstu z wykorzystaniem refleksji.
    /// </summary>
    public class EnumToStringDescription : IValueConverter
    {
        /// <summary>
        /// Konwersja typu wyliczeniowego do jego opisu.
        /// </summary>
        /// <param name="value">Enum posiadający DescriptionAttribute</param>
        /// <returns></returns>
        public string ToDescription(Enum value)
        {
            return TranslationExtension.GetString(value.ToString());
        }
        /// <summary>
        /// Konwersja typu wyliczeniowego do jego jednostki.
        /// </summary>
        /// <param name="value">Enum posiadający MeasureInformationAttribute</param>
        /// <returns></returns>
        public string ToUnit(Enum value)
        {
            return "";
        }

        #region Private Methods
        /// <summary>
        /// Zwraca tekstową reprezentację typu wiliczeniowego dla ogólnego typu Enum.
        /// Z wykorzystaniem atrybutu Description i refleksji.
        /// </summary>
        /// <param name="value">wartość z atrybutem description</param>
        /// <returns></returns>
        private TAttribute GetAttribute<TAttribute>(Enum value) where TAttribute : DescriptionAttribute
        {
            if (value == null)
                return null;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            TAttribute[] attributes =
                (TAttribute[])fi.GetCustomAttributes(
                typeof(TAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0];
            else
                return null;
        }
        #endregion

        #region IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
                return ToDescription((Enum)value);
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
