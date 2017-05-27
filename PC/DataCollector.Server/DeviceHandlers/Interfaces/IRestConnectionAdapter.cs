using DataCollector.Device.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers.Interfaces
{
    /// <summary>
    /// Interfejs definiujący funkcjonalność adaptera komunikacji po protokole REST.
    /// </summary>
    public interface IRestConnectionAdapter
    {
        /// <summary>
        /// Metoda nawiązująca komunikację
        /// </summary>
        void Connect();
        /// <summary>
        /// Metoda rozłączająca połączenie.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Metoda wykonująca zapytanie GET.
        /// </summary>
        /// <param name="restRequest">zapytanie</param>
        /// <returns>odpowiedź</returns>
        string GetRequest(string restRequest);
    }
}
