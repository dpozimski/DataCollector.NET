using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.Handlers.Interfaces
{
    /// <summary>
    /// Interfejs określający zakres interakcji z urządzeniem pomiarowym.
    /// </summary>
    public interface IDeviceHandler : IDeviceInfo, IDisposable
    {
        #region Events
        /// <summary>
        /// Zdarzenie nadejścia pomiarów.
        /// </summary>
        event EventHandler<MeasuresArrivedEventArgs> MeasuresArrived;
        /// <summary>
        /// Sygnalizacja zerwaniu połączenia.
        /// </summary>
        event EventHandler<IDeviceHandler> Disconnected;
        #endregion

        #region Methods
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <returns></returns>
        bool GetLedState();
        /// <summary>
        /// Zmiana stanu diody LED.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        bool ChangeLedState(bool state);
        /// <summary>
        /// Rozłączenie z serwerem.
        /// </summary>
        /// <returns></returns>
        bool Disconnect();
        /// <summary>
        /// Połączenie z serwerem pomiarów.
        /// </summary>
        /// <returns></returns>
        bool Connect();
        #endregion
    }
}
