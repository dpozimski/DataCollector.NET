using DataCollector.Server.DataAccess.Context;
using DataCollector.Server.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess
{
    /// <summary>
    /// Klasa abstrakcyjna implementująca interfejs IDataAccesBase.
    /// </summary>
    public abstract class DataAccessBase : IDataAccessBase
    {
        #region Public Properties
        /// <summary>
        /// Dane połączeniowe do bazy danych.
        /// </summary>
        public string ConnectionString { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor nowej instancji klasy.
        /// </summary>
        /// <param name="ConnectionString">dane połączeniowe</param>
        public DataAccessBase(string ConnectionString)
        {
            TryApplyConnectionString(ConnectionString);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda ustawiająca nowe dane połączeniowe do bazy danych.
        /// </summary>
        /// <param name="connStr">dane połączeniowe</param>
        /// <returns>zwraca status migracji</returns>
        private bool TryApplyConnectionString(string connStr)
        {
            try
            {
                using (var db = new DataCollectorContext(connStr))
                {
                    db.Database.CommandTimeout = 20;
                    //migracja bazy danych
                    db.Users.ToList();
                    ConnectionString = connStr;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TryApplyConnectionString Exception: " + ex);
                return false;
            }
        }
        #endregion
    }
}
