using DataCollector.Client.UI.DeviceCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess.Interfaces
{
    /// <summary>
    /// Interfejs definiujący interakcję zdarzeń serwisu <see cref="ICommunicationService"/> z UI.
    /// </summary>
    public interface ICommunicationServiceEventCallback
    {
        #region Events
        /// <summary>
        /// Zdarzenie nadejścia aktualizacji stanu urządzenia.
        /// </summary>
        event EventHandler<DeviceUpdatedEventArgs> DeviceChangedStateEvent;
        /// <summary>
        /// Zdarzenie nadejścia pomiarów z urządzenia.
        /// </summary>
        event EventHandler<MeasuresArrivedEventArgs> MeasuresArrivedEvent;
        #endregion
    }
}
