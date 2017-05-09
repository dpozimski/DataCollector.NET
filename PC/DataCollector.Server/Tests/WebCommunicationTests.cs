using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using NSubstitute;
using System;
using Xunit;

namespace DataCollector.Server.Tests
{
    /// <summary>
    /// Klasa testująca <see cref="WebCommunication"/>
    /// </summary>
    public class WebCommunicationTests
    {
        private IBroadcastScanner broadcastScanner;
        private IDeviceHandlerFactory deviceHandlerFactory;
        private ICommunication webCommunication;
        private int port;

        public WebCommunicationTests()
        {
            port = 41352;
            broadcastScanner = Substitute.For<IBroadcastScanner>();
            deviceHandlerFactory = Substitute.For<IDeviceHandlerFactory>();

            webCommunication = new WebCommunication(broadcastScanner, deviceHandlerFactory, port);
        }

        [Fact]
        public void IsConnectedPropertyWhileCommunicationStartedTest()
        {
            webCommunication.Start();
            Assert.True(webCommunication.IsStarted);
        }

        [Fact]
        public void IsConnectedPropertyWhileCommunicationStoppedTest()
        {
            webCommunication.Start();
            webCommunication.Stop();
            Assert.False(webCommunication.IsStarted);
        }

        [Fact]
        public void PreventFromDoubleStartCommunicationTest()
        {
            webCommunication.Start();
            Assert.Throws<InvalidOperationException>(() => webCommunication.Start());
        }

        [Fact]
        public void PreventFromInvalidStopCommunicationTest()
        {
            Assert.Throws<InvalidOperationException>(() => webCommunication.Stop());
        }
    }
}
