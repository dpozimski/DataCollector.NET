using DataCollector.Server.DataFlow.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.Handlers.Adapters
{
    /// <summary>
    /// Klasa konfiguracyjna <see cref="RestDeviceHandler"/>
    /// </summary>
    public class RestDeviceHandlerConfiguration : IDeviceHandlerConfiguration
    {
        /// <summary>
        /// Referencja do zapytania o pomiary.
        /// </summary>
        public string GetMeasurementsRequest { get; set; }
        /// <summary>
        /// Referencje do zapytania o zmianę statusu diody LED.
        /// </summary>
        public string LedChangeRequest { get; set; }
        /// <summary>
        /// Referencja do zapytania odczytu statusu diody LED
        /// </summary>
        public string LedStateRequest { get; set; }
    }
}
