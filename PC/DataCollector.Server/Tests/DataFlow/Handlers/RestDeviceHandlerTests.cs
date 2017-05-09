﻿using DataCollector.Server.DataFlow.Handlers;
using DataCollector.Server.Tests.Utils;
using Xunit;
using DataCollector.Server.Interfaces;
using DataCollector.Server.DataFlow.Handlers.Adapters;
using NSubstitute;
using Newtonsoft.Json;
using DataCollector.Device.Models;
using System;
using DataCollector.Server.DataFlow.Handlers.Interfaces;

namespace DataCollector.Server.Tests.DataFlow.Handlers
{
    /// <summary>
    /// Klasa testująca <see cref="RestDeviceHandler"/>
    /// </summary>
    public class RestDeviceHandlerTests : AbstractDeviceHandlerTests
    {
        private RestDeviceHandlerConfiguration deviceHandlerConfiguration;
        private IRestConnectionAdapter connectionAdapter;
        private bool ledState;

        public override IDeviceHandler Init()
        {
            this.deviceHandlerConfiguration = TestModelsFactory.CreateRestDeviceConfigMock();
            this.connectionAdapter = Substitute.For<IRestConnectionAdapter>();
            this.connectionAdapter.GetRequest(deviceHandlerConfiguration.GetMeasurementsRequest).Returns(s => JsonConvert.SerializeObject(new Measures()));
            this.connectionAdapter.GetRequest(deviceHandlerConfiguration.LedStateRequest).Returns(s => ledState.ToString());
            this.connectionAdapter.GetRequest(string.Format(deviceHandlerConfiguration.LedChangeRequest, true)).Returns(s => ChangeLedState(true));
            this.connectionAdapter.GetRequest(string.Format(deviceHandlerConfiguration.LedChangeRequest, false)).Returns(s => ChangeLedState(false));

            var deviceHandler = new RestDeviceHandler(connectionAdapter, deviceHandlerConfiguration, TestModelsFactory.CreateDeviceBroadcastInfoMock());
            return deviceHandler;
        }

        private string ChangeLedState(bool state)
        {
            ledState = state;
            return ledState.ToString();
        }
    }
}