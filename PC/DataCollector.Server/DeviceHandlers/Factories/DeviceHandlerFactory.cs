using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DeviceHandlers.Adapters;
using DataCollector.Server.DeviceHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers.Factories
{
    /// <summary>
    /// Klasa stanowiąca 
    /// </summary>
    public class DeviceHandlerFactory : IDeviceHandlerFactory
    {
        #region Fields
        private int simulatorDevicesCount;
        private IRestConnectionAdapterFactory restConnectionAdapterFactory;
        private IDeviceHandlerConfiguration configuration;
        #endregion

        #region ctor
        /// <summary>
        /// Tworzy nową instancję fabryki <see cref="IDeviceHandler"/>
        /// </summary>
        /// <param name="restConnectionAdapterFactory">fabryka adapterów komunikacji REST</param>
        /// <param name="configuration">konfiguracja protokołu REST</param>
        public DeviceHandlerFactory(IRestConnectionAdapterFactory restConnectionAdapterFactory, IDeviceHandlerConfiguration configuration)
        {
            this.configuration = configuration;
            this.restConnectionAdapterFactory = restConnectionAdapterFactory;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Metoda tworząca symulator obiektu pomiarowego.
        /// </summary>
        /// <returns>referencja interfejsu wykonawczego</returns>
        public IDeviceHandler CreateSimulatorDevice()
        {
            simulatorDevicesCount++;
            IDeviceBroadcastInfo fakeBroadcastInfo = new DeviceBroadcastInfo($"Rpi Simulator {simulatorDevicesCount}", 
                IPAddress.Parse($"192.168.110.{simulatorDevicesCount}"), 
                $"11:11:33:44:{simulatorDevicesCount.ToString("X2")}", "ARM", "10.0.14393.00", "Raspberry Pi 2 Model B");
            return new SimulatorDeviceHandler(fakeBroadcastInfo);
        }
        /// <summary>
        /// Metoda tworząca urządzenie pomiarowe na bazie dostarczonych informacji.
        /// </summary>
        /// <param name="broadcastInfo">informacje połączeniowe</param>
        /// <param name="port">port połączeniowy</param>
        /// <returns>referencja interfejsu wykonawczego</returns>
        public IDeviceHandler CreateRestDevice(IDeviceBroadcastInfo broadcastInfo, int port)
        {
            if (broadcastInfo is null)
                throw new ArgumentNullException($"{nameof(broadcastInfo)} cannot be null.");
            if (port <= 0 || port > 65536)
                throw new ArgumentException("Port must be a valid value between 0 and 65536.");

            var restConnectionAdapter = restConnectionAdapterFactory.Create(broadcastInfo.IPv4, port);
            return new RestDeviceHandler(restConnectionAdapter, configuration, broadcastInfo);
        }
        #endregion
    }
}
