using AutoMapper;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DataAccess.Models.Entities;
using DataCollector.Server.DeviceHandlers.Interfaces;
using DataCollector.Server.DeviceHandlers.Models;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Interfaces.Communication;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace DataCollector.Server.Tests
{
    /// <summary>
    /// Klasa testująca <see cref="WebCommunicationService"/>
    /// </summary>
    public class WebCommunicationServiceTests : IDisposable
    {
        private MeasuresArrivedEventArgs measuresArrived;
        private DeviceHandlers.Models.DeviceUpdatedEventArgs deviceUpdated;
        private IDeviceHandler simulatorDevice;
        private IBroadcastScanner broadcastScanner;
        private IDeviceHandlerFactory deviceHandlerFactory;
        private ICommunicationServiceCallback callback;
        private ICommunicationClientCallbacksContainer callbackContainer;
        private WebCommunicationService webCommunication;
        private int port;
        private bool ledState;
        private bool isConnected;

        public WebCommunicationServiceTests()
        {
            SimulatorDeviceInit();
            port = 41352;
            broadcastScanner = Substitute.For<IBroadcastScanner>();
            deviceHandlerFactory = Substitute.For<IDeviceHandlerFactory>();
            deviceHandlerFactory.CreateSimulatorDevice().Returns(s => simulatorDevice);
            deviceHandlerFactory.CreateRestDevice(Arg.Any<IDeviceBroadcastInfo>(), Arg.Any<int>()).Returns(s => simulatorDevice);
            callbackContainer = Substitute.For<ICommunicationClientCallbacksContainer>();
            callbackContainer.When(s => s.OnMeasuresArrived(Arg.Any<MeasuresArrivedEventArgs>())).Do(s => measuresArrived = s.Arg<MeasuresArrivedEventArgs>());
            callbackContainer.When(s => s.OnDeviceChangedState(Arg.Any<DeviceHandlers.Models.DeviceUpdatedEventArgs>())).Do(s => deviceUpdated = s.Arg<DeviceHandlers.Models.DeviceUpdatedEventArgs>());
            callbackContainer.When(s => s.RegisterCallbackChannel(Arg.Any<string>(), Arg.Any<ICommunicationServiceCallback>())).Do(s => callback = s.Arg<ICommunicationServiceCallback>());

            webCommunication = new WebCommunicationService(broadcastScanner, deviceHandlerFactory, callbackContainer, port);
        }

        private void SimulatorDeviceInit()
        {
            this.simulatorDevice = Substitute.For<IDeviceHandler>();
            this.simulatorDevice.Connect().Returns(d =>
            {
                isConnected = true;
                return true;
            });
            this.simulatorDevice.Disconnect().Returns(d =>
            {
                isConnected = false;
                return true;
            });
            this.simulatorDevice.ChangeLedState(Arg.Any<bool>()).Returns(d =>
            {
                ledState = d.Arg<bool>();
                return ledState;
            });
            this.simulatorDevice.GetLedState().Returns(d => ledState);
            this.simulatorDevice.IsConnected.Returns(d => isConnected);
        }

        private void InitializeMapper()
        {
            //AutoMapper
            Mapper.Initialize(cfg => {
                cfg.CreateMap<IDeviceInfo, MeasureDevice>();
            });
        }

        private MeasureDevice GetConnectedDevice()
        {
            InitializeMapper();
            webCommunication.Start();
            webCommunication.AddSimulatorDevice();
            Assert.NotNull(deviceUpdated);
            return Mapper.Map<MeasureDevice>(deviceUpdated.Device);
        }

        [Fact]
        public void IsConnectedPropertyWhileCommunicationStartedTest()
        {
            webCommunication.Start();
            Assert.True(webCommunication.IsStarted());
        }

        [Fact]
        public void IsConnectedPropertyWhileCommunicationStoppedTest()
        {
            webCommunication.Start();
            webCommunication.Stop();
            Assert.False(webCommunication.IsStarted());
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

        [Fact]
        public void AddSimulatorTest()
        {
            webCommunication.Start();
            webCommunication.AddSimulatorDevice();
            Assert.NotNull(deviceUpdated);
        }

        [Fact]
        public void DeviceConnectTest()
        {
            MeasureDevice device = GetConnectedDevice();
            bool success = webCommunication.ConnectDevice(device);
            Assert.True(success);
        }

        [Fact]
        public void DeviceFalseConnectTest()
        {
            MeasureDevice device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.ConnectDevice(device));
        }

        [Fact]
        public void DeviceFalseDisconnectTest()
        {
            MeasureDevice device = GetConnectedDevice();
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.DisconnectDevice(device));
        }

        [Fact]
        public void DeviceDisconnectTest()
        {
            MeasureDevice device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            bool success = webCommunication.DisconnectDevice(device);
            Assert.True(success);
        }

        [Fact]
        public void DeviceAutoDisconnectTest()
        {
            MeasureDevice device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            simulatorDevice.Disconnected += Raise.Event<EventHandler<IDeviceHandler>>(this, simulatorDevice as IDeviceHandler);
            Thread.Sleep(10);
            Assert.False(device.IsConnected);
        }

        [Fact]
        public void DeviceLedStateTest()
        {
            MeasureDevice device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            bool ledState = webCommunication.GetLedState(device);
            Assert.Equal(this.ledState, ledState);
            ledState = webCommunication.ChangeLedState(device, !this.ledState);
            Assert.Equal(this.ledState, ledState);
        }

        [Fact]
        public void DeviceLedStateWithoutConnectingToDeviceTest()
        {
            MeasureDevice device = GetConnectedDevice();
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.GetLedState(device));
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.ChangeLedState(device, false));
        }

        [Fact]
        public void MeasuresArrivedTest()
        {
            MeasureDevice device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            simulatorDevice.MeasuresArrived += Raise.Event<EventHandler<MeasuresArrivedEventArgs>>(this, 
                new MeasuresArrivedEventArgs(Mapper.Map<MeasureDevice>(simulatorDevice), new Device.Models.Measures(), DateTime.Now));
            Assert.NotNull(measuresArrived);
        }

        [Fact]
        public void BroadcastScannerDeviceLostTest()
        {
            MeasureDevice device = GetConnectedDevice();
            broadcastScanner.DeviceInfoUpdated += Raise.Event<EventHandler<BroadcastListener.Models.DeviceUpdatedEventArgs>>(this,
                new BroadcastListener.Models.DeviceUpdatedEventArgs(simulatorDevice, UpdateStatus.Lost));
            Assert.False(webCommunication.Devices.Any(s => s.MacAddress == device.MacAddress));
        }

        [Fact]
        public void BroadcastScannerRestDeviceFoundTest()
        {
            InitializeMapper();
            var simulatorDevice = deviceHandlerFactory.CreateSimulatorDevice();
            webCommunication.Start();
            broadcastScanner.DeviceInfoUpdated += Raise.Event<EventHandler<BroadcastListener.Models.DeviceUpdatedEventArgs>>(this,
                new BroadcastListener.Models.DeviceUpdatedEventArgs(simulatorDevice, UpdateStatus.Found));
            Assert.NotNull(webCommunication.Devices.SingleOrDefault(s => s.MacAddress == simulatorDevice.MacAddress));
        }

        [Fact]
        public void RegisterCallbackChannelWithoutWcfContextTests()
        {
            webCommunication.RegisterCallbackChannel();
            Assert.NotNull(callback);
        }

        public void Dispose()
        {
            webCommunication.Dispose();
        }
    }
}
