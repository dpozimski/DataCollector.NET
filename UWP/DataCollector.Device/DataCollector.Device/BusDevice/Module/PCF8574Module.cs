using DataCollector.Device.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice.Module
{
    /// <summary>
    /// Klasa implementująca obsługę komunikacji urządzenia rozszerzającego IO.
    /// </summary>
    public sealed class PCF8574Module : I2CBusDevice, ILedController
    {
        #region Constants
        /// <summary>
        /// Pin sterujący diodą LED.
        /// </summary>
        private const byte ControlLedPin = (1 << 3);
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy PCF8574.
        /// </summary>
        public PCF8574Module():base((char)0x21)
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Zmiana stanu diody LED.
        /// Powtórzenie nadawania wartości
        /// ze względu na błędy zapisu.
        /// </summary>
        /// <param name="state">stan</param>
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
            //eliminacja drgań
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
