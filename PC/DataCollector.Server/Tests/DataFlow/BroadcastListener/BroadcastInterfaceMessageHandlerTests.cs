using DataCollector.Server.DataFlow.BroadcastListener;
using DataCollector.Server.Tests.Utils;
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
        private readonly BroadcastInterfaceMessageHandler deviceListener;
        private Socket testSocket;

        public BroadcastInterfaceMessageHandlerTests()
        {
            deviceListener = new BroadcastInterfaceMessageHandler(NetworkTestsUtils.Localhost, NetworkTestsUtils.MulticastAddress, NetworkTestsUtils.Port);
            testSocket = NetworkTestsUtils.CreateLocalhostMultiCastSocket();
        }

        [Fact]
        public void TestMultiCastByteReceived()
        {
            string loopbackReturn = null;
            deviceListener.StartListening();
            deviceListener.OnReceivedBytes += (o, e) =>
                loopbackReturn = Encoding.ASCII.GetString(e);

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
