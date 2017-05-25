using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces
{
    /// <summary>
    /// Interfejs określający funkcjonalność komunikacji z urządzeniami zewnętrznym.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ICommunicationServiceCallback))]
    public interface ICommunicationService : IDisposable
    {
        #region Properties
        /// <summary>
        /// Aktualnie podłączone urządzenia.
        /// </summary>
        [DataMember]
        IEnumerable<DeviceInfo> Devices { get; }
        /// <summary>
        /// Usługi serwisu uruchomione.
        /// </summary>
        [DataMember]
        bool IsStarted { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        [OperationContract]
        void RegisterCallbackChannel();
        /// <summary>
        /// Uruchamia usługi serwisu.
        /// </summary>
        [OperationContract]
        void Start();
        /// <summary>
        /// Zatrzymuje usługi serwisu.
        /// </summary>
        [OperationContract]
        void Stop();
        /// <summary>
        /// Metoda inicjująca połączenie z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        [OperationContract]
        bool ConnectDevice(DeviceInfo device);
        /// <summary>
        /// Metoda przerywająca trwającą komunikacje z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        [OperationContract]
        bool DisconnectDevice(DeviceInfo device);
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <param name="deviceHandler">urządzenie</param>
        /// <returns></returns>
        [OperationContract]
        bool GetLedState(DeviceInfo deviceHandler);
        /// <summary>
        /// Metoda zmieniająca stan diody we wskazanym urządzeniu.
        /// </summary>
        /// <param name="target">urządzenie docelowe</param>
        /// <param name="state">stan diody</param>
        /// <returns></returns>
        [OperationContract]
        bool ChangeLedState(DeviceInfo target, bool state);
        /// <summary>
        /// Metoda dodające nowe urządzenie symulujące komunikację.
        /// </summary>
        [OperationContract]
        void AddSimulatorDevice();
        #endregion
    }
}
