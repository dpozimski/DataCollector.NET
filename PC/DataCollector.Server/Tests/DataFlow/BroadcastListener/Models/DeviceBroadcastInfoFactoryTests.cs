using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.BroadcastListener.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DataCollector.Server.DataFlow.BroadcastListener.Models.Exceptions.InvalidFrameException;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener.Models
{
    /// <summary>
    /// Klasa testująca fabrykę ramek identyfikujących urządzenie.
    /// </summary>
    public class DeviceBroadcastInfoFactoryTests
    {
        private DeviceBroadcastInfoFactory broadcastInfoFactory;

        public DeviceBroadcastInfoFactoryTests()
        {
            broadcastInfoFactory = new DeviceBroadcastInfoFactory();
        }

        DeviceBroadcastInfo Execute(byte[] frame)
        {
            return broadcastInfoFactory.From(frame);
        }

        [Fact]
        public void ParsingNullFrame()
        {
            byte[] frame = null;
            Assert.Throws<ArgumentNullException>(() => Execute(frame));
        }

        [Fact]
        public void ParsingBadSizeFrame()
        {
            byte[] frame = new byte[1];
            var exception = Assert.Throws<InvalidFrameException>(() => Execute(frame));
            Assert.Equal(ErrorType.FrameSize, exception.Type);
        }

        [Fact]
        public void ParsingCorrectFrame()
        {
            byte[] frame = Properties.Resources.CorrectFrame;
            DeviceBroadcastInfo deviceInfo = Execute(frame);
            Assert.Equal(string.Compare("Raspberry Pi 3", deviceInfo.Name), 0);
            Assert.Equal("AA:AA:AA:AA:AA:AA", deviceInfo.MacAddress);
            Assert.Equal("ARM", deviceInfo.Architecture);
            Assert.Equal("10.1586", deviceInfo.WinVer);
            Assert.True(IPAddress.Parse("192.168.101.101").Equals(deviceInfo.IPv4));
        }
    }
}
