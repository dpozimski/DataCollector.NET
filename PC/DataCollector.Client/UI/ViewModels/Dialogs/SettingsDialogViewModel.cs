using DataCollector.Client.UI.ModulesAccess;
using DataCollector.Client.UI.ModulesAccess.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ViewModels.Dialogs
{
    /// <summary>
    /// ViewModel implementujący obsługę zmiany ustawień aplikacji.
    /// </summary>
    class SettingsDialogViewModel : ViewModelBase, IAppSettings
    {
        #region Private Fields
        private bool runAppDuringStartup;
        private string collectorServiceHost, dataAccessHost, deviceCommunicationHost, usersHost;
        #endregion

        #region Public Properties
        /// <summary>
        /// Uruchamiaj aplikacje przy starcie systemu.
        /// </summary>
        public bool RunAppDuringStartup
        {
            get { return runAppDuringStartup; }
            set { this.RaiseAndSetIfChanged(ref runAppDuringStartup, value); }
        }
        /// <summary>
        /// Adres serwisu kolektora danych.
        /// </summary>
        public string CollectorServiceHost
        {
            get { return collectorServiceHost; }
            set { this.RaiseAndSetIfChanged(ref collectorServiceHost, value); }
        }
        /// <summary>
        /// Adres serwisu dostępu do danych.
        /// </summary>
        public string DataAccessHost
        {
            get { return dataAccessHost; }
            set { this.RaiseAndSetIfChanged(ref dataAccessHost, value); }
        }
        /// <summary>
        /// Adres serwisu komunikacji z urządzeniami.
        /// </summary>
        public string DeviceCommunicationHost
        {
            get { return deviceCommunicationHost; }
            set { this.RaiseAndSetIfChanged(ref deviceCommunicationHost, value); }
        }
        /// <summary>
        /// Adres serwisu dostępu do użytkowników.
        /// </summary>
        public string UsersHost
        {
            get { return usersHost; }
            set { this.RaiseAndSetIfChanged(ref usersHost, value); }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstrukotr klasy SettingsDialogViewModel.
        /// </summary>
        public SettingsDialogViewModel()
        {
            var settings = ServiceLocator.Resolve<IAppSettings>();
            RunAppDuringStartup = settings.RunAppDuringStartup;
        }
        #endregion
    }
}
