
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
        /// Dane połączeniowe do bazy danych.
        /// </summary>
        string DatabaseConnectionString { get; set; }
        #endregion
    }
}
