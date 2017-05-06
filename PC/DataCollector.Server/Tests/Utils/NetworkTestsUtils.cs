using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.Tests.Utils
{
    /// <summary>
    /// Klasa statyczna zawierająca przydatne metody testujące w szczególności aspekty sieciowe.
    /// </summary>
    public static class NetworkTestsUtils
    {
        public static readonly IPAddress MulticastAddress = IPAddress.Parse("239.0.0.222");
        public static readonly IPAddress Localhost = IPAddress.Parse("127.0.0.1");
        public static readonly int Port = 8;

        public static Socket CreateLocalhostMultiCastSocket()
        {
            return CreateMultiCastSocket(Localhost);
        }

        public static Socket CreateMultiCastSocket(IPAddress ip)
        {
            UdpClient client = new UdpClient(Port);
            EndPoint endPoint = new IPEndPoint(ip, Port);
            client.Client.Connect(endPoint);
            return client.Client;
        }
    }
}
