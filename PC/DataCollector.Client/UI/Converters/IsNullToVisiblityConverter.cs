using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DataCollector.Client.UI.Converters
{
    /// <summary>
    /// Klasa implementująca konwersję porównania obiektu do null do widoczności.
    /// jako parametr podaje się czy ma być przeprowadzona inwersja wartości.
    /// </summary>
    class IsNullToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            bool.TryParse(parameter as string, out invert);

            if(invert)
                return (value != null) ? Visibility.Collapsed : Visibility.Visible;
            else
                return (value != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
