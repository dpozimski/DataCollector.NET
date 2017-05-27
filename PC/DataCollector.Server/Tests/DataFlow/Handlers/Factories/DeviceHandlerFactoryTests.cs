using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.DeviceHandlers.Adapters;
using DataCollector.Server.DeviceHandlers.Factories;
using DataCollector.Server.DeviceHandlers.Interfaces;
using DataCollector.Server.Tests.Utils;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.Handlers.Factories
{
    /// <summary>
    /// Klasa testująca <see cref="DeviceHandlerFactory"/>.
    /// </summary>
    public class DeviceHandlerFactoryTests
    {
        private RestDeviceHandlerConfiguration restDeviceConfiguration;
        private DeviceHandlerFactory deviceHandlerFactory;
        private IRestConnectionAdapterFactory restConnectionAdapterFactory;
        private IDeviceBroadcastInfo broadcastInfo;

        public DeviceHandlerFactoryTests()
        {
            broadcastInfo = Substitute.For<IDeviceBroadcastInfo>();
            restConnectionAdapterFactory = Substitute.For<IRestConnectionAdapterFactory>();
            restDeviceConfiguration = TestModelsFactory.CreateRestDeviceConfigMock();

            deviceHandlerFactory = new DeviceHandlerFactory(restConnectionAdapterFactory, restDeviceConfiguration);
        }

        [Fact]
        public void CreateSingleSimulatorDeviceWithoutNull()
        {
            IDeviceHandler deviceHandler = deviceHandlerFactory.CreateSimulatorDevice();
            Assert.NotNull(deviceHandler);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(255)]
        public void CreateMultiSimulatorDevicesWithEqualsChecking(int objectsToCreate)
        {
            List<IDeviceHandler> deviceHandlers = new List<IDeviceHandler>();
            for (int i = 0; i < objectsToCreate; i++)
            {
                IDeviceHandler deviceHandler = deviceHandlerFactory.CreateSimulatorDevice();
                Assert.False(deviceHandlers.Any(s => s.Equals(deviceHandler)));
                deviceHandlers.Add(deviceHandler);
            } 
        }

        [Fact]
        public void CreateRestDeviceWithNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => deviceHandlerFactory.CreateRestDevice(null, 0));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateRestDeviceWithWrongPortArgument(int port)
        {
            Assert.Throws<ArgumentException>(() => deviceHandlerFactory.CreateRestDevice(broadcastInfo, port));
        }

        [Theory]
        [InlineData(80)]
        [InlineData(443)]
        public void CreateRestDeviceWithWithoutErrors(int port)
        {
            IDeviceHandler deviceHandler = deviceHandlerFactory.CreateRestDevice(broadcastInfo, port);
            Assert.NotNull(deviceHandler);
        }
    }
}
