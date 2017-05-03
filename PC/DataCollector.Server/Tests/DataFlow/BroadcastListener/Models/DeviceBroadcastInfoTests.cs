using DataCollector.Server.DataFlow.BroadcastListener.Models;
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
        private readonly DeviceBroadcastInfo deviceBroadcastInfo;
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

        [Fact]
        public void CreateCopyOfObject()
        {
            DeviceBroadcastInfo copy = new DeviceBroadcastInfo(deviceBroadcastInfo);
            Assert.True(copy.Equals(deviceBroadcastInfo));
        }

    }
}
