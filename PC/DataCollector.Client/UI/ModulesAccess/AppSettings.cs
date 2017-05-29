using DataCollector.Client.UI.ModulesAccess.Interfaces;
using System.Globalization;
using Microsoft.Win32;
using System.Reflection;
using DataCollector.Client.UI.Properties;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Klasa zarzadzajaca ustawieniami aplikacji.
    /// </summary>
    class AppSettings : IAppSettings
    {
        #region Public Properties
        /// <summary>
        /// Uruchamiaj aplikacje przy starcie systemu.
        /// </summary>
        public bool RunAppDuringStartup
        {
            get
            {
                //zdobycie klucza rejestru
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //sprawdzenie czy istnieje wpis w rejestrze startowym
                return (rkApp.GetValue("DataCollector") != null);
            }

            set
            {
                //zdobycie klucza rejestru
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                var s = Assembly.GetExecutingAssembly().Location;
                if (value)
                    //dodanie aplikacji do rejestru rozruchowego
                    rkApp.SetValue("DataCollector", Assembly.GetExecutingAssembly().Location);
                else
                    //usunięcie aplikacji z rejestru rozruchowego
                    rkApp.DeleteValue("DataCollector", false);
            }
        }
        /// <summary>
        /// Adres serwisu kolektora danych.
        /// </summary>
        public string CollectorServiceHost
        {
            get
            {
                return Settings.Default.CollectorServiceHost;
            }
            set
            {
                Settings.Default.CollectorServiceHost = value;
            }
        }
        /// <summary>
        /// Adres serwisu dostępu do danych.
        /// </summary>
        public string DataAccessHost
        {
            get
            {
                return Settings.Default.DataAccessHost;
            }
            set
            {
                Settings.Default.DataAccessHost = value;
            }
        }
        /// <summary>
        /// Adres serwisu komunikacji z urządzeniami.
        /// </summary>
        public string DeviceCommunicationHost
        {
            get
            {
                return Settings.Default.DeviceCommunicationHost;
            }
            set
            {
                Settings.Default.DeviceCommunicationHost = value;
            }
        }
        /// <summary>
        /// Adres serwisu dostępu do użytkowników.
        /// </summary>
        public string UsersHost
        {
            get
            {
                return Settings.Default.UsersHost;
            }
            set
            {
                Settings.Default.UsersHost = value;
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy AppSettings.
        /// </summary>
        public AppSettings()
        {
            //podniesienie wersji ustawień jeśli mamy do czynienia z nową wersją aplikacji
            //standardowo Settings.Default.UpgradeRequired==True
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
            }
            Settings.Default.PropertyChanged += (o, e) => Settings.Default.Save();
        }
        #endregion
    }
}
