using DataCollector.Device.BusDevice.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// The bus devices factory.
    /// </summary>
    public static class BusDevicesFactory
    {
        /// <summary>
        /// Creates a collection of all devices connected to I2C bus.
        /// </summary>
        /// <returns>bus devices</returns>
        public static IEnumerable<I2CBusDevice> Create()
        {
            var busDevices = new List<I2CBusDevice>();
            busDevices.Add(new BMP085Module());
            busDevices.Add(new MPU_6050Module());
            busDevices.Add(new PCF8574Module());
            busDevices.Add(new Sensirion_SHT21Module());
            return busDevices;
        }
    }
}
