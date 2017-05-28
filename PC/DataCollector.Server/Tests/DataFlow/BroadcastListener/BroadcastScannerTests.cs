using DataCollector.Server.BroadcastListener;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.Tests.Utils;
using NSubstitute;
using System;
using System.Collections.Concurrent;
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
    /// <summary>
    /// Klasa testujaca <see cref="BroadcastScanner"/>
    /// </summary>
    public class BroadcastScannerTests : IDisposable
    {
        private Socket interfaceBroadcastSocket;
        private readonly ConcurrentDictionary<string, DeviceBroadcastInfo> broadcastInfoCollection;
        private readonly IBroadcastScanner broadcastScanner;
        private readonly IDetectedDevicesContainer devicesContainer;
        private readonly IDevicesBroadcastInfoFactory broadcastInfoFactory;
        private readonly INetworkAddressFactory addressFactory;

        public BroadcastScannerTests()
        {
            broadcastInfoCollection = new ConcurrentDictionary<string, DeviceBroadcastInfo>();

            devicesContainer = Substitute.For<IDetectedDevicesContainer>();
            devicesContainer.Devices.Returns(s => broadcastInfoCollection.Values);
            devicesContainer.When(s => s.Update(Arg.Any<DeviceBroadcastInfo>())).Do(s =>
            {
                broadcastInfoCollection.TryAdd(s.Arg<DeviceBroadcastInfo>().MacAddress, s.Arg<DeviceBroadcastInfo>());
            });

            broadcastInfoFactory = Substitute.For<IDevicesBroadcastInfoFactory>();
            broadcastInfoFactory.From(Arg.Any<byte[]>()).Returns<IDeviceBroadcastInfo>(s => 
                    TestModelsFactory.CreateDeviceBroadcastInfoMock());

            addressFactory = Substitute.For<INetworkAddressFactory>();
            addressFactory.Create().Returns(s => new List<IPAddress>() { NetworkTestsUtils.Localhost } );

            broadcastScanner = new BroadcastScanner(addressFactory, devicesContainer, broadcastInfoFactory);
            interfaceBroadcastSocket = NetworkTestsUtils.CreateLocalhostMultiCastSocket();
        }

        [Fact]
        public void FindDevicesOnAllInterfacesTest()
        {
            bool eventRaised = false;
            EventHandler<DeviceUpdatedEventArgs> eventHandler = (o, e) => eventRaised = true;
            broadcastScanner.StartListening();
            broadcastScanner.DeviceInfoUpdated += eventHandler;
            interfaceBroadcastSocket.Send(Properties.Resources.CorrectFrame);
            devicesContainer.DeviceInfoUpdated += Raise.EventWith(this, new DeviceUpdatedEventArgs(null, UpdateStatus.ConnectedToRestService));
            Thread.Sleep(20);
            broadcastScanner.DeviceInfoUpdated -= eventHandler;
            Assert.Equal(devicesContainer.Devices.Count(), 1);
            Assert.True(eventRaised);
        }

        public void Dispose()
        {
            broadcastScanner.Dispose();
            interfaceBroadcastSocket.Dispose();
        }
    }
}
