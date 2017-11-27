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
    /// Contians a method that are the helpers to calculate the values taken from each I2C module.
    /// </summary>
    public sealed class BusIO
    {
        #region Private Fields
        private I2cDevice i2cDevice;
        #endregion

        #region Public Properties
        /// <summary>
        /// The last transfer report.
        /// </summary>
        public I2cTransferResult LastResult { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="i2cDevice"></param>
        public BusIO(I2cDevice i2cDevice)
        {
            this.i2cDevice = i2cDevice;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Reads the single byte from the device.
        /// </summary>
        /// <returns></returns>
        public byte Read()
        {
            byte[] buffer = new byte[1];
            LastResult = i2cDevice.ReadPartial(buffer);
            return buffer[0];
        }
        /// <summary>
        /// Sends the command to the device.
        /// </summary>
        /// <param name="command">komenda</param>
        /// <returns>powodzenie operacji</returns>
        [DefaultOverload]
        public bool Write(byte command)
        {
            return Write(new List<byte>(), command);
        }
        /// <summary>
        /// Sends the data to registry.
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="registry">registry</param>
        /// <returns>succes of the operation</returns>
        public bool Write(byte data, byte registry)
        {
            return Write(new List<byte>() { data }, registry);
        }
        /// <summary>
        /// Sends the data to the device.
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="registry">registry</param>
        /// <returns>success of the operation</returns>
        public bool Write(IList<byte> data, byte registry)
        {
            data.Insert(0, registry);
            LastResult = i2cDevice.WritePartial(data.ToArray());
            return LastResult.Status == I2cTransferStatus.FullTransfer;
        }
        /// <summary>
        /// Reads the single byte from the registry.
        /// </summary>
        /// <param name="registry">registry</param>
        /// <returns>the value</returns>
        public byte Read(byte registry)
        {
            var buffer = Read(1, (byte)registry);
            return buffer[0];
        }
        /// <summary>
        /// Reads the bytes collection from the registry.
        /// </summary>
        /// <param name="bufferLength">buffer length</param>
        /// <param name="registry">registry</param>
        /// <exception cref="InvalidOperationException">when the connection will be failed</exception>
        /// <returns></returns>
        public byte[] Read(int bufferLength, byte registry)
        {
            byte[] writeBuffer = { registry };
            byte[] readBuffer = new byte[bufferLength];
            LastResult = i2cDevice.WriteReadPartial(writeBuffer, readBuffer);

            if (LastResult.Status != I2cTransferStatus.FullTransfer)
                Debug.Write($"There was a communication issue with {i2cDevice.ConnectionSettings.SlaveAddress}");
            return readBuffer;
        }
        #endregion
    }
}
