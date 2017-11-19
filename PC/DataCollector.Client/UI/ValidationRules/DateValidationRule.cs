using DataCollector.Client.Translation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataCollector.Client.UI.ValidationRules
{
    /// <summary>
    /// The date validation rule.
    /// </summary>
    class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string date = value as string;

            DateTime time;
            bool success = DateTime.TryParse(date, CultureInfo.CurrentCulture, DateTimeStyles.None, out time);
            if (success)
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, TranslationExtension.GetString("InvalidDate"));
        }
    }
}
