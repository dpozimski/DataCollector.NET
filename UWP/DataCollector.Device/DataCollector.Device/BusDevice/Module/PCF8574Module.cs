using DataCollector.Device.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice.Module
{
    /// <summary>
    /// Represents an I2C expander.
    /// </summary>
    public sealed class PCF8574Module : I2CBusDevice, ILedController
    {
        #region Constants
        /// <summary>
        /// The pin of the LED.
        /// </summary>
        private const byte ControlLedPin = (1 << 3);
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        public PCF8574Module():base((char)0x21)
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Cgange the LED state.
        /// The requests are repeated many times because
        /// of the write errors.
        /// </summary>
        /// <param name="state">target state</param>
        public bool ChangeLedState(bool state)
        {
            Task.Delay(5).Wait();
            byte output = ReadWrite.Read();
            byte command = state ? (byte)(output | ControlLedPin) :
                                   (byte)(output & ~ControlLedPin);
            for(int i=0;i<100;i++)
                ReadWrite.Write(command);
            return GetLedState();
        }
        public bool GetLedState()
        {
            const int loopCount = 5;
            //shake-blocking
            List<bool> values = new List<bool>();
            for (int i = 0; i < loopCount; i++)
            {
                Task.Delay(4).Wait();
                byte output = ReadWrite.Read();
                values.Add((output & ControlLedPin) != 0);
            }
            return values.Any(s => s);
        }
        #endregion

        public override bool UpdateData([In] ref Measures measures)
        {
            measures.IsLedActive = GetLedState();
            return true;
        }

        protected override void InitHardware()
        {
            ChangeLedState(false);
        }
    }
}
