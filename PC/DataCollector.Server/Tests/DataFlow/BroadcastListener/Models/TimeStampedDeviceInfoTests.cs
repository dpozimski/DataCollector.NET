using DataCollector.Server.DataFlow.BroadcastListener.Models;
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
        private readonly DeviceBroadcastInfo broadcastInfo;

        public TimeStampedDeviceInfoTests()
        {
            broadcastInfo = new DeviceBroadcastInfo("TestMachineName", IPAddress.Parse("192.168.101.101"), "AA:AA:AA:AA:AA:AA", "ARM", "10.586", "ARM");
        }

        [Fact]
        public void ExpirationTest()
        {
            TimestampedDeviceInfo timeStampDeviceInfo = new TimestampedDeviceInfo(broadcastInfo);
            Assert.False(timeStampDeviceInfo.IsExpired);
        }
    }
}
