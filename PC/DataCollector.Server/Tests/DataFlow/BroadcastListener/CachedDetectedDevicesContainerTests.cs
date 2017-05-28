using DataCollector.Server.BroadcastListener;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener
{
    /// <summary>
    /// Klasa testująca <see cref="CachedDetectedDevicesContainer"/>
    /// </summary>
    public class CachedDetectedDevicesContainerTests : IDisposable
    {
        private readonly TimeSpan cleanUpCacheInterval;
        private readonly CachedDetectedDevicesContainer devicesContainer;
        private readonly IDeviceBroadcastInfo broadcastInfo;

        public CachedDetectedDevicesContainerTests()
        {
            cleanUpCacheInterval = TimeSpan.FromMilliseconds(100);
            devicesContainer = new CachedDetectedDevicesContainer(cleanUpCacheInterval);
            broadcastInfo = TestModelsFactory.CreateDeviceBroadcastInfoMock();
        }

        public void Dispose()
        {
            devicesContainer.Dispose();
        }

        [Fact]
        public void TestUpdateMethod()
        {
            devicesContainer.Update(broadcastInfo);
            Assert.True(devicesContainer.Devices.Any(s => s.MacAddress == broadcastInfo.MacAddress));
        }

        [Fact]
        public void TestClearDeviceCache()
        {
            devicesContainer.Update(broadcastInfo);
            devicesContainer.ClearDeviceCache(broadcastInfo);
            Assert.False(devicesContainer.Devices.Any(s => s.MacAddress == broadcastInfo.MacAddress));
        }

        [Fact]
        public void TestDeviceUpdateNotChangedIfTheSame()
        {
            bool eventRaised = false;
            devicesContainer.Update(broadcastInfo);
            devicesContainer.DeviceInfoUpdated += (o, e) => eventRaised = true;
            devicesContainer.Update(broadcastInfo);
            Assert.False(eventRaised);
        }

        [Fact]
        public void TestDeviceUpdateChangedIfTheSameObjectChangedData()
        {
            bool eventRaised = false;
            devicesContainer.Update(broadcastInfo);
            devicesContainer.DeviceInfoUpdated += (o, e) => eventRaised = true;
            IDeviceBroadcastInfo updatedInfo = new DeviceBroadcastInfo(broadcastInfo.Name + "Other", 
                broadcastInfo.IPv4, broadcastInfo.MacAddress, broadcastInfo.MacAddress, broadcastInfo.WinVer, broadcastInfo.Model);
            devicesContainer.Update(updatedInfo);
            Assert.True(eventRaised);
        }

        [Fact]
        public void CleanUpOldDevicesTest()
        {
            devicesContainer.Update(broadcastInfo);
            Assert.Equal(1, devicesContainer.Devices.Count());
            Thread.Sleep(500);
            Assert.Equal(0, devicesContainer.Devices.Count());
        }
    }
}
