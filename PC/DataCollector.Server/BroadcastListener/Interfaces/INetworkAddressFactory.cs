using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener.Interfaces
{
    /// <summary>
    /// Interfejs opisujący metody wytwarzania adresów sieciowych.
    /// </summary>
    public interface INetworkAddressFactory
    {
        /// <summary>
        /// Metoda zwracająca adresy sieciowe.
        /// </summary>
        /// <returns>kolekcja adresów sieciowych</returns>
        IEnumerable<IPAddress> Create();
    }
}
