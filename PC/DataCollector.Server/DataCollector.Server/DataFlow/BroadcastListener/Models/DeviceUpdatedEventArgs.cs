using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Models
{
    /// <summary>
    /// Argumenty zdarzenia wykrycia urządzenia w sieci.
    /// </summary>
    public class DeviceUpdatedEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// Zaktualizowane dane o urządzeniu.
        /// </summary>
        public DeviceBroadcastInfo DeviceInfo { get; }
        /// <summary>
        /// Status aktualizacji.
        /// </summary>
        public UpdateStatus UpdateStatus { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DeviceUpdatedEventArgs.
        /// </summary>
        /// <param name="deviceInfo">urządzenie</param>
        /// <param name="updateStatus">status</param>
        internal DeviceUpdatedEventArgs(DeviceBroadcastInfo deviceInfo, UpdateStatus updateStatus)
        {
            DeviceInfo = deviceInfo;
            UpdateStatus = updateStatus;
        }
        #endregion

    }
}
