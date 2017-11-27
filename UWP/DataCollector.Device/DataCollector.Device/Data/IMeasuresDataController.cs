using DataCollector.Device.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.Data
{
    /// <summary>
    /// The measures holder interface.
    /// </summary>
    public interface IMeasuresDataController
    {
        /// <summary>
        /// The newest measures.
        /// </summary>
        Measures NewestMeasure { get; set; }

        /// <summary>
        /// Returns a measures in JSON.
        /// </summary>
        string NewestMeasureOutput { get; }
    }
}
