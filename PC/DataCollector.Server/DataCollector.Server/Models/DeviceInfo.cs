using DataCollector.Server.DataFlow.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.ServiceModel;

namespace DataCollector.Server.Models
{
    /// <summary>
    /// Klasa reprezentująca komunikat WCF.
    /// </summary>
    [MessageContract]
    public class DeviceInfo : IDeviceInfo
    {
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        [MessageBodyMember]
        public bool IsConnected { get; set; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        [MessageBodyMember]
        public double MeasurementsMsRequestInterval { get; set; }
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        [MessageBodyMember]
        public string Name { get; set; }
        /// <summary>
        /// IPv4.
        /// </summary>
        [MessageBodyMember]
        public IPAddress IPv4 { get; set; }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        [MessageBodyMember]
        public string WinVer { get; set; }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        [MessageBodyMember]
        public string Architecture { get; set; }
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        [MessageBodyMember]
        public string MacAddress { get; set; }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        [MessageBodyMember]
        public string Model { get; set; }
    }
}
