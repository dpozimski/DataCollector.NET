using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Models
{
    /// <summary>
    /// Argumenty zdarzenia aktualizacji urządzenia pomiarowego.
    /// </summary>
    public sealed class DeviceUpdatedEventArgs
    {
        #region Public Properties
        /// <summary>
        /// Urządzenie.
        /// </summary>
        public IDeviceInfo Device { get; }
        /// <summary>
        /// Status aktualizacji.
        /// </summary>
        public UpdateStatus UpdateStatus { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DeviceUpdatedEventArgs.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <param name="updateStatus">status</param>
        internal DeviceUpdatedEventArgs(IDeviceHandler device, UpdateStatus updateStatus)
        {
            Device = device;
            UpdateStatus = updateStatus;
        }
        #endregion
    }
}
