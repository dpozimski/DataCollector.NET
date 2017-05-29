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
        /// Dane połączeniowe do bazy danych.
        /// </summary>
        public string DatabaseConnectionString
        {
            get
            {
                return Settings.Default.DatabaseConnectionString;
            }

            set
            {
                Settings.Default.DatabaseConnectionString = value;
            }
        }
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
