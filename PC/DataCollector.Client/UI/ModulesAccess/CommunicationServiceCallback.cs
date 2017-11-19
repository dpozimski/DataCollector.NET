using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Handles the events from the communication service.
    /// </summary>
    public class CommunicationServiceCallback : ICommunicationServiceCallback, ICommunicationServiceEventCallback
    {
        #region Events        
        /// <summary>
        /// The event which contains an information about the changed device.
        /// </summary>
        /// <CreatedOn>19.11.2017 12:36</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public event EventHandler<DeviceUpdatedEventArgs> DeviceChangedStateEvent;
        /// <summary>
        /// The event which contains the measures from the device.
        /// </summary>
        /// <CreatedOn>19.11.2017 12:36</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public event EventHandler<MeasuresArrivedEventArgs> MeasuresArrivedEvent;
        #endregion

        #region ICommunicationServiceCallback        
        /// <summary>
        /// Devices the state of the changed.
        /// </summary>
        /// <param name="deviceUpdated">The <see cref="DeviceUpdatedEventArgs"/> instance containing the event data.</param>
        /// <CreatedOn>19.11.2017 12:36</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public void DeviceChangedState(DeviceUpdatedEventArgs deviceUpdated)
        {
            DeviceChangedStateEvent?.Invoke(this, deviceUpdated);
        }
        /// <summary>
        /// Measureses the arrived.
        /// </summary>
        /// <param name="measures">The <see cref="MeasuresArrivedEventArgs"/> instance containing the event data.</param>
        /// <CreatedOn>19.11.2017 12:36</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public void MeasuresArrived(MeasuresArrivedEventArgs measures)
        {
            MeasuresArrivedEvent?.Invoke(this, measures);
        }
        #endregion
    }
}
