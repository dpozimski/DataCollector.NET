﻿using AutoMapper;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.Handlers;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
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
        private DeviceUpdatedEventArgs deviceUpdated;
        private IDeviceHandler simulatorDevice;
        private IBroadcastScanner broadcastScanner;
        private IDeviceHandlerFactory deviceHandlerFactory;
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
            callbackContainer = Substitute.For<ICommunicationClientCallbacksContainer>();
            callbackContainer.When(s => s.OnMeasuresArrived(Arg.Any<MeasuresArrivedEventArgs>())).Do(s => measuresArrived = s.Arg<MeasuresArrivedEventArgs>());
            callbackContainer.When(s => s.OnDeviceChangedState(Arg.Any<DeviceUpdatedEventArgs>())).Do(s => deviceUpdated = s.Arg<DeviceUpdatedEventArgs>());

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

        private DeviceInfo GetConnectedDevice()
        {
            //AutoMapper
            Mapper.Initialize(cfg => {
                cfg.CreateMap<IDeviceInfo, DeviceInfo>();
            });

            webCommunication.Start();
            webCommunication.AddSimulatorDevice();
            Assert.NotNull(deviceUpdated);
            return Mapper.Map<DeviceInfo>(deviceUpdated.Device);
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
            DeviceInfo device = GetConnectedDevice();
            bool success = webCommunication.ConnectDevice(device);
            Assert.True(success);
        }

        [Fact]
        public void DeviceFalseConnectTest()
        {
            DeviceInfo device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.ConnectDevice(device));
        }

        [Fact]
        public void DeviceFalseDisconnectTest()
        {
            DeviceInfo device = GetConnectedDevice();
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.DisconnectDevice(device));
        }

        [Fact]
        public void DeviceDisconnectTest()
        {
            DeviceInfo device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            bool success = webCommunication.DisconnectDevice(device);
            Assert.True(success);
        }

        [Fact]
        public void DeviceAutoDisconnectTest()
        {
            DeviceInfo device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            simulatorDevice.Disconnected += Raise.Event<EventHandler<IDeviceHandler>>(this, simulatorDevice as IDeviceHandler);
            Thread.Sleep(10);
            Assert.False(device.IsConnected);
        }

        [Fact]
        public void DeviceLedStateTest()
        {
            DeviceInfo device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            bool ledState = webCommunication.GetLedState(device);
            Assert.Equal(this.ledState, ledState);
            ledState = webCommunication.ChangeLedState(device, !this.ledState);
            Assert.Equal(this.ledState, ledState);
        }

        [Fact]
        public void DeviceLedStateWithoutConnectingToDeviceTest()
        {
            DeviceInfo device = GetConnectedDevice();
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.GetLedState(device));
            Assert.Throws(typeof(InvalidOperationException), () => webCommunication.ChangeLedState(device, false));
        }

        [Fact]
        public void MeasuresArrivedTest()
        {
            DeviceInfo device = GetConnectedDevice();
            webCommunication.ConnectDevice(device);
            simulatorDevice.MeasuresArrived += Raise.Event<EventHandler<MeasuresArrivedEventArgs>>(this, 
                new MeasuresArrivedEventArgs(simulatorDevice, new Device.Models.Measures(), DateTime.Now));
            Assert.NotNull(measuresArrived);
        }

        public void Dispose()
        {
            webCommunication.Dispose();
        }
    }
}