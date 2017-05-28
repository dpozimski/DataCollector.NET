using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Interfaces.Data
{
    /// <summary>
    /// Interfejs dostępu do danych tabelarycznych.
    /// </summary>
    [ServiceContract]
    public interface IMeasureAccessService : IDataAccessBase
    {
        #region Methods
        /// <summary>
        /// Aktualizuje urządzenie pomiarowe.
        /// </summary>
        /// <param name="deviceHandler">urządzenie pomiarowe</param>
        [OperationContract]
        void UpdateMeasureDevice(IDeviceInfo deviceHandler);
        /// <summary>
        /// Pobiera dostępne urządzenia pomiarowe z bazy danych.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IReadOnlyList<MeasureDevice> GetMeasureDevices();
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">zakres dolny</param>
        /// <param name="upperRange">zakres górny</param>
        /// <returns>punkty pomiarowe [X]</returns>
        [OperationContract]
        IEnumerable<DateTimePoint[]> GetMeasures(MeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange);
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">od</param>
        /// <param name="upperRange">do</param>
        /// <returns>punkty pomiarowe [X,Y,Z]</returns>
        [OperationContract]
        IEnumerable<DateTimePoint[]> GetMeasures(SphereMeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange);
        #endregion
    }
}
