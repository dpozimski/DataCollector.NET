using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Atrybut typu wyliczeniowego określający jego nazwę oraz jednostkę.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class MeasureInformationAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Jednostka.
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Konstruktor klasy MeasureInformationAttribute.
        /// </summary>
        /// <param name="description">opis</param>
        /// <param name="unit">jednostka</param>
        public MeasureInformationAttribute(string description, string unit):base(description)
        {
            this.Unit = unit;
        }
    }
}
