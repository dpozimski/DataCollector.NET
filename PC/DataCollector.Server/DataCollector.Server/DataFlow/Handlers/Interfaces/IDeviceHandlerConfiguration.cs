using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollector.Server.DataFlow.Handlers.Interfaces
{
    /// <summary>
    /// Kontrakt konfiguracji zapytań uchwytu urządzenia.
    /// </summary>
    public interface IDeviceHandlerConfiguration
    {
        /// <summary>
        /// Referencja do zapytania o pomiary.
        /// </summary>
        string GetMeasurementsRequest { get; }
        /// <summary>
        /// Referencje do zapytania o zmianę statusu diody LED.
        /// </summary>
        string LedChangeRequest { get; }
        /// <summary>
        /// Referencja do zapytania odczytu statusu diody LED
        /// </summary>
        string LedStateRequest { get; }
    }
}
