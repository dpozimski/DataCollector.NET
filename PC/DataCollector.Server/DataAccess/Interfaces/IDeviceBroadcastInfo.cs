using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Interfaces
{
    /// <summary>
    /// Interfejs określający urządzenie znalezione w sieci.
    /// </summary>
    public interface IDeviceBroadcastInfo
    {
        #region Public Properties
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// IPv4.
        /// </summary>
        string IPv4 { get; }
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
        #endregion
    }
}
