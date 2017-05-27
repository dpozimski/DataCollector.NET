using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener.Models
{
    public class DeviceBroadcastInfoTests
    {
        private readonly IDeviceBroadcastInfo deviceBroadcastInfo;
        private const string stringRepresentation = "\"TestMachineName\" - 192.168.101.101 [AA:AA:AA:AA:AA:AA]";

        public DeviceBroadcastInfoTests()
        {
            deviceBroadcastInfo = new DeviceBroadcastInfo("TestMachineName", IPAddress.Parse("192.168.101.101"), "AA:AA:AA:AA:AA:AA", "ARM", "10.586", "ARM");
        }

        [Fact]
        public void ToStringTest()
        {
            string data = deviceBroadcastInfo.ToString();
            Assert.Equal(stringRepresentation.CompareTo(data), 0);
        }

        [Fact]
        public void GetHashCodeTest()
        {
            int hashCode = stringRepresentation.GetHashCode();
            Assert.Equal(hashCode, deviceBroadcastInfo.GetHashCode());
        }


        [Fact]
        public void CompareTheSameObjectEqualTest()
        {
            Assert.True(deviceBroadcastInfo.Equals(deviceBroadcastInfo));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(null)]
        [InlineData("Test")]
        public void CompareObjectWithWrongType(object type)
        {
            Assert.False(deviceBroadcastInfo.Equals(type));
        }

        [Theory]
        [InlineData("21:22:33:44:55:66")]
        [InlineData("5:22:33:44:55:66")]
        [InlineData("11:22:33:14:55:26")]
        [InlineData("11:55:33:44:55:46")]
        [InlineData("11:22:33:44:55:62")]
        [InlineData("11:22:33:44:55:64")]
        [InlineData("76:22:33:44:55:66")]
        [InlineData("10:11:23:44:55:66")]
        public void CompareObjectsFalseTest(string mac)
        {
            IDeviceBroadcastInfo compareTestInfo = new DeviceBroadcastInfo("TestMachineName", IPAddress.Parse("192.168.101.101"), mac, "ARM", "10.586", "ARM");
            Assert.False(deviceBroadcastInfo.Equals(compareTestInfo));
        }

        [Fact]
        public void CreateCopyOfObject()
        {
            IDeviceBroadcastInfo copy = new DeviceBroadcastInfo(deviceBroadcastInfo);
            Assert.True(copy.Equals(deviceBroadcastInfo));
        }

    }
}
