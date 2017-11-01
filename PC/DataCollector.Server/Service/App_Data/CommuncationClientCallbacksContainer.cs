using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ServiceModel.Channels;
using System.Collections;
using System.Collections.Generic;
using DataCollector.Server.DeviceHandlers.Models;
using DataCollector.Server.Interfaces.Communication;

namespace DataCollector.Server
{
    /// <summary>
    /// Klasa obsługująca kontener klientów subskrybujących callback <see cref="ICommunicationServiceCallback"/>.
    /// </summary>
    public class CommunicationClientCallbacksContainer : ICommunicationClientCallbacksContainer
    {
        #region Private Fields
        private ConcurrentDictionary<string, ICommunicationServiceCallback> callbacks;
        #endregion

        #region Public Properties
        /// <summary>
        /// Proxy aktualnie podłączonego klienta.
        /// </summary>
        public ICommunicationServiceCallback CurrentClient
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ICommunicationServiceCallback>();
            }
        }
        /// <summary>
        /// Identyfikator aktualnego klienta.
        /// </summary>
        public string CurrentClientId
        {
            get
            {
                return OperationContext.Current.SessionId;
            }
        }
        /// <summary>
        /// Lista podłączonych klientów.
        /// </summary>
        public IEnumerable<ICommunicationServiceCallback> Clients
        {
            get
            {
                return callbacks.Select(s => s.Value);
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Tworzy nową instancję klasy.
        /// </summary>
        public CommunicationClientCallbacksContainer()
        {
            callbacks = new ConcurrentDictionary<string, ICommunicationServiceCallback>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Usuwa klienta z subskrypcji zdarzeń serwisu.
        /// </summary>
        /// <param name="sessionId">id</param>
        public void DeleteCallbackChannel(string sessionId)
        {
            ICommunicationServiceCallback callback = null;
            callbacks.TryRemove(sessionId, out callback);
        }
        /// <summary>
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        /// <param name="serviceCallback">callback</param>
        /// <param name="sessionId">id</param>
        public void RegisterCallbackChannel(string sessionId, ICommunicationServiceCallback serviceCallback)
            => callbacks.TryAdd(sessionId, serviceCallback);
        /// <summary>
        /// Powiadomienie o aktualizacji stanu urządzenia.
        /// </summary>
        /// <param name="deviceUpdated">dane dot. aktualizacji</param>
        public void OnDeviceChangedState(DeviceUpdatedEventArgs deviceUpdated) 
            => NotifyCallbackSubscribers(s => s.DeviceChangedState(deviceUpdated));
        /// <summary>
        /// Powiadomienie o nadejściu pomiarów.
        /// </summary>
        /// <param name="measures">pomiary</param>
        public void OnMeasuresArrived(MeasuresArrivedEventArgs measures) 
             => NotifyCallbackSubscribers(s => s.MeasuresArrived(measures));
        #endregion

        #region Private Methods
        /// <summary>
        /// Wysyłą wiadomość do klientów.
        /// </summary>
        /// <param name="data">wiadomość</param>
        private void NotifyCallbackSubscribers(Action<ICommunicationServiceCallback> data)
        {
            //usuń nieaktywnych
            var notActiveClients = callbacks
                .Where(s => s.Value is IChannel channel && channel.State != CommunicationState.Opened)
                .Select(s => s.Key).ToList();
            ICommunicationServiceCallback deletedCallback = null;
            foreach (var item in notActiveClients)
                callbacks.TryRemove(item, out deletedCallback);
            //wyslij wiadomość
            foreach (var client in callbacks)
                data(client.Value);
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Czyści zajęte zasoby.
        /// </summary>
        public void Dispose()
        {
            callbacks.Clear();
        }
        #endregion
    }
}