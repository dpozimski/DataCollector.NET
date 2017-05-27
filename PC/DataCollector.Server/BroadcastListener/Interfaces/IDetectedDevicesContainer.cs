using DataCollector.Server.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener.Interfaces
{
    /// <summary>
    /// Interfejs definiujacy podstawowe funkcjonalności kontenera wykrytych urzadze w sieci.
    /// </summary>
    public interface IDetectedDevicesContainer : IDisposable
    {
        #region Events
        /// <summary>
        /// Zdarzenie wyzwalane podczas aktualizacji informacji o urządzeniu.
        /// </summary>
        event EventHandler<DeviceUpdatedEventArgs> DeviceInfoUpdated;
        #endregion

        #region Public Properties
        /// <summary>
        /// Enumeracja urządzeń w sieci.
        /// </summary>
        IEnumerable<IDeviceBroadcastInfo> Devices { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Metoda aktualizująca słownik o nowe informacje o urządzeniu zwracając tym samym status aktualizacji.
        /// Wywołuje zdarzenie aktualizacji danych o urządzeniu.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        void Update(IDeviceBroadcastInfo device);
        #endregion
    }
}
