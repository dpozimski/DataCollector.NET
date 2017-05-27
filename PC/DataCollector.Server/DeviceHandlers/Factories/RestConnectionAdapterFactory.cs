using DataCollector.Server.DeviceHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DataCollector.Server.DeviceHandlers.Adapters;

namespace DataCollector.Server.DeviceHandlers.Factories
{
    /// <summary>
    /// Fabryka adapterów połączeń protkołu REST.
    /// </summary>
    public class RestConnectionAdapterFactory : IRestConnectionAdapterFactory
    {
        #region Public Methods
        /// <summary>
        /// Tworzy nową instancję adaptera połączeniowego bazującego na protokole REST.
        /// </summary>
        /// <param name="ip">docelowy IP</param>
        /// <param name="port">docelowy port</param>
        /// <returns>Adapter połączeniowy</returns>
        public IRestConnectionAdapter Create(IPAddress ip, int port)
        {
            if (ip is null)
                throw new ArgumentNullException("IP");
            else if (port < 0 || port > 65536)
                throw new ArgumentException("Port must be greater than 0 and less than 65536.");

            return new RestSharpLibConnectionAdapter(ip, port);
        }
        #endregion
    }
}
