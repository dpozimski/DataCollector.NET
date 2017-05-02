using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Windows.Foundation.Metadata;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Klasa implementująca metody IO dla urządzenia I2C.
    /// </summary>
    public sealed class BusIO
    {
        #region Private Fields
        private I2cDevice i2cDevice;
        #endregion

        #region Public Properties
        /// <summary>
        /// Raport ostatniego transferu.
        /// </summary>
        public I2cTransferResult LastResult { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy BusIO
        /// </summary>
        /// <param name="i2cDevice"></param>
        public BusIO(I2cDevice i2cDevice)
        {
            this.i2cDevice = i2cDevice;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Czyta pojedynczy bajt z urządzenia.
        /// </summary>
        /// <returns></returns>
        public byte Read()
        {
            byte[] buffer = new byte[1];
            LastResult = i2cDevice.ReadPartial(buffer);
            return buffer[0];
        }
        /// <summary>
        /// Metoda wysyłająca komendę do urządzenia.
        /// </summary>
        /// <param name="command">komenda</param>
        /// <returns>powodzenie operacji</returns>
        [DefaultOverload]
        public bool Write(byte command)
        {
            return Write(new List<byte>(), command);
        }
        /// <summary>
        /// Metoda wysyłająca dane do urządzenia.
        /// </summary>
        /// <param name="data">dane</param>
        /// <param name="registry">rejestr</param>
        /// <returns>powodzenie operacji</returns>
        public bool Write(byte data, byte registry)
        {
            return Write(new List<byte>() { data }, registry);
        }
        /// <summary>
        /// Metoda wysyłająca dane do urządzenia.
        /// </summary>
        /// <param name="data">dane</param>
        /// <param name="registry">rejestr</param>
        /// <returns>powodzenie operacji</returns>
        public bool Write(IList<byte> data, byte registry)
        {
            data.Insert(0, registry);
            LastResult = i2cDevice.WritePartial(data.ToArray());
            return LastResult.Status == I2cTransferStatus.FullTransfer;
        }
        /// <summary>
        /// Odczyt jednego bajtu ze wskazanego rejestru.
        /// </summary>
        /// <param name="registry">rejestr</param>
        /// <returns></returns>
        public byte Read(byte registry)
        {
            var buffer = Read(1, (byte)registry);
            return buffer[0];
        }
        /// <summary>
        /// Metoda odczytująca dane ze wskazanego rejestru urządzenia.
        /// </summary>
        /// <param name="bufferLength">rozmiar buforu wyjściowego</param>
        /// <param name="registry">rejestr</param>
        /// <exception cref="InvalidOperationException">W przypadku niepowodzenia zapytania</exception>
        /// <returns></returns>
        public byte[] Read(int bufferLength, byte registry)
        {
            byte[] writeBuffer = { registry };
            byte[] readBuffer = new byte[bufferLength];
            LastResult = i2cDevice.WriteReadPartial(writeBuffer, readBuffer);

            if (LastResult.Status != I2cTransferStatus.FullTransfer)
                Debug.Write($"Wystąpił problem z komunikacją z urządzeniem o adresie {i2cDevice.ConnectionSettings.SlaveAddress}");
            return readBuffer;
        }
        #endregion
    }
}
