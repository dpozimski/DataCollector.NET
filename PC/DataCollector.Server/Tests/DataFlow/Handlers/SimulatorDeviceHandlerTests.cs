using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DeviceHandlers;
using DataCollector.Server.DeviceHandlers.Interfaces;
using DataCollector.Server.Tests.Utils;

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
