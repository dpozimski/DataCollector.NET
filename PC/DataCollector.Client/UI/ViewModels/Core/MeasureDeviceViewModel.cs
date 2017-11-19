using ReactiveUI;
using System;
using System.Threading.Tasks;
using DataCollector.Client.UI.ModulesAccess;
using System.Windows;
using DataCollector.Client.UI.DataAccess;
using DataCollector.Client.UI.DeviceCommunication;

namespace DataCollector.Client.UI.ViewModels.Core
{
    /// <summary>
    /// The device view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    public class MeasureDeviceViewModel : ViewModelBase
    {
        #region Private Fields
        private IMeasureAccessService measureAccess;
        private ICommunicationService webCommunication;
        private DeviceCommunication.MeasureDevice deviceHandler;
        private string name, winVer, architecture, macAddress, model;
        private string ipV4;
        private bool isConnected;
        #endregion

        #region Public Properties       
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }
        /// <summary>
        /// Gets or sets the i PV4.
        /// </summary>
        /// <value>
        /// The i PV4.
        /// </value>
        public string IPv4
        {
            get { return ipV4; }
            set { this.RaiseAndSetIfChanged(ref ipV4, value); }
        }
        /// <summary>
        /// Gets or sets the win ver.
        /// </summary>
        /// <value>
        /// The win ver.
        /// </value>
        public string WinVer
        {
            get { return winVer; }
            set { this.RaiseAndSetIfChanged(ref winVer, value); }
        }
        /// <summary>
        /// Gets or sets the architecture.
        /// </summary>
        /// <value>
        /// The architecture.
        /// </value>
        public string Architecture
        {
            get { return architecture; }
            set { this.RaiseAndSetIfChanged(ref architecture, value); }
        }
        /// <summary>
        /// Gets or sets the mac address.
        /// </summary>
        /// <value>
        /// The mac address.
        /// </value>
        public string MacAddress
        {
            get { return macAddress; }
            set { this.RaiseAndSetIfChanged(ref macAddress, value); }
        }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public string Model
        {
            get { return model; }
            set { this.RaiseAndSetIfChanged(ref model, value); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected
        {
            get { return isConnected; }
            set { this.RaiseAndSetIfChanged(ref isConnected, value); }
        }
        /// <summary>
        /// Gets or sets the measurements ms request interval.
        /// </summary>
        /// <value>
        /// The measurements ms request interval.
        /// </value>
        public double MeasurementsMsRequestInterval
        {
            get { return deviceHandler.MeasurementsMsRequestInterval; }
            set
            {
                deviceHandler.MeasurementsMsRequestInterval = value;
                measureAccess.UpdateDeviceRequestInterval(deviceHandler.MacAddress, deviceHandler.MeasurementsMsRequestInterval);
                this.RaisePropertyChanged(nameof(MeasurementsMsRequestInterval));
            }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the connect prompt command.
        /// </summary>
        /// <value>
        /// The connect prompt command.
        /// </value>
        public ReactiveCommand<object> ConnectPromptCommand
        {
            get; protected set;
        }
        /// <summary>
        /// Gets or sets the disconnect prompt command.
        /// </summary>
        /// <value>
        /// The disconnect prompt command.
        /// </value>
        public ReactiveCommand<object> DisconnectPromptCommand
        {
            get; protected set;
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureDeviceViewModel"/> class.
        /// </summary>
        /// <param name="deviceHandler">The device handler.</param>
        public MeasureDeviceViewModel(DeviceCommunication.MeasureDevice deviceHandler)
        {
            webCommunication = ServiceLocator.Resolve<ICommunicationService>();
            measureAccess = ServiceLocator.Resolve<IMeasureAccessService>();
            Update(deviceHandler);
            InitCommands();
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the device handler.
        /// </summary>
        /// <returns></returns>
        public DeviceCommunication.MeasureDevice GetDeviceHandler() =>
            deviceHandler;
        /// <summary>
        /// Updates the specified device handler.
        /// </summary>
        /// <param name="deviceHandler">The device handler.</param>
        public void Update(DeviceCommunication.MeasureDevice deviceHandler)
        {
            this.Name = deviceHandler.Name;
            this.IPv4 = deviceHandler.IPv4;
            this.IsConnected = deviceHandler.IsConnected;
            this.MacAddress = deviceHandler.MacAddress;
            this.Model = deviceHandler.Model;
            this.WinVer = deviceHandler.WinVer;
            this.Architecture = deviceHandler.Architecture;
            this.deviceHandler = deviceHandler;
        }
        #endregion

        #region Private Methods       
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            ConnectPromptCommand = ReactiveCommand.Create();
            DisconnectPromptCommand = ReactiveCommand.Create();
            ConnectPromptCommand.Subscribe(async s => await ConnectPromptMethod());
            DisconnectPromptCommand.Subscribe(async s => await DisconnectPromptMethod());
        }
        /// <summary>
        /// Disconnects the prompt method.
        /// </summary>
        /// <returns></returns>
        private async Task DisconnectPromptMethod()
        {
            await ChangeBusyState(true);
            await Task.Run(async () =>
            {
                try
                {
                    webCommunication.DisconnectDevice(deviceHandler);
                }
                finally
                {
                    await ChangeBusyState(false);
                }
            });
        }
        /// <summary>
        /// Connects the prompt method.
        /// </summary>
        /// <returns></returns>
        private async Task ConnectPromptMethod()
        {
            await ChangeBusyState(true);
            await Task.Run(async () =>
            {
                try
                {
                    webCommunication.ConnectDevice(deviceHandler);
                }
                finally
                {
                    await ChangeBusyState(false);
                }
            });
        }
        /// <summary>
        /// Changes the state of the busy.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        private async Task ChangeBusyState(bool value) =>
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() => IsBusy = value));
        #endregion
    }
}
