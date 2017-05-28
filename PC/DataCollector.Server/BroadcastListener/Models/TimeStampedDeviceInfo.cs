using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener.Models
{
    /// <summary>
    /// Klasa reprezentująca znacznik czasu nadejścia obiektu Device.
    /// </summary>
    public class TimestampedDeviceInfo
    {
        #region Private Fields
        /// <summary>
        /// Czas ważności odcisku urządzenia.
        /// </summary>
        private readonly TimeSpan expirationInterval;
        #endregion

        #region Public Properties
        /// <summary>
        /// Informacje o urządzeniu.
        /// </summary>
        public IDeviceBroadcastInfo Info { get; set; }
        /// <summary>
        /// Znacznik czasowy.
        /// </summary>
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// Obiekt z informacją utracił termin ważności.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                DateTime timeoutAt = DateTime.Now - expirationInterval;
                return LastUpdate < timeoutAt;
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy TimestampedDeviceInfo.
        /// </summary>
        /// <param name="expirationInterval">czas ważności instancji obiektu</param>
        /// <param name="info">dane urządzenia</param>
        public TimestampedDeviceInfo(IDeviceBroadcastInfo info, TimeSpan expirationInterval)
        {
            this.expirationInterval = expirationInterval;
            this.Info = info;
            LastUpdate = DateTime.Now;
        }
        #endregion
    }
}
