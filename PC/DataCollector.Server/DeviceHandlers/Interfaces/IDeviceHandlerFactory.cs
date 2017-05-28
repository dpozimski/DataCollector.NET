using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metody tworzące reprezentację urządzeń pomiarowych.
    /// </summary>
    public interface IDeviceHandlerFactory
    {
        /// <summary>
        /// Metoda tworząca symulator obiektu pomiarowego.
        /// </summary>
        /// <returns>referencja interfejsu wykonawczego</returns>
        IDeviceHandler CreateSimulatorDevice();
        /// <summary>
        /// Metoda tworząca urządzenie pomiarowe na bazie dostarczonych informacji.
        /// </summary>
        /// <param name="broadcastInfo">informacje połączeniowe</param>
        /// <param name="port">port połączeniowy</param>
        /// <returns>referencja interfejsu wykonawczego</returns>
        IDeviceHandler CreateRestDevice(IDeviceBroadcastInfo broadcastInfo, int port);
    }
}
