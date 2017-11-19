using DataCollector.Client.UI.ModulesAccess.Interfaces;
using System.Globalization;
using Microsoft.Win32;
using System.Reflection;
using DataCollector.Client.UI.Properties;

namespace DataCollector.Client.UI.ModulesAccess
{
    /// <summary>
    /// Class which holds the settings values.
    /// </summary>
    class AppSettings : IAppSettings
    {
        #region Public Properties
        /// <summary>
        /// Run the app on startup.
        /// </summary>
        public bool RunAppDuringStartup
        {
            get
            {
                //gets the registry key
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //checks if the entry exists
                return (rkApp.GetValue("DataCollector") != null);
            }

            set
            {
                //gets the registry key
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                var s = Assembly.GetExecutingAssembly().Location;
                if (value)
                    //add the app location to run registry
                    rkApp.SetValue("DataCollector", Assembly.GetExecutingAssembly().Location);
                else
                    //delete the app location from run registry
                    rkApp.DeleteValue("DataCollector", false);
            }
        }
        /// <summary>
        /// The collector service host.
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
        /// The data access host.
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
        /// The device communication host.
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
        /// The users host.
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
        /// The class constructor.
        /// </summary>
        public AppSettings()
        {
            //if the version is higher(the flag is true) upgrade the settings
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
