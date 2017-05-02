using DataCollector.Device.BusDevice.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Fabryka urządzeń pomiarowych.
    /// </summary>
    public static class BusDevicesFactory
    {
        /// <summary>
        /// Tworzy kolekcję peryferii podłączonych do szyny I2C.
        /// </summary>
        /// <returns>kolekcja urządzeń szyny I2C</returns>
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
