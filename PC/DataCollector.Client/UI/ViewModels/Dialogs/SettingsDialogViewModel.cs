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
    /// The settings dialog view model.
    /// </summary>
    /// <seealso cref="DataCollector.Client.UI.ViewModels.ViewModelBase" />
    /// <seealso cref="DataCollector.Client.UI.ModulesAccess.Interfaces.IAppSettings" />
    class SettingsDialogViewModel : ViewModelBase, IAppSettings
    {
        #region Private Fields
        private bool runAppDuringStartup;
        private string collectorServiceHost, dataAccessHost, deviceCommunicationHost, usersHost;
        #endregion

        #region Public Properties        
        /// <summary>
        /// Run the app on startup.
        /// </summary>
        public bool RunAppDuringStartup
        {
            get { return runAppDuringStartup; }
            set { this.RaiseAndSetIfChanged(ref runAppDuringStartup, value); }
        }
        /// <summary>
        /// The collector service host.
        /// </summary>
        public string CollectorServiceHost
        {
            get { return collectorServiceHost; }
            set { this.RaiseAndSetIfChanged(ref collectorServiceHost, value); }
        }
        /// <summary>
        /// The data access host.
        /// </summary>
        public string DataAccessHost
        {
            get { return dataAccessHost; }
            set { this.RaiseAndSetIfChanged(ref dataAccessHost, value); }
        }
        /// <summary>
        /// The device communication host.
        /// </summary>
        public string DeviceCommunicationHost
        {
            get { return deviceCommunicationHost; }
            set { this.RaiseAndSetIfChanged(ref deviceCommunicationHost, value); }
        }
        /// <summary>
        /// The user management host.
        /// </summary>
        public string UsersHost
        {
            get { return usersHost; }
            set { this.RaiseAndSetIfChanged(ref usersHost, value); }
        }
        #endregion

        #region ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDialogViewModel"/> class.
        /// </summary>
        public SettingsDialogViewModel()
        {
            var settings = ServiceLocator.Resolve<IAppSettings>();
            RunAppDuringStartup = settings.RunAppDuringStartup;
        }
        #endregion
    }
}
