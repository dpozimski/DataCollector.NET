using DataCollector.Server.DataAccess.Context;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using LiveCharts.Defaults;
using DataCollector.Server.Interfaces.Data;
using DataCollector.Server.DataAccess;
using System.ServiceModel;
using DataCollector.Server.DataAccess.Models.Entities;

namespace DataCollector.Server
{
    /// <summary>
    /// Klasa implementująca serws IMeasureAccess.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MeasureAccessService : DataAccessBase, IMeasureAccessService
    {
        #region ctor
        /// <summary>
        /// Konstruktor nowej instancji klasy.
        /// </summary>
        /// <param name="ConnectionString">dane połączeniowe</param>
        public MeasureAccessService(string ConnectionString) : base(ConnectionString)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">zakres dolny</param>
        /// <param name="upperRange">zakres górny</param>
        /// <returns>punkty pomiarowe [X]</returns>
        public List<DateTimePoint[]> GetMeasures(MeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange)
        {
            using (var db = new StoredProceduresDataContext(ConnectionString))
            {
                var data = db.SPU_GetMeasurePoints(device.ID, (int)type, lowerRange, upperRange).Select(s => new DateTimePoint[] {
                                            new DateTimePoint(s.TimeStamp, s.Value ?? 0)});

                return data.ToList();
            }
        }
        /// <summary>
        /// Pobiera pomiary od wskazanego urządzenia, typu i okresu.
        /// </summary>
        /// <param name="type">typ pomiary</param>
        /// <param name="device">urządzenie pomiarowe</param>
        /// <param name="lowerRange">od</param>
        /// <param name="upperRange">do</param>
        /// <returns>punkty pomiarowe [X,Y,Z]</returns>
        public List<DateTimePoint[]> GetSphereMeasures(SphereMeasureType type, MeasureDevice device, DateTime lowerRange, DateTime upperRange)
        {
            using (var db = new StoredProceduresDataContext(ConnectionString))
            {
                var data = db.SPU_GetSphereMeasurePoints(device.ID, (int)type, lowerRange, upperRange).Select(s => new DateTimePoint[] {
                                            new DateTimePoint(s.TimeStamp, s.Point_X),
                                            new DateTimePoint(s.TimeStamp, s.Point_Y),
                                            new DateTimePoint(s.TimeStamp, s.Point_Z)});

                return data.ToList();
            }
        }
        /// <summary>
        /// Pobiera dostępne urządzenia pomiarowe z bazy danych.
        /// </summary>
        /// <returns></returns>
        public List<MeasureDevice> GetMeasureDevices()
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                return db.MeasureDevices.OrderBy(s => s.Name).ToList();
            }
        }
        /// <summary>
        /// Aktualizuje urządzenie pomiarowe.
        /// </summary>
        /// <param name="macAddress">adres MAC urządzenia</param>
        /// <param name="requestInterval">interwal rejestracji</param>
        public void UpdateDeviceRequestInterval(string macAddress, double requestInterval)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                MeasureDevice existingDevice = db.MeasureDevices.Single(s => s.MacAddress == macAddress);
                //zaktualizuj ustawienia w bazie danych
                existingDevice.MeasurementsMsRequestInterval = requestInterval;
                db.SaveChanges();
            }
        }
        #endregion
    }
}
