using DataCollector.Client.Communication.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.Views.Core;
using DataCollector.Client.DataAccess.Interfaces;
using System.Windows;

namespace DataCollector.Client.UI.ViewModels.Core
{
    /// <summary>
    /// ViewModel reprezentujący urządzenie pomiarowe.
    /// </summary>
    public class MeasureDeviceViewModel : ViewModelBase
    {
        #region Private Fields
        private IMeasureAccess measureAccess;
        private ICommunication webCommunication;
        private IDevice deviceHandler;
        private string name, winVer, architecture, macAddress, model;
        private IPAddress ipV4;
        private bool isConnected;
        #endregion

        #region Public Properties
        /// <summary>
        /// Adres IP urządzenia.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }
        /// <summary>
        /// IPv4.
        /// </summary>
        public IPAddress IPv4
        {
            get { return ipV4; }
            set { this.RaiseAndSetIfChanged(ref ipV4, value); }
        }
        /// <summary>
        /// Wersja Windows 10 IoT Core.
        /// </summary>
        public string WinVer
        {
            get { return winVer; }
            set { this.RaiseAndSetIfChanged(ref winVer, value); }
        }
        /// <summary>
        /// Architektura systemu.
        /// </summary>
        public string Architecture
        {
            get { return architecture; }
            set { this.RaiseAndSetIfChanged(ref architecture, value); }
        }
        /// <summary>
        /// Adres MAC urządzenia.
        /// </summary>
        public string MacAddress
        {
            get { return macAddress; }
            set { this.RaiseAndSetIfChanged(ref macAddress, value); }
        }
        /// <summary>
        /// Model urządzenia.
        /// </summary>
        public string Model
        {
            get { return model; }
            set { this.RaiseAndSetIfChanged(ref model, value); }
        }
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        public bool IsConnected
        {
            get { return isConnected; }
            set { this.RaiseAndSetIfChanged(ref isConnected, value); }
        }
        /// <summary>
        /// Interwał pobierania pomiarów.
        /// </summary>
        public double MeasurementsMsRequestInterval
        {
            get { return deviceHandler.MeasurementsMsRequestInterval; }
            set
            {
                deviceHandler.MeasurementsMsRequestInterval = value;
                measureAccess.UpdateMeasureDevice(deviceHandler);
                this.RaisePropertyChanged(nameof(MeasurementsMsRequestInterval));
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Komenda nawiązania komunikacji z urządzeniem.
        /// </summary>
        public ReactiveCommand<object> ConnectPromptCommand
        {
            get; protected set;
        }
        /// <summary>
        /// Komenda rozłączenia komunikacji z urządzeniem.
        /// </summary>
        public ReactiveCommand<object> DisconnectPromptCommand
        {
            get; protected set;
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy MeasureDeviceViewModel.
        /// </summary>
        /// <param name="deviceHandler"></param>
        public MeasureDeviceViewModel(IDevice deviceHandler)
        {
            webCommunication = ServiceLocator.Resolve<ICommunication>();
            measureAccess = ServiceLocator.Resolve<IMeasureAccess>();
            Update(deviceHandler);
            InitCommands();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Zwraca referencję do głównego uchwytu urządzenia.
        /// </summary>
        /// <returns></returns>
        public IDevice GetDeviceHandler() =>
            deviceHandler;
        /// <summary>
        /// Aktualizuje model o nowe dane urządzenia.
        /// </summary>
        /// <param name="deviceHandler"></param>
        public void Update(IDevice deviceHandler)
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
        /// Metoda inicjalizująca komendy.
        /// </summary>
        private void InitCommands()
        {
            ConnectPromptCommand = ReactiveCommand.Create();
            DisconnectPromptCommand = ReactiveCommand.Create();
            ConnectPromptCommand.Subscribe(async s => await ConnectPromptMethod());
            DisconnectPromptCommand.Subscribe(async s => await DisconnectPromptMethod());
        }
        /// <summary>
        /// Obsługa rozłączenia komunikacji z urządzeniem.
        /// </summary>
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
        /// Obsługa nawiązania komunikacji z urządzeniem.
        /// </summary>
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
        /// Zmiana stanu flagu zajętości.
        /// </summary>
        /// <param name="value"></param>
        private async Task ChangeBusyState(bool value) =>
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() => IsBusy = value));
        #endregion
    }
}
