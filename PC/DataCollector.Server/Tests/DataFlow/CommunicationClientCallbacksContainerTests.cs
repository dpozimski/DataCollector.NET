using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.DeviceHandlers.Models;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Interfaces.Communication;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow
{
    /// <summary>
    /// Klasa testująca <see cref="CommuncationClientCallbacksContainer"/>
    /// </summary>
    public class CommunicationClientCallbacksContainerTests : IDisposable
    {
        private MeasuresArrivedEventArgs measuresArrived;
        private DeviceHandlers.Models.DeviceUpdatedEventArgs deviceUpdated;
        private ICommunicationServiceCallback callback;
        private readonly CommunicationClientCallbacksContainer container;

        public CommunicationClientCallbacksContainerTests()
        {
            callback = Substitute.For<ICommunicationServiceCallback, IChannel>();
            callback.When(s => s.MeasuresArrived(Arg.Any<DeviceHandlers.Models.MeasuresArrivedEventArgs>())).Do(s => measuresArrived = s.Arg<DeviceHandlers.Models.MeasuresArrivedEventArgs>());
            callback.When(s => s.DeviceChangedState(Arg.Any<DeviceHandlers.Models.DeviceUpdatedEventArgs>())).Do(s => deviceUpdated = s.Arg<DeviceHandlers.Models.DeviceUpdatedEventArgs>());
            container = new CommunicationClientCallbacksContainer();
        }

        private void RegisterCallback()
        {
            string id = "123";
            container.RegisterCallbackChannel(id, callback);
        }

        [Fact]
        public void RegisterCallbackTest()
        {
            RegisterCallback();
            Assert.Contains(callback, container.Clients);
        }

        [Fact]
        public void SubscribeForMeasuresTest()
        {
            RegisterCallback();
            ((IChannel)callback).State.Returns(s => CommunicationState.Opened);
            MeasureDevice deviceInfo = new MeasureDevice();
            MeasuresArrivedEventArgs measures = new MeasuresArrivedEventArgs(deviceInfo, null, DateTime.Now);
            container.OnMeasuresArrived(measures);
            Assert.Equal(measures, measuresArrived);
        }

        [Fact]
        public void SubscribeForDeviceUpdatedTest()
        {
            RegisterCallback();
            ((IChannel)callback).State.Returns(s => CommunicationState.Opened);
            MeasureDevice deviceInfo = new MeasureDevice();
            DeviceHandlers.Models.DeviceUpdatedEventArgs deviceUpdated = new DeviceHandlers.Models.DeviceUpdatedEventArgs(deviceInfo, UpdateStatus.ConnectedToRestService);
            container.OnDeviceChangedState(deviceUpdated);
            Assert.Equal(deviceUpdated, this.deviceUpdated);
        }

        [Fact]
        public void DeleteOnDisconnectedClientWhileSendingDataTest()
        {
            RegisterCallback();
            ((IChannel)callback).State.Returns(s => CommunicationState.Closed);
            DeviceHandlers.Models.DeviceUpdatedEventArgs deviceUpdated = new DeviceHandlers.Models.DeviceUpdatedEventArgs(null, UpdateStatus.ConnectedToRestService);
            container.OnDeviceChangedState(deviceUpdated);
            Assert.DoesNotContain(callback, container.Clients);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
