using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Obsługa zdarzeń zwrotnych z serwisu <see cref=">ICommunicationService"/>
    /// </summary>
    public class CommunicationServiceCallback : ICommunicationServiceCallback, ICommunicationServiceEventCallback
    {
        #region Events
        /// <summary>
        /// Zdarzenie nadejścia aktualizacji stanu urządzenia.
        /// </summary>
        public event EventHandler<DeviceUpdatedEventArgs> DeviceChangedStateEvent;
        /// <summary>
        /// Zdarzenie nadejścia pomiarów z urządzenia.
        /// </summary>
        public event EventHandler<MeasuresArrivedEventArgs> MeasuresArrivedEvent;
        #endregion

        #region ICommunicationServiceCallback
        /// <summary>
        /// Zdarzenie z serwisu o nadejsciu aktualizacji stanu urządzenia.
        /// </summary>
        /// <param name="deviceUpdated">stan</param>
        public void DeviceChangedState(DeviceUpdatedEventArgs deviceUpdated)
        {
            DeviceChangedStateEvent?.Invoke(this, deviceUpdated);
        }
        /// <summary>
        /// Zdarzenie z serwisu o nadejsciu pomiarów z urządzenia.
        /// </summary>
        /// <param name="measures">urządzenie</param>
        public void MeasuresArrived(MeasuresArrivedEventArgs measures)
        {
            MeasuresArrivedEvent?.Invoke(this, measures);
        }
        #endregion
    }
}
