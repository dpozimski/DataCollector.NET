using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DeviceHandlers.Models;
using DataCollector.Server.Interfaces.Communication;
using DataCollector.Server.Tests.Utils;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests
{
    /// <summary>
    /// Klasa testująca <see cref="MeasureCollectorService"/>.
    /// </summary>
    public class MeasureCollectorServiceTests : IDisposable
    {
        private readonly ICommunicationClientCallbacksContainer container;
        private readonly MeasureCollectorService collectorService;
        private ICommunicationServiceCallback callback;

        public MeasureCollectorServiceTests()
        {
            container = Substitute.For<ICommunicationClientCallbacksContainer>();
            container.When(s => s.RegisterCallbackChannel(Arg.Any<string>(), Arg.Any<ICommunicationServiceCallback>())).Do(s => callback = s.Arg<ICommunicationServiceCallback>());
            container.When(s => s.DeleteCallbackChannel(Arg.Any<string>())).Do(s => callback = null);
            collectorService = new MeasureCollectorService(container, TestModelsFactory.TestConnectionString);
        }

        [Fact]
        public void StartCollectingDataTests()
        {
            collectorService.StartCollectingData();
            Assert.NotNull(callback);
        }

        [Fact]
        public void StopCollectingDataTests()
        {
            collectorService.StartCollectingData();
            collectorService.StopCollectingData();
            Assert.Null(callback);
        }

        [Fact]
        public void CollectingStartDataStateFailChangeTest()
        {
            collectorService.StartCollectingData();
            Assert.Throws<InvalidOperationException>(() => collectorService.StartCollectingData());
        }

        [Fact]
        public void CollectingStopDataStateFailChangeTest()
        {
            collectorService.StartCollectingData();
            collectorService.StopCollectingData();
            Assert.Throws<InvalidOperationException>(() => collectorService.StopCollectingData());
        }

        [Fact]
        public void InsertDataTests()
        {
            MeasureDevice device = new MeasureDevice();
            MeasuresArrivedEventArgs eventArgs = new MeasuresArrivedEventArgs(device, new Device.Models.Measures(), DateTime.Now);
            Assert.NotNull(collectorService.MeasuresArrived(eventArgs));
        }

        [Fact]
        public void UpdateDeviceFailTests()
        {
            MeasureDevice device = new MeasureDevice();
            DeviceUpdatedEventArgs deviceUpdated = new DeviceUpdatedEventArgs(device, BroadcastListener.Models.UpdateStatus.Found);
            Assert.ThrowsAsync<DbEntityValidationException>(() => collectorService.DeviceChangedState(deviceUpdated));
        }

        public void Dispose()
        {
            collectorService.Dispose();
        }
    }
}
