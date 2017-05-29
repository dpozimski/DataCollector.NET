using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace netoaster
{
    public static class NotificationColor
    {
        public static readonly SolidColorBrush Success = "#4BA253".ToSolidColorBrush();
        public static readonly SolidColorBrush Info = "#5EAEC5".ToSolidColorBrush();
        public static readonly SolidColorBrush Error = "#C43829".ToSolidColorBrush();
        public static readonly SolidColorBrush Warning = "#FF9400".ToSolidColorBrush();

        private static SolidColorBrush ToSolidColorBrush(this string hex)
        {
            var convertFromString = ColorConverter.ConvertFromString(hex);
            var color = (Color?)convertFromString ?? Colors.Black;
            return new SolidColorBrush(color);
        }
    }
}
