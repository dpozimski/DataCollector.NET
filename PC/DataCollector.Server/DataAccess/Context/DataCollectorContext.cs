using DataCollector.Server.DataAccess.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Context
{
    /// <summary>
    /// Kontekst bazy danych.
    /// </summary>
    class DataCollectorContext : DbContext
    {
        #region DbSet
        /// <summary>
        /// Tabela z historią logowania użytkowników.
        /// </summary>
        public DbSet<UserLoginHistory> UsersLoginHistory { get; set; }
        /// <summary>
        /// Tabela z urządzeniami pomiarowymi.
        /// </summary>
        public DbSet<MeasureDevice> MeasureDevices { get; set; }
        /// <summary>
        /// Tabela z odciskiem czasu pomiarów.
        /// </summary>
        public DbSet<DeviceTimeMeasurePoint> DevicesTimeStampTable { get; set; }
        /// <summary>
        /// Tabela użytkowników.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Tabela z pomiarami przestrzennymi.
        /// </summary>
        public DbSet<SphereMeasurePoint> SphereMeasurePoints { get; set; }
        /// <summary>
        /// Tabela z pomiarami jednostkowymi.
        /// </summary>
        public DbSet<MeasurePoint> MeasurePoints { get; set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DataCollectorContext.
        /// <param name="connectionString">dane połączeniowe</param>
        /// </summary>
        public DataCollectorContext(string connectionString): base(connectionString)
        {
            Database.SetInitializer(new DataCollectorContextInitializer());
        }
        #endregion
    }
}
