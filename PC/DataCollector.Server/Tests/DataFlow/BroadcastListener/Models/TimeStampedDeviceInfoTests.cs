using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener.Models
{
    public class TimeStampedDeviceInfoTests
    {
        private readonly IDeviceBroadcastInfo broadcastInfo;

        public TimeStampedDeviceInfoTests()
        {
            broadcastInfo = TestModelsFactory.CreateDeviceBroadcastInfoMock();
        }

        [Fact]
        public void ExpirationTest()
        {
            TimestampedDeviceInfo timeStampDeviceInfo = new TimestampedDeviceInfo(broadcastInfo, TimeSpan.FromSeconds(1));
            Assert.False(timeStampDeviceInfo.IsExpired);
        }
    }
}
