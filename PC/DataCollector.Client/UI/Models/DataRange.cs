using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.Models
{
    /// <summary>
    /// Class which describes a date range.
    /// </summary>
    public class DataRange : ReactiveObject
    {
        #region Private Fields
        private DateTime firstStamp, lastStamp;
        #endregion

        #region Public Properties
        /// <summary>
        /// The first stamp.
        /// </summary>
        public DateTime FirstStamp
        {
            get { return firstStamp; }
            set { this.RaiseAndSetIfChanged(ref firstStamp, value); }
        }
        /// <summary>
        /// The last stamp.
        /// </summary>
        public DateTime LastStamp
        {
            get { return lastStamp; }
            set { this.RaiseAndSetIfChanged(ref lastStamp, value); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="from">the start stamp</param>
        /// <param name="to">the last stamp</param>
        public void Update(DateTime from, DateTime to)
        {
            this.FirstStamp = from;
            this.LastStamp = to;
        }
        #endregion
    }
}
