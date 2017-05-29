using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DeviceHandlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces.Communication
{
    /// <summary>
    /// Interfejs określający funkcjonalność komunikacji z urządzeniami zewnętrznym.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ICommunicationServiceCallback))]
    public interface ICommunicationService : IDisposable
    {
        #region Properties
        /// <summary>
        /// Proxy aktualnie podłączonego klienta.
        /// </summary>
        ICommunicationServiceCallback CurrentClient { get; }
        /// <summary>
        /// Aktualnie podłączone urządzenia.
        /// </summary>
        [DataMember]
        IEnumerable<MeasureDevice> Devices { get; }
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
        bool ConnectDevice(MeasureDevice device);
        /// <summary>
        /// Metoda przerywająca trwającą komunikacje z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        [OperationContract]
        bool DisconnectDevice(MeasureDevice device);
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <param name="deviceHandler">urządzenie</param>
        /// <returns></returns>
        [OperationContract]
        bool GetLedState(MeasureDevice deviceHandler);
        /// <summary>
        /// Metoda zmieniająca stan diody we wskazanym urządzeniu.
        /// </summary>
        /// <param name="target">urządzenie docelowe</param>
        /// <param name="state">stan diody</param>
        /// <returns></returns>
        [OperationContract]
        bool ChangeLedState(MeasureDevice target, bool state);
        /// <summary>
        /// Metoda dodające nowe urządzenie symulujące komunikację.
        /// </summary>
        [OperationContract]
        void AddSimulatorDevice();
        #endregion
    }
}
