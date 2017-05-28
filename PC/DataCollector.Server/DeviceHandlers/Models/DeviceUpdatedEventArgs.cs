﻿using AutoMapper;
using DataCollector.Server.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers.Models
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
        public DeviceInfo Device { get; }
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
        public DeviceUpdatedEventArgs(DeviceInfo device, UpdateStatus updateStatus)
        {
            Device = device;
            UpdateStatus = updateStatus;
        }
        #endregion
    }
}