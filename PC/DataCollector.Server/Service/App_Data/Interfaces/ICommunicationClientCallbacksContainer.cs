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
    /// Kontener klientów subskrybujących proxy.
    /// </summary>
	public interface ICommunicationClientCallbacksContainer : IDisposable
    {
        #region Properties
        /// <summary>
        /// Lista podłączonych klientów.
        /// </summary>
        IEnumerable<ICommunicationServiceCallback> Clients { get; }
        /// <summary>
        /// Proxy aktualnie podłączonego klienta.
        /// </summary>
        ICommunicationServiceCallback CurrentClient { get; }
        /// <summary>
        /// Identyfikator aktualnego klienta.
        /// </summary>
        string CurrentClientId { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        /// <param name="serviceCallback">callback</param>
        /// <param name="sessionId">id</param>
        void RegisterCallbackChannel(string sessionId, ICommunicationServiceCallback serviceCallback);
        /// <summary>
        /// Powiadomienie o aktualizacji stanu urządzenia.
        /// </summary>
        /// <param name="deviceUpdated">dane dot. aktualizacji</param>
        void OnDeviceChangedState(Models.DeviceUpdatedEventArgs deviceUpdated);
        /// <summary>
        /// Powiadomienie o nadejściu pomiarów.
        /// </summary>
        /// <param name="measures">pomiary</param>
        void OnMeasuresArrived(MeasuresArrivedEventArgs measures);
        #endregion
    }
}