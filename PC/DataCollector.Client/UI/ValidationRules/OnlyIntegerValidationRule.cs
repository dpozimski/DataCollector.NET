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
    /// Klasa implementująca walidację zezwalającą tylko na liczby całkowite.
    /// </summary>
    class OnlyIntegerValidationRule : ValidationRule
    {
        /// <summary>
        /// Minimalna wartość.
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        /// Maksymalna wartość.
        /// </summary>
        public int MaxValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = ValidationResult.ValidResult;

            int iValue = -1;
            bool success = int.TryParse(value.ToString(), NumberStyles.Any, cultureInfo, out iValue);
            if (success && (iValue < MinValue || iValue > MaxValue))
                result = new ValidationResult(false, $"Dozwolony zakres {MinValue}-{MaxValue}");
            else if(!success)
                result = new ValidationResult(false, "Dozwolone są tylko liczby całkowite");

            return result;
        }
    }
}
