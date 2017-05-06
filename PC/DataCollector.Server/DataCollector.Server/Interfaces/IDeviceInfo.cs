using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces
{
    /// <summary>
    /// Interfejs opisujący podstawowe właściwości urządzenia.
    /// </summary>
    public interface IDeviceInfo
    {
        #region Public Properties
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// IPv4.
        /// </summary>
        IPAddress IPv4 { get; }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        string WinVer { get; }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        string Architecture { get; }
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        string MacAddress { get; }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        string Model { get; }
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        double MeasurementsMsRequestInterval { get; set; }
        #endregion
    }
}
