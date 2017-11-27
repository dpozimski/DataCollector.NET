using DataCollector.Device.Models;
using System;
using System.Threading.Tasks;
using UnitsNet;

namespace DataCollector.Device.BusDevice.Module
{
    /// <summary>
    /// Represents an air pressure module.
    /// </summary>
    public sealed class BMP085Module : I2CBusDevice
    {
        #region Declarations
        /// <summary>
        /// The calibration model definition.
        /// </summary>
        private class CalibrationData
        {
            public Int16 ac1;
            public Int16 ac2;
            public Int16 ac3;
            public UInt16 ac4;
            public UInt16 ac5;
            public UInt16 ac6;
            public Int16 b1;
            public Int16 b2;
            public Int16 mb;
            public Int16 mc;
            public Int16 md;
        }
        /// <summary>
        /// Kody rejestrów.
        /// </summary>
        private enum Registers : byte
        {
            CAL_AC1 = 0xAA,
            CAL_AC2 = 0xAC,
            CAL_AC3 = 0xAE,
            CAL_AC4 = 0xB0,
            CAL_AC5 = 0xB2,
            CAL_AC6 = 0xB4,
            CAL_B1 = 0xB6,
            CAL_B2 = 0xB8,
            CAL_MB = 0xBA,
            CAL_MC = 0xBC,
            CAL_MD = 0xBE,
            CHIPID = 0xD0,
            VERSION = 0xD1,
            SOFTRESET = 0xE0,
            CONTROL = 0xF4,
            TEMPDATA = 0xF6,
            PRESSUREDATA = 0xF6,
            READTEMPCMD = 0x2E,
            READPRESSURECMD = 0x34,
        }
        #endregion

        #region Constants
        /// <summary>
        /// The standard mode.
        /// </summary>
        private const byte measureMode = 0x01;
        #endregion

        #region Private Fields
        /// <summary>
        /// The calibration data.
        /// </summary>
        private CalibrationData calibrationData;
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public BMP085Module() : base((char)0x77)
        {
            calibrationData = new CalibrationData();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Reads the word from register.
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private ushort ReadWord(Registers reg)
        {
            var buffer = ReadWrite.Read(2, (byte)reg);
            return (ushort)(buffer[0] << 8 | buffer[1]);
        }
        /// <summary>
        /// Reads the int 16 value from the register.
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        private short ReadShort(Registers reg)
        {
            return (short)ReadWord(reg);
        }
        /// <summary>
        /// Caluclates B5 value which will be used to calculate the main values.
        /// </summary>
        /// <param name="ut">not procceded value</param>
        /// <returns>współczynnik B5</returns>
        private Int32 ComputeB5(int ut)
        {
            int X1 = (ut - (int)calibrationData.ac6) * ((int)calibrationData.ac5) >> 15;
            int X2 = ((int)calibrationData.mc << 11) / (X1 + (int)calibrationData.md);
            return X1 + X2;
        }
        /// <summary>
        /// Reads the calibration data from module.
        /// </summary>
        private void ReadCalibrationData()
        {
            calibrationData.ac1 = ReadShort(Registers.CAL_AC1);
            calibrationData.ac2 = ReadShort(Registers.CAL_AC2);
            calibrationData.ac3 = ReadShort(Registers.CAL_AC3);
            calibrationData.ac4 = ReadWord(Registers.CAL_AC4);
            calibrationData.ac5 = ReadWord(Registers.CAL_AC5);
            calibrationData.ac6 = ReadWord(Registers.CAL_AC6);
            calibrationData.b1 = ReadShort(Registers.CAL_B1);
            calibrationData.b2 = ReadShort(Registers.CAL_B2);
            calibrationData.mb = ReadShort(Registers.CAL_MB);
            calibrationData.mc = ReadShort(Registers.CAL_MC);
            calibrationData.md = ReadShort(Registers.CAL_MD);
        }
        /// <summary>
        /// Reads raw temperature from the module.
        /// </summary>
        /// <returns>Raw Temperature</returns>
        private int ReadRawTemperature()
        {
            ReadWrite.Write((byte)Registers.READTEMPCMD, (byte)Registers.CONTROL);
            Task.Delay(5).Wait();
            ushort t = ReadWord(Registers.TEMPDATA);
            return t;
        }

        /// <summary>
        /// Reads raw humidity value.
        /// </summary>
        /// <returns>Raw Pressure</returns>
        private int ReadRawPressure()
        {
            byte p8;
            uint p16;
            int p32;

            ReadWrite.Write((byte)Registers.READPRESSURECMD + (byte)((byte)measureMode << 6), (byte)Registers.CONTROL);

            Task.Delay(8).Wait();

            p16 = ReadWord(Registers.PRESSUREDATA);
            p32 = (int)(p16 << 8);
            p8 = ReadWrite.Read((byte)(Registers.PRESSUREDATA + 2));
            p32 += p8;
            p32 >>= (8 - (byte)measureMode);

            return p32;
        }
        /// <summary>
        /// Reads the pressure in hPa format.
        /// </summary>
        /// <returns></returns>
        private float GetPreasure()
        {
            int ut = 0, up = 0, compp = 0;
            int x1, x2, b5, b6, x3, b3, p;
            uint b4, b7;

            /* Get the raw pressure and temperature values */
            ut = ReadRawTemperature();
            up = ReadRawPressure();

            /* Temperature compensation */
            b5 = ComputeB5(ut);

            /* Pressure compensation */
            b6 = b5 - 4000;
            x1 = (calibrationData.b2 * ((b6 * b6) >> 12)) >> 11;
            x2 = (calibrationData.ac2 * b6) >> 11;
            x3 = x1 + x2;
            b3 = (((((int)calibrationData.ac1) * 4 + x3) << (byte)measureMode) + 2) >> 2;
            x1 = (calibrationData.ac3 * b6) >> 13;
            x2 = (calibrationData.b1 * ((b6 * b6) >> 12)) >> 16;
            x3 = ((x1 + x2) + 2) >> 2;
            b4 = (calibrationData.ac4 * (uint)(x3 + 32768)) >> 15;
            b7 = ((uint)(up - b3) * (uint)(50000 >> (byte)measureMode));

            if (b7 < 0x80000000)
            {
                p = (int)((b7 << 1) / b4);
            }
            else
            {
                p = (int)((b7 / b4) << 1);
            }

            x1 = (p >> 8) * (p >> 8);
            x1 = (x1 * 3038) >> 16;
            x2 = (-7357 * p) >> 16;
            compp = p + ((x1 + x2 + 3791) >> 4);

            //converts the value to hPa.
            return Convert.ToSingle(Pressure.From(compp, UnitsNet.Units.PressureUnit.Pascal).Hectopascals);
        }
        #endregion

        public override bool UpdateData([System.Runtime.InteropServices.In] ref Measures measures)
        {
            measures.AirPressure = GetPreasure();
            return true;
        }

        protected override void InitHardware()
        {
            ReadCalibrationData();
        }
    }
}
