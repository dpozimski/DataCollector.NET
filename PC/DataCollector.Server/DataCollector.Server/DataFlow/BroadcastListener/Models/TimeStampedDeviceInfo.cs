using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Models
{
    /// <summary>
    /// Klasa reprezentująca znacznik czasu nadejścia obiektu Device.
    /// </summary>
    public class TimestampedDeviceInfo : DeviceBroadcastInfo
    {
        #region Public Properties
        /// <summary>
        /// Znacznik czasowy.
        /// </summary>
        public DateTime LastUpdate { get; private set; }
        /// <summary>
        /// Obiekt z informacją utracił termin ważności.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                DateTime timeoutAt = DateTime.Now.AddSeconds(-10);
                return LastUpdate < timeoutAt;
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy TimestampedDeviceInfo.
        /// </summary>
        /// <param name="info">dane urządzenia</param>
        public TimestampedDeviceInfo(DeviceBroadcastInfo info):base(info)
        {
            LastUpdate = DateTime.Now;
        }
        #endregion
    }
}
