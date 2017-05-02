using DataCollector.Device.Data;
using DataCollector.Device.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.Controller
{
    /// <summary>
    /// Klasa będąca uchwytem do aktualizowanych danych pomiarowych urządzenia.
    /// </summary>
    public sealed class JsonMeasuresDataController : IMeasuresDataController
    {
        #region Private Fields
        private Measures measures;
        private string jsonMeasures;
        private readonly object syncObject = new object();
        #endregion

        /// <summary>
        /// Uchwyt do najnowszych pomiarów.
        /// </summary>
        public Measures NewestMeasure
        {
            get
            {
                lock (syncObject)
                    return measures;
            }
            set
            {
                lock (syncObject)
                {
                    measures = value;
                    jsonMeasures = JsonConvert.SerializeObject(measures);
                }
                   
            }
        }

        /// <summary>
        /// Zwraca pomiary w formacie JSON.
        /// </summary>
        public string NewestMeasureOutput
        {
            get
            {
                lock (syncObject)
                    return jsonMeasures;
            }
        }
    }
}
