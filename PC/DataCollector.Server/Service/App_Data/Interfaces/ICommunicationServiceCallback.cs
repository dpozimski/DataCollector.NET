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
    /// Interfejs subskrypcji danych zwrotnych z serwisu.
    /// </summary>
    [ServiceContract]
    public interface ICommunicationServiceCallback
    {
        /// <summary>
        /// Aktualne pomiary z pochodzące z urządzenia.
        /// <paramref name="measures">pomiary z urządzenia</paramref>
        /// </summary>
        [OperationContract(IsOneWay = true)]
        Task MeasuresArrived(MeasuresArrivedEventArgs measures);
        /// <summary>
        /// Wykryto urządzenie w sieci.
        /// <paramref name="deviceUpdated">dane dot. aktualizacji stanu urządzenia</paramref>
        /// </summary>
        [OperationContract(IsOneWay = true)]
        Task DeviceChangedState(DeviceUpdatedEventArgs deviceUpdated);
    }
}