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
    /// The not empty field validation rule.
    /// </summary>
    class NotEmptyValidationRule : ValidationRule
    {
        /// <summary>
        /// The minimum length of the text.
        /// </summary>
        public int MinLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            string text = (value ?? "").ToString();

            if (string.IsNullOrEmpty(text))
                result = new ValidationResult(false, TranslationExtension.GetString("RequiredField"));
            else if (text.Length < MinLength)
                result = new ValidationResult(false, string.Format(TranslationExtension.GetString("MinimumChars"), MinLength));

            return result;
        }
    }
}
