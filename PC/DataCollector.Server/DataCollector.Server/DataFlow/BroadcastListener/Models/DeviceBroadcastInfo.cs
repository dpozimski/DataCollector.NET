using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Models
{
    /// <summary>
    /// Information regarding a WinIotCore device.
    /// </summary>
    public class DeviceBroadcastInfo
    {
        #region Public Properties
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// IPv4.
        /// </summary>
        public IPAddress IPv4 { get; private set; }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        public string WinVer { get; private set; }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        public string Architecture { get; private set; }
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        public string MacAddress { get; private set; }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        public string Model { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy Device.
        /// </summary>
        /// <param name="machineName">Nazwa urządzenia</param>
        /// <param name="ipv4">adres urządzenia</param>
        /// <param name="macAddressString">adres mac</param>
        /// <param name="architecture">architektura systemu</param>
        /// <param name="model">model</param>
        /// <param name="winVer">wersja windows 10 iot core</param>
        public DeviceBroadcastInfo(string machineName, IPAddress ipv4, string macAddressString, string architecture, string winVer, string model)
        {
            Name = machineName;
            IPv4 = ipv4;
            MacAddress = macAddressString;
            WinVer = winVer;
            Architecture = architecture;
            Model = model;
        }
        /// <summary>
        /// Konstruktor kopiujący.
        /// <param name="broadcastInfo">obiekt oryginalny</param>
        /// </summary>
        public DeviceBroadcastInfo(DeviceBroadcastInfo broadcastInfo) :
            this(broadcastInfo.Name, broadcastInfo.IPv4, broadcastInfo.MacAddress, broadcastInfo.Architecture, broadcastInfo.WinVer, broadcastInfo.Model)
        { }
        #endregion

        #region IsEqual
        /// <summary>
        /// Metoda porównująca dwa obiekty typu Device.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || obj.GetType() != GetType())
                return false;
            DeviceBroadcastInfo other = obj as DeviceBroadcastInfo;
            return string.Equals(MacAddress, other.MacAddress) &&
                   IPv4.Equals(other.IPv4) &&
                   string.Equals(Name, other.Name);
        }
        /// <summary>
        /// Zwraca ID obliczone na podstawie metody ToString.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        /// <summary>
        /// Nadpisanie metody ToString.
        /// </summary>
        public override string ToString()
        {
            return $"\"{Name}\" - {IPv4} [{MacAddress}]";
        }
        #endregion
    }
}
