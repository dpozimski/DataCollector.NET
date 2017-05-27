using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Interfaces
{
    /// <summary>
    /// Interfejs określający metody sterowania usługą umieszczania danych w bazie danych.
    /// </summary>
    public interface IMeasureCollector : IDataAccessBase, IDisposable
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
        void StartCollectingData();
        /// <summary>
        /// Zakończenie kolekcjonowania danych z warstwy komunikacyjnej.
        /// </summary>
        void StopCollectingData();
        #endregion
    }
}
