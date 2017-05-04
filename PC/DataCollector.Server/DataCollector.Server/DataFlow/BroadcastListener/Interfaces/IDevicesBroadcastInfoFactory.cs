﻿using DataCollector.Server.DataFlow.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener.Interfaces
{
    /// <summary>
    /// Fabryka obiektów identyfikujących urządzenia w sieci.
    /// </summary>
    public interface IDevicesBroadcastInfoFactory
    {
        /// <summary>
        /// Metoda parsująca bufor wejściowy do obiektu Device.
        /// </summary>
        /// <param name="buffer">bufor wejściowy</param>
        /// <param name="device">informacje o urządzeniu</param>
        /// <returns>obiekt definiujący powstałą ramkę</returns>
        DeviceBroadcastInfo From(byte[] buffer);
    }
}