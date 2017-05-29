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
    /// Klasa implementująca metodę walidacji pustych pól.
    /// </summary>
    class NotEmptyValidationRule : ValidationRule
    {
        /// <summary>
        /// Minimalna długość tekstu.
        /// </summary>
        public int MinLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            string text = (value ?? "").ToString();

            if (string.IsNullOrEmpty(text))
                result = new ValidationResult(false, "Pole wymagane");
            else if (text.Length < MinLength)
                result = new ValidationResult(false, $"Minimum {MinLength} znaków");

            return result;
        }
    }
}
