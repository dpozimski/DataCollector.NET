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
    public interface ILedController
    {
        /// <summary>
        /// Zmiana stanu diody LED.
        /// Powtórzenie nadawania wartości
        /// ze względu na błędy zapisu.
        /// </summary>
        /// <param name="state">stan</param>
        bool ChangeLedState(bool state);
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <returns>stan diody LED</returns>
        bool GetLedState();
    }
}
