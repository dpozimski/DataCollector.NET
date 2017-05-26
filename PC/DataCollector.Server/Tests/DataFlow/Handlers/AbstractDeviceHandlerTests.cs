using AutoMapper;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.Handlers
{
    /// <summary>
    /// Klasa abstrakcyjna testujące podstawowe funkcjonalności klasy.
    /// </summary>
    public abstract class AbstractDeviceHandlerTests : IDisposable
    {
        private IDeviceHandler deviceHandler;

        public AbstractDeviceHandlerTests()
        {
            deviceHandler = Init();
        }

        /// <summary>
        /// Metoda inicjująca obiekt.
        /// </summary>
        /// <returns></returns>
        public abstract IDeviceHandler Init();

        [Fact]
        public void TestConnect()
        {
            bool success = deviceHandler.Connect();
            Assert.True(success && deviceHandler.IsConnected);
        }

        [Fact]
        public void GetMeasurementsTests()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<IDeviceInfo, DeviceInfo>();
            });
            bool measuresArrived = false;
            deviceHandler.MeasuresArrived += (o, e) => measuresArrived = true;
            deviceHandler.MeasurementsMsRequestInterval = 0;
            bool success = deviceHandler.Connect();
            Thread.Sleep(30);
            Assert.True(measuresArrived && success);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void TestLedStateChanging(bool state)
        {
            deviceHandler.Connect();
            deviceHandler.ChangeLedState(state);
            bool ledState = deviceHandler.GetLedState();
            Assert.Equal(state, ledState);
        }

        [Fact]
        public void DisconnectEventRaiseTest()
        {
            bool eventRaised = false;
            deviceHandler.Disconnected += (o, e) => eventRaised = true;
            deviceHandler.Connect();
            Assert.False(eventRaised);
            deviceHandler.Disconnect();
            Assert.True(eventRaised);
        }

        public void Dispose()
        {
            deviceHandler.Dispose();
        }
    }
}
