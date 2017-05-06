using DataCollector.Device.Models;
using DataCollector.Server.Interfaces;
using System;

namespace DataCollector.Server.Models
{
    /// <summary>
    /// Klasa reprezentująca parametry zdarzenia przyjścia wyników pomiarowych,
    /// </summary>
    public sealed class MeasuresArrivedEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// Źródło danych.
        /// </summary>
        public IDeviceInfo Source { get; private set; }
        /// <summary>
        /// Wartość.
        /// </summary>
        public Measures Value { get; private set; }
        /// <summary>
        /// Odcisk czasu.
        /// </summary>
        public DateTime TimeStamp { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Wewnętrzny konstruktor klasy MeasuresArrivedEventArgs.
        /// </summary>
        /// <param name="source">źródło pomiarów</param>
        /// <param name="value">wartość</param>
        internal MeasuresArrivedEventArgs(IDeviceInfo source, Measures value, DateTime timeStamp)
        {
            this.Source = source;
            this.Value = value;
            this.TimeStamp = timeStamp;
        }
        #endregion
    }
}
