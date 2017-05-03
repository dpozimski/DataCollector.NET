using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.BroadcastListener.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
