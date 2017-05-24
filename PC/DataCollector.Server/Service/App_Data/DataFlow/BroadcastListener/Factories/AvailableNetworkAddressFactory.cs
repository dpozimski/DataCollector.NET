using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DataCollector.Server.DataFlow.BroadcastListener.Factories
{
    /// <summary>
    /// Klasa będąca fabryką adresów sieciowych dostępnych w systemie.
    /// </summary>
    public class AvailableNetworkAddressFactory : INetworkAddressFactory
    {
        /// <summary>
        /// Zwraca adresy IP lokalnych zewnętrznych interfejsów dostępnych dla użytkownika
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPAddress> Create()
        {
            return GetIpAddresses();
        }

        /// <summary>
        /// Zwraca liste wszystkich możliwych adresów IP, na których możliwy jest nasłuch gniazda broadcast.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IPAddress> GetIpAddresses()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(networkInterface =>
                    networkInterface.Supports(NetworkInterfaceComponent.IPv4) &&
                    networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                .Where(unicastIpAddressInformation => unicastIpAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(unicastIpAddressInformation => unicastIpAddressInformation.Address);
        }
    }
}
