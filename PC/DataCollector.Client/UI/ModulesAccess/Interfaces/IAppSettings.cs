
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess.Interfaces
{
    /// <summary>
    /// The interface that defines an app settings.
    /// </summary>
    public interface IAppSettings
    {
        #region Properties
        /// <summary>
        /// Run the app on startup.
        /// </summary>
        bool RunAppDuringStartup { get; set; }
        /// <summary>
        /// The collector service host.
        /// </summary>
        string CollectorServiceHost { get; set; }
        /// <summary>
        /// The data access host.
        /// </summary>
        string DataAccessHost { get; set; }
        /// <summary>
        /// The device communication host.
        /// </summary>
        string DeviceCommunicationHost { get; set; }
        /// <summary>
        /// The user management host.
        /// </summary>
        string UsersHost { get; set; }
        #endregion
    }
}
