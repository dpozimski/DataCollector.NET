﻿using AutoMapper;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server
{
    /// <summary>
    /// klasa impleentująca interfejs ICommunication.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WebCommunicationService : ICommunicationService
    {
        #region Private Fields
        private int port;
        private IBroadcastScanner broadcastScanner;
        private SynchronizedCollection<IDeviceHandler> deviceHandlers;
        private IDeviceHandlerFactory deviceHandlerFactory;
        private ICommunicationClientCallbacksContainer callbackContainer;
        #endregion

        #region Properties
        /// <summary>
        /// Usługi serwisu uruchomione.
        /// </summary>
        public bool IsStarted { get; private set; }
        /// <summary>
        /// Aktualnie podłączone urządzenia.
        /// </summary>
        public IEnumerable<DeviceInfo> Devices
        {
            get { return Mapper.Map<IEnumerable<DeviceInfo>>(deviceHandlers); }
        }
        #endregion

        /// <summary>
        /// Konstruktor nowej instancji klasy.
        /// </summary>
        /// <param name="deviceHandlerFactory">fabryka urządzeń pomiarowych</param>
        /// <param name="broadcastScanner"></param>
        /// <param name="port">port komunikacyjny</param>
        /// <param name="callbackContainer">kontener danych zwrotnych</param>
        public WebCommunicationService(IBroadcastScanner broadcastScanner, IDeviceHandlerFactory deviceHandlerFactory, ICommunicationClientCallbacksContainer callbackContainer, int port)
        {
            this.broadcastScanner = broadcastScanner;
            this.deviceHandlerFactory = deviceHandlerFactory;
            this.deviceHandlers = new SynchronizedCollection<IDeviceHandler>();
            this.port = port;
            this.callbackContainer = callbackContainer;
        }

        #region Public Methods
        /// <summary>
        /// Dodaje klienta do sybkrypcji zdarzeń serwisu.
        /// </summary>
        public void RegisterCallbackChannel()
        {
            var callback = OperationContext.Current.GetCallbackChannel<ICommunicationServiceCallback>();
            string id = OperationContext.Current.SessionId;
            callbackContainer.RegisterCallbackChannel(id, callback);
        }
        /// <summary>
        /// Metoda dodające nowe urządzenie symulujące komunikację.
        /// </summary>
        public void AddSimulatorDevice()
        {
            var device = deviceHandlerFactory.CreateSimulatorDevice();
            Models.DeviceUpdatedEventArgs simulateEvent = new Models.DeviceUpdatedEventArgs(device, UpdateStatus.Found);
            deviceHandlers.Add(device);
            callbackContainer.OnDeviceChangedState(simulateEvent);
        }
        /// <summary>
        /// Uruchamia usługi serwisu,
        /// </summary>
        public void Start()
        {
            if (IsStarted)
                throw new InvalidOperationException("Serwis jest już uruchomiony");

            broadcastScanner.DeviceInfoUpdated += new EventHandler<DataFlow.BroadcastListener.Models.DeviceUpdatedEventArgs>(OnBroadcastDeviceInfoUpdated);
            broadcastScanner.StartListening();

            IsStarted = true;
        }
        /// <summary>
        /// Zatrzymuje usługi serwisu.
        /// </summary>
        public void Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Serwis nie jest uruchomiony");

            Dispose();
        }
        /// <summary>
        /// Metoda inicjująca połączenie z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        public bool ConnectDevice(DeviceInfo device)
        {
            var deviceHandler = deviceHandlers.SingleOrDefault(s => s.MacAddress == device.MacAddress);

            if (deviceHandler == null)
                throw new InvalidOperationException($"Brak urządzenia z MAC: {device.MacAddress}");
            if (deviceHandler.IsConnected)
                throw new InvalidOperationException("Urządzenie zostało już podłączone.");

            bool success = deviceHandler.Connect();
        
            //tutaj przypisanie eventu z zdarzeniami
            if (success)
            {
                deviceHandler.MeasuresArrived += new EventHandler<MeasuresArrivedEventArgs>(OnMeasuresArrived);
                deviceHandler.Disconnected += new EventHandler<IDeviceHandler>(OnDeviceDisconnected);
                callbackContainer.OnDeviceChangedState(new Models.DeviceUpdatedEventArgs(deviceHandler, UpdateStatus.ConnectedToRestService));
            } 

            return success;
        }

        /// <summary>
        /// Metoda przerywająca trwającą komunikacje z urządzeniem.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        public bool DisconnectDevice(DeviceInfo device)
        {
            var deviceHandler = deviceHandlers.Single(s => s.MacAddress == device.MacAddress);

            if (!deviceHandler.IsConnected || !deviceHandlers.Contains(deviceHandler))
                throw new InvalidOperationException("Urządzenie nie zostało podłączone.");

            bool success = deviceHandler.Disconnect();

            if (success)
            {
                deviceHandler.MeasuresArrived -= OnMeasuresArrived;
                deviceHandler.Disconnected -= OnDeviceDisconnected;
                callbackContainer.OnDeviceChangedState(new Models.DeviceUpdatedEventArgs(deviceHandler, UpdateStatus.DisconnectedFromRestService));
            }
                
            return success;
        }
        /// <summary>
        /// Metoda zmieniająca stan diody we wskazanym urządzeniu.
        /// </summary>
        /// <param name="target">urządzenie docelowe</param>
        /// <param name="state">stan diody</param>
        /// <returns></returns>
        public bool ChangeLedState(DeviceInfo target, bool state)
        {
            var deviceHandler = deviceHandlers.Single(s => s.MacAddress == target.MacAddress);

            if (!deviceHandlers.Contains(deviceHandler) || !deviceHandler.IsConnected)
                throw new InvalidOperationException("Urządzenie nie jest podłączone.");

            return deviceHandler.ChangeLedState(state);
        }
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <param name="target">urządzenie</param>
        /// <returns></returns>
        public bool GetLedState(DeviceInfo target)
        {
            var deviceHandler = deviceHandlers.Single(s => s.MacAddress == target.MacAddress);

            if (!deviceHandlers.Contains(deviceHandler) || !deviceHandler.IsConnected)
                throw new InvalidOperationException("Urządzenie nie jest podłączone.");

            return deviceHandler.GetLedState();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sygnalizacja rozłączenia urządzenia z komunikacji właściwej.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeviceDisconnected(object sender, IDeviceHandler e)
        {
            DeviceInfo deviceInfo = Mapper.Map<DeviceInfo>(e);
            Task.Factory.StartNew(() => DisconnectDevice(deviceInfo));
        }
        /// <summary>
        /// Obsługa zdarzenia nadejścia nowego pakietu informacji o urządzeniu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBroadcastDeviceInfoUpdated(object sender, DataFlow.BroadcastListener.Models.DeviceUpdatedEventArgs e)
        {
            var device = deviceHandlers.SingleOrDefault(s => s.MacAddress == e.DeviceInfo.MacAddress);
            if (device == null)
            {
                device = deviceHandlerFactory.CreateRestDevice(e.DeviceInfo, port);
                deviceHandlers.Add(device);
            }
            else if (e.UpdateStatus == UpdateStatus.Lost)
            {
                device.Disconnect();
                device.MeasuresArrived -= OnMeasuresArrived;
                deviceHandlers.Remove(device);
            }

            callbackContainer.OnDeviceChangedState(new Models.DeviceUpdatedEventArgs(device, e.UpdateStatus));
        }
        /// <summary>
        /// Obsługa zdarzenia nadejscia pomiarów z urządzenia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMeasuresArrived(object sender, MeasuresArrivedEventArgs e)
        {
            callbackContainer.OnMeasuresArrived(e);
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            if (IsStarted)
            {
                broadcastScanner.DeviceInfoUpdated -= OnBroadcastDeviceInfoUpdated;
                broadcastScanner.Dispose();
                foreach (var item in deviceHandlers)
                {
                    item.MeasuresArrived -= OnMeasuresArrived;
                    item.Disconnected -= OnDeviceDisconnected;
                    item.Dispose();
                }
                deviceHandlers.Clear();
                callbackContainer.Dispose();
                IsStarted = false;
            }
        }
        #endregion
    }
}
