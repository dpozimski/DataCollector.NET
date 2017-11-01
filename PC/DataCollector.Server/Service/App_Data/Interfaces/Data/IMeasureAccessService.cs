using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DataAccess.Models.Entities;
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
        /// <param name="macAddress">adres MAC urządzenia</param>
        /// <param name="requestInterval">interwal rejestracji</param>
        [OperationContract]
        void UpdateDeviceRequestInterval(string macAddress, double requestInterval);
        /// <summary>
        /// Pobiera dostępne urządzenia pomiarowe z bazy danych.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<MeasureDevice> GetMeasureDevices();
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">zakres dolny</param>
        /// <param name="upperRange">zakres górny</param>
        /// <returns>punkty pomiarowe [X]</returns>
        [OperationContract]
        List<DateTimePoint[]> GetMeasures(MeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange);
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">od</param>
        /// <param name="upperRange">do</param>
        /// <returns>punkty pomiarowe [X,Y,Z]</returns>
        [OperationContract]
        List<DateTimePoint[]> GetSphereMeasures(SphereMeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange);
        #endregion
    }
}
