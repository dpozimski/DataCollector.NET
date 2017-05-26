using DataCollector.Server.Models;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces
{
	public interface ICommunicationClientCallbacksContainer : IDisposable
    {
        /// <summary>
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        void RegisterCallbackChannel(ICommunicationServiceCallback serviceCallback);
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
    }
}