﻿using DataCollector.Server.BroadcastListener.Exceptions;
using DataCollector.Server.BroadcastListener.Factories;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DataCollector.Server.BroadcastListener.Exceptions.InvalidFrameException;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener.Factories
{
    /// <summary>
    /// Klasa testująca fabrykę ramek identyfikujących urządzenie.
    /// </summary>
    public class DeviceBroadcastInfoFactoryTests
    {
        private IDevicesBroadcastInfoFactory broadcastInfoFactory;

        public DeviceBroadcastInfoFactoryTests()
        {
            broadcastInfoFactory = new DeviceBroadcastInfoFactory();
        }

        IDeviceBroadcastInfo Execute(byte[] frame)
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
            IDeviceBroadcastInfo deviceInfo = Execute(frame);
            Assert.Equal(string.Compare("Raspberry Pi 3", deviceInfo.Name), 0);
            Assert.Equal("AA:AA:AA:AA:AA:AA", deviceInfo.MacAddress);
            Assert.Equal("ARM", deviceInfo.Architecture);
            Assert.Equal("10.1586", deviceInfo.WinVer);
            Assert.True(deviceInfo.IPv4.Equals("192.168.101.101"));
        }
    }
}
