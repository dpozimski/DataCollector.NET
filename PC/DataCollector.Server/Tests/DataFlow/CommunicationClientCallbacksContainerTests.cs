using DataCollector.Server.DataFlow;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
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
        private Models.DeviceUpdatedEventArgs deviceUpdated;
        private ICommunicationServiceCallback callback;
        private readonly CommunicationClientCallbacksContainer container;

        public CommunicationClientCallbacksContainerTests()
        {
            callback = Substitute.For<ICommunicationServiceCallback, IChannel>();
            callback.When(s => s.MeasuresArrived(Arg.Any<MeasuresArrivedEventArgs>())).Do(s => measuresArrived = s.Arg<MeasuresArrivedEventArgs>());
            callback.When(s => s.DeviceChangedState(Arg.Any<Models.DeviceUpdatedEventArgs>())).Do(s => deviceUpdated = s.Arg<Models.DeviceUpdatedEventArgs>());
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
            MeasuresArrivedEventArgs measures = new MeasuresArrivedEventArgs(null, null, DateTime.Now);
            container.OnMeasuresArrived(measures);
            Assert.Equal(measures, measuresArrived);
        }

        [Fact]
        public void SubscribeForDeviceUpdatedTest()
        {
            RegisterCallback();
            ((IChannel)callback).State.Returns(s => CommunicationState.Opened);
            Models.DeviceUpdatedEventArgs deviceUpdated = new Models.DeviceUpdatedEventArgs(null, UpdateStatus.ConnectedToRestService);
            container.OnDeviceChangedState(deviceUpdated);
            Assert.Equal(deviceUpdated, this.deviceUpdated);
        }

        [Fact]
        public void DeleteOnDisconnectedClientWhileSendingDataTest()
        {
            RegisterCallback();
            ((IChannel)callback).State.Returns(s => CommunicationState.Closed);
            Models.DeviceUpdatedEventArgs deviceUpdated = new Models.DeviceUpdatedEventArgs(null, UpdateStatus.ConnectedToRestService);
            container.OnDeviceChangedState(deviceUpdated);
            Assert.DoesNotContain(callback, container.Clients);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}
