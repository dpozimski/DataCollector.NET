using DataCollector.Device.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Klasa abstrakcyjna definiująca podstawową funkcjonalność urządzenia z protkołem I2C.
    /// </summary>
    public abstract class I2CBusDevice : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Konfiguracja komunikacji.
        /// </summary>
        private I2cConnectionSettings connectionSettings;
        /// <summary>
        /// Interfejs sprzętowy I2C.
        /// </summary>
        private I2cDevice i2cBusDevice;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Moduł odczytu/zapisu danych.
        /// </summary>
        protected BusIO ReadWrite { get; private set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Komunikacja została zainicjalizowana.
        /// </summary>
        public bool IsInitialized { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy I2CBusDevice.
        /// </summary>
        /// <param name="address">adres urządzenia na szynie</param>
        protected I2CBusDevice(char address)
        {
            connectionSettings = new I2cConnectionSettings(Convert.ToInt32(address))
            {
                BusSpeed = I2cBusSpeed.StandardMode,
                SharingMode = I2cSharingMode.Exclusive
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Metoda inicjująca komunikację z urządzeniem peryferyjnym szyny I2C.
        /// </summary>
        /// <returns>sukces</returns>
        public async Task<bool> InitCommunication()
        {
            try
            {
                //filtr wyszukiwania urządzenia I2C1
                string aqs = I2cDevice.GetDeviceSelector("I2C1");

                // znajdź urządzenie ze wskazanym filtrem
                var devices = await DeviceInformation.FindAllAsync(aqs);
                if (devices.Count == 0)
                    throw new InvalidProgramException("Brak urządzenia I2C");

                // inicjalizuj komunikację ze wskazanym urządzeniem przez konstruktor
                i2cBusDevice = await I2cDevice.FromIdAsync(devices[0].Id, connectionSettings);
                ReadWrite = new BusIO(i2cBusDevice);

                //inicjalizacja modułu
                InitHardware();

                IsInitialized = true;

                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Wystąpił bład podczas inicjalizacji urządzenia o adresie {connectionSettings.SlaveAddress}");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Metoda aktualizująca zadanie o nowe pomiary.
        /// </summary>
        /// <param name="measures">obiekt wejściowy</param>
        /// <returns>powodzenie pobierania danych</returns>
        public abstract bool UpdateData([In] ref Measures measures);
        #endregion

        #region Protected Methods
        /// <summary>
        /// Abstrakcyjna metoda inicjująca hardware.
        /// </summary>
        protected abstract void InitHardware();
        #endregion

        #region IDisposable

        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            i2cBusDevice?.Dispose();
        }
        #endregion
    }
}
