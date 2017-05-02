using DataCollector.Device.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.Data
{
    /// <summary>
    /// Interfejs kontrolera pomiarów.
    /// </summary>
    public interface IMeasuresDataController
    {
        /// <summary>
        /// Uchwyt do najnowszych pomiarów.
        /// </summary>
        Measures NewestMeasure { get; set; }

        /// <summary>
        /// Zwraca pomiary w formacie JSON.
        /// </summary>
        string NewestMeasureOutput { get; }
    }
}
