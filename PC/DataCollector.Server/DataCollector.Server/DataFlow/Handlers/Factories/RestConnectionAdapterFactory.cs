﻿using DataCollector.Server.DataFlow.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DataCollector.Server.DataFlow.Handlers.Adapters;

namespace DataCollector.Server.DataFlow.Handlers.Factories
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
            return new RestSharpLibConnectionAdapter(ip, port);
        }
        #endregion
    }
}
