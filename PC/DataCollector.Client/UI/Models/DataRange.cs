using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Klasa opisująca zakres danych pomiarowych.
    /// </summary>
    public class DataRange : ReactiveObject
    {
        #region Private Fields
        private DateTime firstStamp, lastStamp;
        #endregion

        #region Public Properties
        /// <summary>
        /// Początkowy znacznik czasu.
        /// </summary>
        public DateTime FirstStamp
        {
            get { return firstStamp; }
            set { this.RaiseAndSetIfChanged(ref firstStamp, value); }
        }
        /// <summary>
        /// Ostatni znacznik czasu.
        /// </summary>
        public DateTime LastStamp
        {
            get { return lastStamp; }
            set { this.RaiseAndSetIfChanged(ref lastStamp, value); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Aktualizuje dane w podanym modelu.
        /// </summary>
        /// <param name="from">początek zakresu danych</param>
        /// <param name="to">ostatni znacznik zakresu danych</param>
        public void Update(DateTime from, DateTime to)
        {
            this.FirstStamp = from;
            this.LastStamp = to;
        }
        #endregion
    }
}
