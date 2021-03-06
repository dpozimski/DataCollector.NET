﻿using AutoMapper;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DataAccess.Models.Entities;
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
        public MeasureDevice Device { get; set; }
        /// <summary>
        /// Status aktualizacji.
        /// </summary>
        [DataMember]
        public UpdateStatus UpdateStatus { get; set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DeviceUpdatedEventArgs.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <param name="updateStatus">status</param>
        public DeviceUpdatedEventArgs(MeasureDevice device, UpdateStatus updateStatus)
        {
            Device = device;
            UpdateStatus = updateStatus;
        }
        #endregion
    }
}
