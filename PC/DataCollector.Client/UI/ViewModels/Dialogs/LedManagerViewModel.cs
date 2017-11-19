using DataCollector.Client.UI.DeviceCommunication;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ViewModels.Core;
using DataCollector.Client.UI.Views.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCollector.Client.UI.ViewModels.Dialogs
{
    /// <summary>
    /// The manager view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    public class LedManagerViewModel : ViewModelBase
    {
        #region Private Fields
        private ICommunicationService webAccess;
        private bool isActive;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get { return isActive; }
            set { this.RaiseAndSetIfChanged(ref isActive, value); }
        }
        #endregion

        #region Commands        
        /// <summary>
        /// Gets or sets the turn on command.
        /// </summary>
        /// <value>
        /// The turn on command.
        /// </value>
        public ReactiveCommand<object> TurnOnCommand { get; protected set; }
        /// <summary>
        /// Gets or sets the turn off command.
        /// </summary>
        /// <value>
        /// The turn off command.
        /// </value>
        public ReactiveCommand<object> TurnOffCommand { get; protected set; }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="LedManagerViewModel"/> class.
        /// </summary>
        public LedManagerViewModel()
        {
            webAccess = ServiceLocator.Resolve<ICommunicationService>();
            InitCommands();
            InitData();
        }
        #endregion

        #region Private Methods        
        /// <summary>
        /// Initializes the data.
        /// </summary>
        private async void InitData()
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                var deviceHandler = MainViewModel.SelectedDevice.GetDeviceHandler();
                bool ledState = deviceHandler.IsConnected && webAccess.GetLedState(deviceHandler);
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    IsActive = ledState;
                    IsBusy = false;
                }));
            });
        }
        /// <summary>
        /// Initializes the commands.
        /// </summary>
        private void InitCommands()
        {
            TurnOnCommand = ReactiveCommand.Create(Observable.CombineLatest(MainViewModel.WhenAnyValue(s=>s.SelectedDevice), 
                                                                            this.WhenAnyValue(s=>s.IsActive),
                                                                            MainViewModel.SelectedDevice.WhenAnyValue(s=>s.IsConnected),
                                                                            (device, ledActive, conn) => device != null && device.IsConnected && conn && !ledActive));
            TurnOffCommand = ReactiveCommand.Create(Observable.CombineLatest(MainViewModel.WhenAnyValue(s => s.SelectedDevice),
                                                                            this.WhenAnyValue(s => s.IsActive),
                                                                            MainViewModel.SelectedDevice.WhenAnyValue(s => s.IsConnected),
                                                                            (device, ledActive, conn) => device != null && device.IsConnected && conn && ledActive));
            TurnOnCommand.Subscribe(async s => await ChangeLedState(true));
            TurnOffCommand.Subscribe(async s => await ChangeLedState(false));
        }
        private async Task ChangeLedState(bool state)
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                var deviceHandler = MainViewModel.SelectedDevice.GetDeviceHandler();
                bool ledState = webAccess.ChangeLedState(deviceHandler, state);
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    IsActive = ledState;
                    IsBusy = false;
                }));
            });

        }
        #endregion
    }
}
