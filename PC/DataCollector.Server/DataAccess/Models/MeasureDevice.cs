using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataCollector.Server.DataAccess.Interfaces;

namespace DataCollector.Server.DataAccess.Models
{
    /// <summary>
    /// Klasa reprezentująca urządzenie pomiarowe w bazie danych.
    /// </summary>
    public class MeasureDevice : BaseTable
    {
        #region Public Properties
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string MacAddress { get; set; }
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// IPv4.
        /// </summary
        [Required]
        [MaxLength(25)]
        public string IPv4 { get; set; }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        [MaxLength(25)]
        public string WinVer { get; set; }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        [MaxLength(25)]
        public string Architecture { get; set; }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        [MaxLength(50)]
        public string Model { get; set; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        public double MeasurementsMsRequestInterval { get; set; }
        /// <summary>
        /// Kolekcja danych pomiarowych.
        /// </summary>
        public virtual ICollection<DeviceTimeMeasurePoint> DeviceTimeMeasurePoints { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Aktualizacja obiektu urządzenia komunikacyjnego.
        /// </summary>
        /// <param name="communicationDevice">uchwyt urządzenia komunikacyjnego</param>
        /// <returns></returns>
        public void Update(IDeviceInfo communicationDevice)
        {
            Architecture = communicationDevice.Architecture;
            IPv4 = communicationDevice.IPv4.ToString();
            Model = communicationDevice.Model;
            Name = communicationDevice.Name;
            WinVer = communicationDevice.WinVer;
            MeasurementsMsRequestInterval = communicationDevice.MeasurementsMsRequestInterval;
        }
        /// <summary>
        /// Konwersja obiektu urządzenia komunikacyjnego do typu MeasureDevice
        /// </summary>
        /// <param name="communicationDevice">uchwyt urządzenia komunikacyjnego</param>
        /// <returns></returns>
        public static MeasureDevice FromCommunicationHandler(IDeviceInfo communicationDevice)
        {
            return new MeasureDevice()
            {
                Architecture = communicationDevice.Architecture,
                IPv4 = communicationDevice.IPv4.ToString(),
                MacAddress = communicationDevice.MacAddress,
                Model = communicationDevice.Model,
                Name = communicationDevice.Name,
                WinVer = communicationDevice.WinVer,
                MeasurementsMsRequestInterval = communicationDevice.MeasurementsMsRequestInterval
            };
        }
        #endregion
    }
}
