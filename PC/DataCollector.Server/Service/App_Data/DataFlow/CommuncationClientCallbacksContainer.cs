﻿using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ServiceModel.Channels;

namespace DataCollector.Server.DataFlow
{
    /// <summary>
    /// Klasa obsługująca kontener klientów subskrybujących callback <see cref="ICommunicationServiceCallback"/>.
    /// </summary>
    public class CommunicationClientCallbacksContainer : ICommunicationClientCallbacksContainer
    {
        #region Private Fields
        private ConcurrentDictionary<string, ICommunicationServiceCallback> callbacks;
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
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        public void RegisterCallbackChannel(ICommunicationServiceCallback serviceCallback)
        {
            callbacks.TryAdd(OperationContext.Current.SessionId, serviceCallback);
        }
        /// <summary>
        /// Powiadomienie o aktualizacji stanu urządzenia.
        /// </summary>
        /// <param name="deviceUpdated">dane dot. aktualizacji</param>
        public void OnDeviceChangedState(Models.DeviceUpdatedEventArgs deviceUpdated) 
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
            var notActiveClients = callbacks.Where(s => ((IChannel)s.Value).State != CommunicationState.Opened)
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