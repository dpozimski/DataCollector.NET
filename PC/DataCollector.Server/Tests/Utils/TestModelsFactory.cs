using DataCollector.Server.DataFlow.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Tests.Utils
{
    /// <summary>
    /// Klasa zawierająca metody tworzące modele danych.
    /// </summary>
    public static class TestModelsFactory
    {
        /// <summary>
        /// Tworzy instancję klasy <see cref="DeviceBroadcastInfo"/>.
        /// </summary>
        /// <returns></returns>
        public static DeviceBroadcastInfo CreateDeviceBroadcastInfoMock()
        {
            return new DeviceBroadcastInfo("Raspberry Pi 3", IPAddress.Parse("192.168.101.101"), "AA:AA:AA:AA:AA:AA", "ARM", "10.586", "ARM");
        }
    }
}
