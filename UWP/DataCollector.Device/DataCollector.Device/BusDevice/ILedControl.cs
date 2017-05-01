using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Interfejs kontrolujący stan diody LED.
    /// </summary>
    public interface ILedControl
    {
        /// <summary>
        /// Zmiana stanu diody LED.
        /// Powtórzenie nadawania wartości
        /// ze względu na błędy zapisu.
        /// </summary>
        /// <param name="val">stan</param>
        bool ChangeLedState(bool val);
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <returns>stan diody LED</returns>
        bool GetLedState();
    }
}
