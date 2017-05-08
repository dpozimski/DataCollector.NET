using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces
{
    /// <summary>
    /// Interfejs określający funkcjonalność komunikacji z urządzeniami zewnętrznym.
    /// </summary>
    public interface ICommunication : IDisposable
    {
        #region Events
        /// <summary>
        /// Aktualne pomiary z pochodzące z urządzenia
        /// </summary>
        event EventHandler<MeasuresArrivedEventArgs> MeasuresArrived;
        /// <summary>
        /// Wykryto urządzenie w sieci.
        /// </summary>
        event EventHandler<DeviceUpdatedEventArgs> DeviceChangedState;
        #endregion

        #region Properties
        /// <summary>
        /// Aktualnie podłączone urządzenia.
        /// </summary>
        IEnumerable<IDeviceInfo> Devices { get; }
        /// <summary>
        /// Usługi serwisu uruchomione.
        /// </summary>
        bool IsStarted { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Uruchamia usługi serwisu,
        /// </summary>
        void Start();
        /// <summary>
        /// Zatrzymuje usługi serwisu.
        /// </summary>
        void Stop();
        /// <summary>
        /// Metoda inicjująca połączenie z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        bool ConnectDevice(IDeviceInfo device);
        /// <summary>
        /// Metoda przerywająca trwającą komunikacje z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        bool DisconnectDevice(IDeviceInfo device);
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <param name="deviceHandler">urządzenie</param>
        /// <returns></returns>
        bool GetLedState(IDeviceInfo deviceHandler);
        /// <summary>
        /// Metoda zmieniająca stan diody we wskazanym urządzeniu.
        /// </summary>
        /// <param name="target">urządzenie docelowe</param>
        /// <param name="state">stan diody</param>
        /// <returns></returns>
        bool ChangeLedState(IDeviceInfo target, bool state);
        #endregion
    }
}
