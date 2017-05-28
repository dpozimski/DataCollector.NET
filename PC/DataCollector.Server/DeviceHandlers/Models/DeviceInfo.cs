using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.ServiceModel;
using System.Runtime.Serialization;
using DataCollector.Server.DeviceHandlers.Interfaces;
using DataCollector.Server.DataAccess.Interfaces;

namespace DataCollector.Server.DeviceHandlers.Models
{
    /// <summary>
    /// Klasa reprezentująca komunikat WCF.
    /// </summary>
    [DataContract]
    public class DeviceInfo : IDeviceInfo
    {
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        [DataMember]
        public bool IsConnected { get; set; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        [DataMember]
        public double MeasurementsMsRequestInterval { get; set; }
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// IPv4.
        /// </summary>
        [DataMember]
        public IPAddress IPv4 { get; set; }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        [DataMember]
        public string WinVer { get; set; }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        [DataMember]
        public string Architecture { get; set; }
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        [DataMember]
        public string MacAddress { get; set; }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        [DataMember]
        public string Model { get; set; }
    }
}
