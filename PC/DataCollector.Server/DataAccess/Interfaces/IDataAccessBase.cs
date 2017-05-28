using DataCollector.Server.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Interfaces
{
    /// <summary>
    /// Podstawowy interfejs dostępu do bazy danych.
    /// </summary>
    public interface IDataAccessBase
    {
        #region Properties
        /// <summary>
        /// Dane połączeniowe do bazy danych.
        /// </summary>
        string ConnectionString { get; }
        #endregion
    }
}
