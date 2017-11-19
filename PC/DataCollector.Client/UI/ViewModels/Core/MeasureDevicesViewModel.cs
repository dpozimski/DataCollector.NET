using DataCollector.Client.UI.Converters;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using netoaster.Enumes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace DataCollector.Client.UI.ViewModels.Core
{
    /// <summary>
    /// The devices view model.
    /// </summary>
    public class MeasureDevicesViewModel : ViewModelBase
    {
        #region Private Fields
        private EnumToStringDescription enumConverter;
        private ObservableCollection<MeasureDeviceViewModel> devices;
        private IMeasureAccessService measureAccess;
        private ICommunicationServiceEventCallback webCommunicationCallback;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets the connected devices count.
        /// </summary>
        /// <value>
        /// The connected devices count.
        /// </value>
        public int ConnectedDevicesCount =>
                devices.Count(s => s.IsConnected);
        /// <summary>
        /// Gets the devices count.
        /// </summary>
        /// <value>
        /// The devices count.
        /// </value>
        public int DevicesCount =>
                devices.Count();
        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public ObservableCollection<MeasureDeviceViewModel> Devices
        {
            get { return devices; }
            set { this.RaiseAndSetIfChanged(ref devices, value); }
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureDevicesViewModel"/> class.
        /// </summary>
        public MeasureDevicesViewModel()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
#endif
            enumConverter = new EnumToStringDescription();
            Devices = new ObservableCollection<MeasureDeviceViewModel>();
            measureAccess = ServiceLocator.Resolve<IMeasureAccessService>();
            webCommunicationCallback = ServiceLocator.Resolve<ICommunicationServiceEventCallback>();
            webCommunicationCallback.DeviceChangedStateEvent += new EventHandler<DeviceUpdatedEventArgs>(OnDeviceChangedState);
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Called when [device changed state].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DeviceUpdatedEventArgs"/> instance containing the event data.</param>
        private async void OnDeviceChangedState(object sender, DeviceUpdatedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ToastType toastType = ToastType.Success;
                var device = devices.SingleOrDefault(s => s.MacAddress == e.Device.MacAddress);
                if (device == null && e.UpdateStatus == UpdateStatus.Found)
                {
                    var vmDevice = new MeasureDeviceViewModel(e.Device);
                    Devices.Add(vmDevice);
                }
                if (device != null)
                {
                    if (e.UpdateStatus != UpdateStatus.Lost)
                        device.Update(e.Device);
                    else
                    {
                        toastType = ToastType.Error;
                        Devices.Remove(device);
                    }
                }
                string strEnum = enumConverter.ToDescription(e.UpdateStatus);
                DialogAccess.ShowToastNotification($"{strEnum} {e.Device.Name}", toastType);
                this.RaisePropertyChanged(nameof(ConnectedDevicesCount));
                this.RaisePropertyChanged(nameof(DevicesCount));
            }));
        }
        #endregion
    }
}
