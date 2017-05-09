﻿using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.Handlers.Interfaces
{
    /// <summary>
    /// Interfejs opisujący podstawowe właściwości urządzenia.
    /// </summary>
    public interface IDeviceInfo : IDeviceBroadcastInfo
    {
        #region Public Properties
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        double MeasurementsMsRequestInterval { get; set; }
        #endregion
    }
}