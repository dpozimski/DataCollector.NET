using DataCollector.Client.Communication.Interfaces;
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
    /// ViewModel implementujący obsługę zmiany diody LED.
    /// </summary>
    public class LedManagerViewModel : ViewModelBase
    {
        #region Private Fields
        private ICommunication webAccess;
        private bool isActive;
        #endregion

        #region Public Properties
        /// <summary>
        /// Stan diody LED.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { this.RaiseAndSetIfChanged(ref isActive, value); }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda aktywacji diody LED.
        /// </summary>
        public ReactiveCommand<object> TurnOnCommand { get; protected set; }
        /// <summary>
        /// Komenda dezaktywacji diody LED.
        /// </summary>
        public ReactiveCommand<object> TurnOffCommand { get; protected set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy LedManagerViewModel.
        /// </summary>
        public LedManagerViewModel()
        {
            webAccess = ServiceLocator.Resolve<ICommunication>();
            InitCommands();
            InitData();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Inicjalizacja danych wejściowych.
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
        /// Inicjalizacja komend.
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
