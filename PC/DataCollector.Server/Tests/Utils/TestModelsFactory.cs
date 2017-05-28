using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DeviceHandlers.Adapters;
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
        /// Zwraca testowe dane połączeniowe do bazy danych.
        /// </summary>
        /// <returns></returns>
        public const string TestConnectionString = "Data source=.\\DCInstance;Initial Catalog=DataCollectorDbTests;User ID=sa;Password=p@ssw0rd;";

        /// <summary>
        /// Tworzy instancję klasy <see cref="DeviceBroadcastInfo"/>.
        /// </summary>
        /// <returns></returns>
        public static IDeviceBroadcastInfo CreateDeviceBroadcastInfoMock()
        {
            return new DeviceBroadcastInfo("Raspberry Pi 3", IPAddress.Parse("127.0.0.1"), "AA:AA:AA:AA:AA:AA", "ARM", "10.586", "ARM");
        }

        /// <summary>
        /// Tworzy nową instancję konfiguracji urządzenia komunikacyjnego w oparciu o protokół REST <see cref="RestDeviceHandlerConfiguration"/> 
        /// <seealso cref="RestDeviceHandler"/>.
        /// </summary>
        /// <returns></returns>
        public static RestDeviceHandlerConfiguration CreateRestDeviceConfigMock()
        {
            return new RestDeviceHandlerConfiguration()
            {
                GetMeasurementsRequest = "/api/measurements",
                LedChangeRequest = "/api/ledState?p={0}",
                LedStateRequest = "/api/getLedState",
            };
        }
    }
}
