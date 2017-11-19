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
    /// Class which implements the only integer validation rule.
    /// </summary>
    class OnlyIntegerValidationRule : ValidationRule
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        /// The maximum value.
        /// </summary>
        public int MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            int iValue = -1;
            bool success = int.TryParse(value.ToString(), NumberStyles.Any, cultureInfo, out iValue);
            if (success && (iValue < MinValue || iValue > MaxValue))
                result = new ValidationResult(false, string.Format(TranslationExtension.GetString("AllowedRange"), MinValue, MaxValue));
            else if(!success)
                result = new ValidationResult(false, TranslationExtension.GetString("OnlyIntegerValuesAllowed"));

            return result;
        }
    }
}
