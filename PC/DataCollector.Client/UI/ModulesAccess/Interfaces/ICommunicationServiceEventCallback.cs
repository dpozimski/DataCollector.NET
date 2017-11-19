using DataCollector.Client.UI.DeviceCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess.Interfaces
{
    /// <summary>
    /// The interface which defines an interaction <see cref="ICommunicationService"/> with an UI.
    /// </summary>
    public interface ICommunicationServiceEventCallback
    {
        #region Events
        /// <summary>
        /// The event which contains an information about the changed device.
        /// </summary>
        event EventHandler<DeviceUpdatedEventArgs> DeviceChangedStateEvent;
        /// <summary>
        /// The event which contains the measures from the device.
        /// </summary>
        event EventHandler<MeasuresArrivedEventArgs> MeasuresArrivedEvent;
        #endregion
    }
}
