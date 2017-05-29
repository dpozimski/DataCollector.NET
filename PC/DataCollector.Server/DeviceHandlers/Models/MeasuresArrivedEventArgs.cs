using AutoMapper;
using DataCollector.Device.Models;
using DataCollector.Server.DataAccess.Models;
using System;
using System.Runtime.Serialization;

namespace DataCollector.Server.DeviceHandlers.Models
{
    /// <summary>
    /// Klasa reprezentująca parametry zdarzenia przyjścia wyników pomiarowych,
    /// </summary>
    [DataContract]
    public sealed class MeasuresArrivedEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// Źródło danych.
        /// </summary>
        [DataMember]
        public MeasureDevice Source { get; private set; }
        /// <summary>
        /// Wartość.
        /// </summary>
        [DataMember]
        public Measures Value { get; private set; }
        /// <summary>
        /// Odcisk czasu.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Wewnętrzny konstruktor klasy MeasuresArrivedEventArgs.
        /// </summary>
        /// <param name="source">źródło pomiarów</param>
        /// <param name="value">wartość</param>
        public MeasuresArrivedEventArgs(MeasureDevice source, Measures value, DateTime timeStamp)
        {
            this.Source = source;
            this.Value = value;
            this.TimeStamp = timeStamp;
        }
        #endregion
    }
}
