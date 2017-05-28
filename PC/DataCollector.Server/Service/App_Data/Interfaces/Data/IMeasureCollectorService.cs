using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces.Data
{
    /// <summary>
    /// Interfejs określający metody sterowania usługą umieszczania danych w bazie danych.
    /// </summary>
    [ServiceContract]
    public interface IMeasureCollectorService : IDataAccessBase, IDisposable
    {
        #region Properties
        /// <summary>
        /// Kolekcjonowanie danych jest aktywne.
        /// </summary>
        bool IsCollectingDataEnabled { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Rozpoczęcie kolekcjonowania danych z warstwy komunikacyjnej.
        /// </summary>
        [OperationContract]
        void StartCollectingData();
        /// <summary>
        /// Zakończenie kolekcjonowania danych z warstwy komunikacyjnej.
        /// </summary>
        [OperationContract]
        void StopCollectingData();
        #endregion
    }
}
