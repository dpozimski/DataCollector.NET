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
        private SqlConnectionStringBuilder sqlConnectionBuilder;
        private bool runAppDuringStartup;
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
        /// Użytkownik.
        /// </summary>
        public string Login
        {
            get { return sqlConnectionBuilder.UserID; }
            set
            {
                sqlConnectionBuilder.UserID = value;
                this.RaisePropertyChanged(nameof(Login));
            }
        }
        /// <summary>
        /// Hasło.
        /// </summary>
        public string Password
        {
            get { return sqlConnectionBuilder.Password; }
            set
            {
                sqlConnectionBuilder.Password = value;
                this.RaisePropertyChanged(nameof(Password));
            }
        }
        /// <summary>
        /// Adres instancji bazy danych.
        /// </summary>
        public string DatabaseAddress
        {
            get { return sqlConnectionBuilder.DataSource; }
            set { sqlConnectionBuilder.DataSource = value;
                this.RaisePropertyChanged(nameof(DatabaseAddress));
            }
        }
        /// <summary>
        /// Dane połączeniowe do bazy danych.
        /// </summary>
        public string DatabaseConnectionString
        {
            get { return sqlConnectionBuilder.ToString(); }
            set { sqlConnectionBuilder = new SqlConnectionStringBuilder(value);
                this.RaisePropertyChanged(nameof(DatabaseConnectionString));
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstrukotr klasy SettingsDialogViewModel.
        /// </summary>
        public SettingsDialogViewModel()
        {
            var settings = ServiceLocator.Resolve<IAppSettings>();
            DatabaseConnectionString = settings.DatabaseConnectionString;
            RunAppDuringStartup = settings.RunAppDuringStartup;
        }
        #endregion
    }
}
