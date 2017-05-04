using DataCollector.Server.DataFlow.BroadcastListener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener
{
    public class BroadcastInterfaceMessageHandlerTests : IDisposable
    {
        private const string testId = "TestMultiCastByteReceived";
        private readonly IPAddress localhost;
        private readonly IPAddress multicastAddress;
        private readonly int port;
        private readonly BroadcastInterfaceMessageHandler deviceListener;
        private Socket testSocket;
        private IPEndPoint endPoint;

        public BroadcastInterfaceMessageHandlerTests()
        {
            multicastAddress = IPAddress.Parse("239.0.0.222");
            localhost = IPAddress.Parse("127.0.0.1");
            port = 8;
            deviceListener = new BroadcastInterfaceMessageHandler(localhost, multicastAddress, port);
            testSocket = CreateMultiCastSocket();
            endPoint = new IPEndPoint(localhost, port);
        }

        private Socket CreateMultiCastSocket()
        {
            IPEndPoint endPoint = new IPEndPoint(localhost, port);
            UdpClient client = new UdpClient(port);
            return client.Client;
        }

        [Fact]
        public void TestMultiCastByteReceived()
        {
            string loopbackReturn = null;
            deviceListener.StartListening();
            deviceListener.OnReceivedBytes += (o, e) =>
                loopbackReturn = Encoding.ASCII.GetString(e);

            testSocket.Connect(endPoint);
            testSocket.Send(Encoding.ASCII.GetBytes(testId));

            Thread.Sleep(10);

            Assert.Equal(loopbackReturn, testId);
        }

        public void Dispose()
        {
            deviceListener.Dispose();
            testSocket.Dispose();
        }
    }
}
