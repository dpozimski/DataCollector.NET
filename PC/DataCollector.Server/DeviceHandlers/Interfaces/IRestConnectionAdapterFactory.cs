using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers.Interfaces
{
    /// <summary>
    /// Interfejs zawierajacy metody kreacyjne adapterów połączeniowych.
    /// </summary>
    public interface IRestConnectionAdapterFactory
    {
        /// <summary>
        /// Tworzy nową instancję adaptera połączeniowego bazującego na protokole REST.
        /// </summary>
        /// <param name="ip">docelowy IP</param>
        /// <param name="port">docelowy port</param>
        /// <returns>Adapter połączeniowy</returns>
        IRestConnectionAdapter Create(IPAddress ip, int port);
    }
}
