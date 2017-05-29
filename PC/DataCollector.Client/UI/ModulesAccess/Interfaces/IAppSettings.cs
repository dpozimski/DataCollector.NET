
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Client.UI.ModulesAccess.Interfaces
{
    /// <summary>
    /// Interfejs zawierający ustawienia aplikacji.
    /// </summary>
    public interface IAppSettings
    {
        #region Properties
        /// <summary>
        /// Uruchamiaj aplikacje przy starcie systemu.
        /// </summary>
        bool RunAppDuringStartup { get; set; }
        /// <summary>
        /// Adres serwisu kolektora danych.
        /// </summary>
        string CollectorServiceHost { get; set; }
        /// <summary>
        /// Adres serwisu dostępu do danych.
        /// </summary>
        string DataAccessHost { get; set; }
        /// <summary>
        /// Adres serwisu komunikacji z urządzeniami.
        /// </summary>
        string DeviceCommunicationHost { get; set; }
        /// <summary>
        /// Adres serwisu dostępu do użytkowników.
        /// </summary>
        string UsersHost { get; set; }
        #endregion
    }
}
