using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using DataCollector.Server.Interfaces;
using DataCollector.Server.DataFlow.Handlers.Interfaces;

namespace DataCollector.Server.Tests.DataFlow.Handlers
{
    /// <summary>
    /// Klasa testująca <see cref="SimulatorDeviceHandler"/>
    /// </summary>
    public class SimulatorDeviceHandlerTests: AbstractDeviceHandlerTests
    {
        public override IDeviceHandler Init()
        {
            return new SimulatorDeviceHandler(TestModelsFactory.CreateDeviceBroadcastInfoMock());
        }
    }
}
