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
    /// Represents an I2C module.
    /// </summary>
    public abstract class I2CBusDevice : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Communication configuration.
        /// </summary>
        private I2cConnectionSettings connectionSettings;
        /// <summary>
        /// The hardware interface of the i2c bus.
        /// </summary>
        private I2cDevice i2cBusDevice;
        #endregion

        #region Protected Properties
        /// <summary>
        /// The ReadWrite reference handler.
        /// </summary>
        protected BusIO ReadWrite { get; private set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// The communication is initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Constructs a new instance of this class.
        /// </summary>
        /// <param name="address">the module address</param>
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
        /// Inits a module.
        /// </summary>
        /// <returns>sukces</returns>
        public async Task<bool> InitCommunication()
        {
            try
            {
                //filter of the searching for the module
                string aqs = I2cDevice.GetDeviceSelector("I2C1");

                //find device with selected filter
                var devices = await DeviceInformation.FindAllAsync(aqs);
                if (devices.Count == 0)
                    throw new InvalidProgramException("Brak urządzenia I2C");

                //initialize communication with device
                i2cBusDevice = await I2cDevice.FromIdAsync(devices[0].Id, connectionSettings);
                ReadWrite = new BusIO(i2cBusDevice);

                //module initialization
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
        /// Updates a specified property in measures instance
        /// with the new measure.
        /// </summary>
        /// <param name="measures">the input object</param>
        /// <returns>the success</returns>
        public abstract bool UpdateData([In] ref Measures measures);
        #endregion

        #region Protected Methods
        /// <summary>
        /// The module initialization method.
        /// </summary>
        protected abstract void InitHardware();
        #endregion

        #region IDisposable

        /// <summary>
        /// Releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            i2cBusDevice?.Dispose();
        }
        #endregion
    }
}
