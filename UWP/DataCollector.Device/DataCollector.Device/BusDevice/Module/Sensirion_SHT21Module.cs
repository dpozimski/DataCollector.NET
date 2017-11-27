using DataCollector.Device.Models;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace DataCollector.Device.BusDevice.Module
{
    /// <summary>
    /// Represents a humidity && temperature module.
    /// </summary>
    public sealed class Sensirion_SHT21Module : I2CBusDevice
    {
        #region Declarations
        /// <summary>
        /// Register addresses.
        /// </summary>
        private enum Registers : byte
        {
            TRIG_T_MEASUREMENT_HM = 0xE3, // command trig. temp meas. hold master
            TRIG_RH_MEASUREMENT_HM = 0xE5, // command trig. humidity meas. hold master
            TRIG_T_MEASUREMENT_POLL = 0xF3, // command trig. temp meas. no hold master
            TRIG_RH_MEASUREMENT_POLL = 0xF5, // command trig. humidity meas. no hold master
            USER_REG_W = 0xE6, // command writing user register
            USER_REG_R = 0xE7, // command reading user register
            SOFT_RESET = 0xFE  // command soft reset
        }
        /// <summary>
        /// The configuration commands.
        /// </summary>
        private enum ConfigCommands : byte
        {
            SHT2x_RES_12_14BIT = 0x00, // RH=12bit, T=14bit
            SHT2x_RES_8_12BIT = 0x01, // RH= 8bit, T=12bit
            SHT2x_RES_10_13BIT = 0x80, // RH=10bit, T=13bit
            SHT2x_RES_11_11BIT = 0x81, // RH=11bit, T=11bit
            SHT2x_RES_MASK = 0x81  // Mask for res. bits (7,0) in user reg.
        }
        /// <summary>
        /// The measure mode.
        /// </summary>
        private enum ReadMode
        {
            Temperature, Humidity
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public Sensirion_SHT21Module() : base((char)0x40)
        {

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// CRC8 validation.
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="checksum">CRC8 value</param>
        /// <returns></returns>
        private bool CrcValidate(byte[] data, byte checksum)
        {
            ushort polynomial = 0x131;
            byte crc = 0;
            byte byteCtr;
            //calculates 8-Bit checksum with given polynomial
            for (byteCtr = 0; byteCtr < data.Length; ++byteCtr)
            {
                crc ^= (data[byteCtr]);
                for (byte bit = 8; bit > 0; --bit)
                {
                    if ((crc & 0x80) != 0)
                        crc = (byte)((crc << 1) ^ polynomial);
                    else
                        crc = (byte)(crc << 1);
                }
            }
            return (crc == checksum);
        }
        /// <summary>
        /// The temperature calculation based on the value.
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>temperature in Celsius</returns>
        private float CalculateTemperature(ushort value)
        {
            float temperature = Convert.ToSingle(-46.85 + 175.72 / 65536 * value);
            return temperature;
        }
        /// <summary>
        /// The humidity calculation based on the value.
        /// </summary>
        /// <param name="value">vale</param>
        /// <returns>humidity in RH</returns>
        private float CalculateHumidity(ushort value)
        {
            float humidity = Convert.ToSingle(-6.0 + 125.0 / 65536 * value);
            return humidity;
        }
        /// <summary>
        /// Reads the measure by the given argument.
        /// </summary>
        /// <param name="mode">read mode</param>
        /// <returns>raw value</returns>
        private float? ReadMeasure(ReadMode mode)
        {
            float? measure = null;
            byte[] data = null;

            //value with crc
            switch (mode)
            {
                case ReadMode.Humidity:
                    data = ReadWrite.Read(3, (byte)Registers.TRIG_RH_MEASUREMENT_HM);
                    break;
                case ReadMode.Temperature:
                    data = ReadWrite.Read(3, (byte)Registers.TRIG_T_MEASUREMENT_HM);
                    break;
            }
            if (ReadWrite.LastResult.Status == I2cTransferStatus.FullTransfer)
            {
                byte[] bValue = data.Take(2).ToArray();
                bool success = CrcValidate(bValue, data[2]);
                if (success)
                {
                    ushort rawValue = BitConverter.ToUInt16(bValue.Reverse().ToArray(), 0);
                    rawValue = (ushort)((short)rawValue & ~0x0003);
                    if (mode == ReadMode.Temperature)
                        measure = CalculateTemperature(rawValue);
                    else
                        measure = CalculateHumidity(rawValue);
                }
            }
            return measure;
        }
        #endregion

        public override bool UpdateData([In] ref Measures measures)
        {
            measures.Temperature = ReadMeasure(ReadMode.Temperature);
            measures.Humidity = ReadMeasure(ReadMode.Humidity);
            return true;
        }

        protected override void InitHardware()
        {
            //reset
            ReadWrite.Write((byte)Registers.SOFT_RESET);
            //sleep for 15ms
            Task.Delay(15).Wait();
            //reads the module configuration
            byte userRegistry = ReadWrite.Read((byte)Registers.USER_REG_R);
            //configure
            userRegistry = (byte)((userRegistry & ~(byte)ConfigCommands.SHT2x_RES_MASK) | (byte)ConfigCommands.SHT2x_RES_10_13BIT);
            //sends the configuration
            ReadWrite.Write(userRegistry, (byte)Registers.USER_REG_W);
        }
    }
}
