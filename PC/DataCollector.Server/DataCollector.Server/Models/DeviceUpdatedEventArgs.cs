using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Models
{
    /// <summary>
    /// Argumenty zdarzenia aktualizacji urządzenia pomiarowego.
    /// </summary>
    [DataContract]
    public sealed class DeviceUpdatedEventArgs
    {
        #region Public Properties
        /// <summary>
        /// Urządzenie.
        /// </summary>
        [DataMember]
        public IDeviceInfo Device { get; }
        /// <summary>
        /// Status aktualizacji.
        /// </summary>
        [DataMember]
        public UpdateStatus UpdateStatus { get; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DeviceUpdatedEventArgs.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <param name="updateStatus">status</param>
        public DeviceUpdatedEventArgs(IDeviceHandler device, UpdateStatus updateStatus)
        {
            Device = device;
            UpdateStatus = updateStatus;
        }
        #endregion
    }
}
