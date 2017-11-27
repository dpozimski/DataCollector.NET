using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Interface provides a methods to control the led.
    /// </summary>
    public interface ILedController
    {
        /// <summary>
        /// CHange the LED state.
        /// The requests are repeated many times because
        /// of the write errors.
        /// </summary>
        /// <param name="state">target state</param>
        bool ChangeLedState(bool state);
        /// <summary>
        /// Cgange the LED state.
        /// The requests are repeated many times because
        /// of the write errors.
        /// </summary>
        /// <param name="state">target state</param>
        bool GetLedState();
    }
}
