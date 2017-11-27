using DataCollector.Device.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice.Module
{
    /// <summary>
    /// Represents a gyroscope && accelerometer device.
    /// </summary>
    public sealed class MPU_6050Module : I2CBusDevice
    {
        #region Declarations
        /// <summary>
        /// The device registry definition.
        /// </summary>
        private enum Registers : byte
        {
            PwrMgmt1 = 0x6B,
            SmplrtDiv = 0x19,
            Config = 0x1A,
            GyroConfig = 0x1B,
            AccelConfig = 0x1C,
            FifoEn = 0x23,
            IntEnable = 0x38,
            IntStatus = 0x3A,
            UserCtrl = 0x6A,
            FifoCount = 0x72,
            FifoRW = 0x74,
            SensorBytes = 0x0C
        }
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public MPU_6050Module() : base((char)0x68)
        {

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Reads the word from the device.
        /// </summary>
        /// <param name="registry">registry</param>
        /// <returns></returns>
        private ushort ReadWord(Registers registry)
        {
            byte[] buffer = ReadWrite.Read(2, (byte)registry);
            return (ushort)(((int)buffer[0] << 8) | (int)buffer[1]);
        }
        /// <summary>
        /// The averages the data.
        /// </summary>
        /// <param name="values">values</param>
        /// <returns>the averaged value from the collection</returns>
        private float Average(params float[] values)
        {
            return values.Average();
        }
        #endregion

        public override bool UpdateData([System.Runtime.InteropServices.In] ref Measures measures)
        {
            //creates a list of temp values
            List<SpherePoint> accelerometerValues = new List<SpherePoint>();
            List<SpherePoint> gyroscopeValues = new List<SpherePoint>();

            //reads the count of the pending measures
            int count = ReadWord(Registers.FifoCount);
            //reads the measures
            while (count >= (byte)Registers.SensorBytes)
            {
                var data = ReadWrite.Read((byte)Registers.SensorBytes, (byte)Registers.FifoRW);
                count -= (byte)Registers.SensorBytes;

                SpherePoint accelerometer = new SpherePoint()
                {
                    X = (short)(data[0] << 8 | data[1]) / 16384f,
                    Y = (short)(data[2] << 8 | data[3]) / 16384f,
                    Z = (short)(data[4] << 8 | data[5]) / 16384f
                };

                SpherePoint gyroscope = new SpherePoint()
                {
                    X = (short)(data[6] << 8 | data[7]) / 131f,
                    Y = (short)(data[8] << 8 | data[9]) / 131f,
                    Z = (short)(data[10] << 8 | data[11]) / 131f
                };

                accelerometerValues.Add(accelerometer);
                gyroscopeValues.Add(gyroscope);
            }

            measures.Accelerometer = accelerometerValues.Aggregate((a, b) =>
                                            new SpherePoint(Average(a.X, b.X), Average(a.Y, b.Y), Average(a.Z, b.Z)));
            measures.Gyroscope = gyroscopeValues.Aggregate((a, b) =>
                                        new SpherePoint(Average(a.X, b.X), Average(a.Y, b.Y), Average(a.Z, b.Z)));

            return true;
        }

        protected override void InitHardware()
        {
            //wait for the device power initialization
            Task.Delay(3).Wait();
            //device reset
            ReadWrite.Write(0x80, (byte)Registers.PwrMgmt1);
            Task.Delay(100).Wait();
            ReadWrite.Write(0x02, (byte)Registers.PwrMgmt1);
            //reset fifo
            ReadWrite.Write(0x04, (byte)Registers.UserCtrl);
            // the source clock = gyro x
            ReadWrite.Write(0x01, (byte)Registers.PwrMgmt1);
            // +/- 250 steps/sec
            ReadWrite.Write(0x00, (byte)Registers.GyroConfig);
            // +/- 2g
            ReadWrite.Write(0x00, (byte)Registers.AccelConfig);
            // 184 Hz, 2ms delay
            ReadWrite.Write(0x01, (byte)Registers.Config);
            // set rate 50Hz
            ReadWrite.Write(19, (byte)Registers.SmplrtDiv);
            // enable accel and gyro to read into fifo
            ReadWrite.Write(0x78, (byte)Registers.FifoEn);
            // reset and enable fifo
            ReadWrite.Write(0x40, (byte)Registers.UserCtrl);
        }
    }
}
