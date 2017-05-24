using DataCollector.Server.DataFlow.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Interfaces
{
    /// <summary>
    /// Interfejs dostarczający kontrakt manipulacji urządzeń w sieci.
    /// </summary>
    public interface IBroadcastScanner : IDisposable
    {
        #region Events
        /// <summary>
        /// Zadzrenie wyzwalane podczas zmiany stanu urządzenia wykrytego w sieci.
        /// </summary>
        event EventHandler<DeviceUpdatedEventArgs> DeviceInfoUpdated;
        #endregion

        #region Methods
        /// <summary>
        /// Rozpoczyna nasłuchiwanie na wybranych w konstruktorze adresach.
        /// </summary>
        void StartListening();
        #endregion  
    }
}
