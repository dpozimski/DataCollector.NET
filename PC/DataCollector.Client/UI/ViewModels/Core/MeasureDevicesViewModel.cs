using DataCollector.Client.Communication.Interfaces;
using DataCollector.Client.Communication.Models;
using DataCollector.Client.DataAccess.Interfaces;
using DataCollector.Client.DataFlow.BroadcastListener.Models;
using DataCollector.Client.UI.Converters;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using DataCollector.Client.UI.ViewModels.Dialogs;
using DataCollector.Client.UI.Views.Core;
using netoaster;
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
    /// ViewModel implementujący prezentację wykrytych urządzeń w sieci.
    /// </summary>
    public class MeasureDevicesViewModel : ViewModelBase
    {
        #region Private Fields
        private EnumToStringDescription enumConverter;
        private ObservableCollection<MeasureDeviceViewModel> devices;
        private IMeasureAccess measureAccess;
        private ICommunication webCommunication;
        #endregion

        #region Public Properties
        /// <summary>
        /// Liczba podłączonych urządzeń w systemie.
        /// </summary>
        public int ConnectedDevicesCount =>
                devices.Count(s => s.IsConnected);
        /// <summary>
        /// Liczba urządzeń w systemie.
        /// </summary>
        public int DevicesCount =>
                devices.Count();
        /// <summary>
        /// Lista urządzeń widzianych w sieci.
        /// </summary>
        public ObservableCollection<MeasureDeviceViewModel> Devices
        {
            get { return devices; }
            set { this.RaiseAndSetIfChanged(ref devices, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy MeasureDevicesViewModel.
        /// </summary>
        public MeasureDevicesViewModel()
        {
#if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
#endif
            enumConverter = new EnumToStringDescription();
            Devices = new ObservableCollection<MeasureDeviceViewModel>();
            measureAccess = ServiceLocator.Resolve<IMeasureAccess>();
            webCommunication = ServiceLocator.Resolve<ICommunication>();
            webCommunication.DeviceChangedState += new EventHandler<DeviceUpdatedEventArgs>(OnDeviceChangedState);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa zdarzenia zmiany stanu urządzenia wykrytego w sieci.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDeviceChangedState(object sender, DeviceUpdatedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ToastType toastType = ToastType.Success;
                var device = devices.SingleOrDefault(s => s.MacAddress == e.Device.MacAddress);
                if (device == null && e.UpdateStatus == UpdateStatus.Found)
                {
                    measureAccess.AssignMeasureDevice(e.Device);
                    var vmDevice = new MeasureDeviceViewModel(e.Device);
                    Devices.Add(vmDevice);
                }
                if (device != null)
                {
                    if (e.UpdateStatus != UpdateStatus.Lost)
                    {
                        measureAccess.AssignMeasureDevice(e.Device);
                        device.Update(e.Device);
                    }
                    else
                    {
                        toastType = ToastType.Error;
                        Devices.Remove(device);
                    }
                }
                string strEnum = enumConverter.ToDescription(e.UpdateStatus);
                DialogAccess.ShowToastNotification(string.Format(strEnum, e.Device.Name), toastType);
                this.RaisePropertyChanged(nameof(ConnectedDevicesCount));
                this.RaisePropertyChanged(nameof(DevicesCount));
            }));
        }
        #endregion
    }
}
