using DataCollector.Server.DataAccess.Context;
using DataCollector.Server.DataAccess.Interfaces;
using DataCollector.Server.DataAccess.Models;
using DataCollector.Device.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollector.Server.DeviceHandlers.Models;
using System.Data.Entity.Migrations;
using DataCollector.Server.Interfaces.Data;
using DataCollector.Server.Interfaces.Communication;
using DataCollector.Server.DataAccess;
using System.ServiceModel;

namespace DataCollector.Server
{
    /// <summary>
    /// Klasa odpowiedzialna za zarządzaniem umieszczania pomiarów w bazie danych.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MeasureCollectorService : DataAccessBase, IMeasureCollectorService, ICommunicationServiceCallback
    {
        #region Constants
        /// <summary>
        /// Liczba buforowanych danych zanim zostaną wpisane do bazy danych.
        /// </summary>
        private const int PrefetchMeasuresCount = 5;
        private readonly object syncObject = new object();
        #endregion

        #region Private Fields
        private PropertyDescriptorCollection measureProperties;
        private ICommunicationService webCommunication;
        private ICommunicationClientCallbacksContainer callbacksContainer;
        private ConcurrentBag<DeviceTimeMeasurePoint> cachedMeasures;
        #endregion

        #region Public Properties
        /// <summary>
        /// Kolekcjonowanie danych jest aktywne.
        /// </summary>
        public bool IsCollectingDataEnabled { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy MeasureAccess.
        /// <paramref name="webCommunication">referencja do serwisu komunikacyjnego</paramref>
        /// <paramref name="callbacksContainer">kontener odpowiedzi urządzenia</paramref>
        /// <paramref name=ConnectionString">dane połączeniowe</paramref>
        /// </summary>
        public MeasureCollectorService(ICommunicationService webCommunication, ICommunicationClientCallbacksContainer callbacksContainer, string ConnectionString) : base(ConnectionString)
        {
            this.callbacksContainer = callbacksContainer;
            this.webCommunication = webCommunication;
            cachedMeasures = new ConcurrentBag<DeviceTimeMeasurePoint>();
            measureProperties = TypeDescriptor.GetProperties(typeof(Measures));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Aktualne pomiary z pochodzące z urządzenia.
        /// <paramref name="e">pomiary z urządzenia</paramref>
        /// </summary>
        public Task MeasuresArrived(MeasuresArrivedEventArgs e)
        {
            List<SphereMeasurePoint> spherePoints = new List<SphereMeasurePoint>();
            List<MeasurePoint> points = new List<MeasurePoint>();
            DeviceTimeMeasurePoint deviceSingleMeasurePoint = new DeviceTimeMeasurePoint()
            {
                TimeStamp = e.TimeStamp,
                SphereMeasurePoints = spherePoints,
                MeasurePoints = points,
                AssignedMeasureDevice = new MeasureDevice() { MacAddress = e.Source.MacAddress }
            };
            //odczytywanie danych z wykorzystaniem refleksji
            foreach (PropertyDescriptor prop in measureProperties)
            {
                if (prop.PropertyType == typeof(SpherePoint))
                {
                    spherePoints.Add(new SphereMeasurePoint()
                    {
                        Point = prop.GetValue(e.Value) as SpherePoint,
                        Type = (SphereMeasureType)Enum.Parse(typeof(SphereMeasureType), prop.Name),
                        AssignedDeviceMeasureTimePoint = deviceSingleMeasurePoint
                    });
                }
                else if (prop.PropertyType == typeof(float?))
                {
                    points.Add(new MeasurePoint()
                    {
                        Type = (MeasureType)Enum.Parse(typeof(MeasureType), prop.Name),
                        Value = (float?)prop.GetValue(e.Value),
                        AssignedDeviceMeasureTimePoint = deviceSingleMeasurePoint
                    });
                }
            }

            cachedMeasures.Add(deviceSingleMeasurePoint);

            lock (syncObject)
            {
                if (cachedMeasures.Count > PrefetchMeasuresCount)
                    SnapMeasures();
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Wykryto urządzenie w sieci.
        /// Aktualizacja danych w bazie danych.
        /// <paramref name="deviceUpdated">dane dot. aktualizacji stanu urządzenia</paramref>
        /// </summary>
        public Task DeviceChangedState(DeviceUpdatedEventArgs deviceUpdated)
        {
            IDeviceInfo deviceHandler = deviceUpdated.Device;
            using (var db = new DataCollectorContext(ConnectionString))
            {
                MeasureDevice existingDevice = db.MeasureDevices.SingleOrDefault(s => s.MacAddress == deviceHandler.MacAddress);
                if (existingDevice != null)
                    existingDevice.Update(deviceHandler);
                else
                    existingDevice = MeasureDevice.FromCommunicationHandler(deviceHandler);
                db.MeasureDevices.AddOrUpdate(s => s.MacAddress, existingDevice);
                db.SaveChanges();
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Rozpoczęcie kolekcjonowania danych z warstwy komunikacyjnej.
        /// </summary>
        public void StartCollectingData()
        {
            if (IsCollectingDataEnabled)
                throw new InvalidOperationException("Kolekcjonowanie danych zostało już aktywowane.");

            callbacksContainer.RegisterCallbackChannel(nameof(MeasureCollectorService), this);

            IsCollectingDataEnabled = true;
        }

        /// <summary>
        /// Zakończenie kolekcjonowania danych z warstwy komunikacyjnej.
        /// </summary>
        public void StopCollectingData()
        {
            if (!IsCollectingDataEnabled)
                throw new InvalidOperationException("Kolekcjonowanie danych nie zostało jeszcze aktywowane.");

            callbacksContainer.DeleteCallbackChannel(nameof(MeasureCollectorService));

            IsCollectingDataEnabled = false;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Zapisuje do bazy danych kolekcje pomiarów urządzeń.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        private void InsertDeviceMeasures(IEnumerable<DeviceTimeMeasurePoint> collection)
        {
            using (var db = new DataCollectorContext(ConnectionString))
            {
                //przypisanie referencji do właściciela pomiaru
                foreach (var macAddressGroup in collection.GroupBy(s => s.AssignedMeasureDevice.MacAddress))
                {
                    var measureDevice = db.MeasureDevices.Single(s => s.MacAddress == macAddressGroup.Key);
                    foreach (var singleMeasure in macAddressGroup)
                        singleMeasure.AssignedMeasureDevice = measureDevice;
                }

                db.DevicesTimeStampTable.AddRange(collection);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Metoda wpisująca pomiary do bazy danych.
        /// </summary>
        private void SnapMeasures()
        {
            List<DeviceTimeMeasurePoint> snappedMeasures = new List<DeviceTimeMeasurePoint>();
            foreach (var item in cachedMeasures)
            {
                DeviceTimeMeasurePoint singleDeviceMeasure = null;
                if (cachedMeasures.TryTake(out singleDeviceMeasure))
                    snappedMeasures.Add(singleDeviceMeasure);
            }

            Task.Factory.StartNew(() => InsertDeviceMeasures(snappedMeasures));
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie przypisanych zasobów.
        /// </summary>
        public void Dispose()
        {
            if(IsCollectingDataEnabled)
                callbacksContainer.DeleteCallbackChannel(nameof(MeasureCollectorService));
        }
        #endregion
    }
}
