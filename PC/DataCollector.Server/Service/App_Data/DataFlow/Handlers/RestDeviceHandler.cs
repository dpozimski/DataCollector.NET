using DataCollector.Device.Models;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using DataCollector.Server.DataFlow.Handlers.Adapters;
using DataCollector.Server.DataFlow.Handlers.Interfaces;
using DataCollector.Server.Interfaces;
using DataCollector.Server.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.Handlers
{
    /// <summary>
    /// Klasa definiująca urządzenie pomiarowe wykorzystujące komunikację z wykorzystaniem REST HTTP.
    /// </summary>
    public class RestDeviceHandler : DeviceBroadcastInfo, IDeviceHandler
    {
        #region Constants
        private TimeSpan measurementsRequestInterval = TimeSpan.FromMilliseconds(3000);
        #endregion

        #region Private Fields
        private readonly object syncObj = new object();
        private readonly IDeviceHandlerConfiguration configuration;
        private readonly IRestConnectionAdapter restConnectionAdapter;
        private Task measurementsRequestTask;
        private CancellationTokenSource tokenSource;
        #endregion

        #region Public Properties
        /// <summary>
        /// Połączono z serwerem.
        /// </summary>
        public bool IsConnected { get; protected set; }
        /// <summary>
        /// Interwal pobierania pomiarów.
        /// </summary>
        public double MeasurementsMsRequestInterval
        {
            get
            {
                lock (syncObj)
                    return measurementsRequestInterval.TotalMilliseconds;
            }
            set
            {
                lock (syncObj)
                    measurementsRequestInterval = TimeSpan.FromMilliseconds(value); ;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Zdarzenie nadejścia pomiarów.
        /// </summary>
        public event EventHandler<MeasuresArrivedEventArgs> MeasuresArrived;
        /// <summary>
        /// Sygnalizacja zerwaniu połączenia.
        /// </summary>
        public event EventHandler<IDeviceHandler> Disconnected;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy CommunicationDeviceHandler.
        /// </summary>
        /// <param name="broadcastInfo">informacje o urządzeniu pochodzące z broadcast</param>
        /// <param name="configuration">Konfiguracja api REST</param>
        /// <param name="restConnectionAdapter">adapter komunikacja protkołu REST</param>
        public RestDeviceHandler(IRestConnectionAdapter restConnectionAdapter, IDeviceHandlerConfiguration configuration, IDeviceBroadcastInfo broadcastInfo) : base(broadcastInfo)
        {
            this.restConnectionAdapter = restConnectionAdapter;
            this.configuration = configuration;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <returns></returns>
        public bool GetLedState()
        {
            string data = restConnectionAdapter.GetRequest(string.Format(configuration.LedStateRequest));
            return bool.Parse(data);
        }
        /// <summary>
        /// Zmiana stanu diody LED.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool ChangeLedState(bool state)
        {
            bool success = false;
            lock (syncObj)
            {
                string data = restConnectionAdapter.GetRequest(string.Format(configuration.LedChangeRequest, state));
                success = bool.Parse(data);
            }
            return success;
        }
        /// <summary>
        /// Rozłączenie z serwerem.
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            bool success = false;

            if (IsConnected)
            {
                tokenSource.Cancel();
                measurementsRequestTask.Wait();

                IsConnected = false;

                success = true;
                Disconnected?.Invoke(this, this);
            }

            return success;
        }
        /// <summary>
        /// Połączenie z serwerem pomiarów.
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            bool success = false;
            if (!IsConnected)
            {
                restConnectionAdapter.Connect();
                tokenSource = new CancellationTokenSource();
                success = (restConnectionAdapter.GetRequest(configuration.GetMeasurementsRequest) != null);
                if (success)
                {
                    measurementsRequestTask = new Task(MeasurementsRequestLoop, tokenSource.Token);
                    measurementsRequestTask.Start();
                    IsConnected = true;
                }
            }
            return success;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa zadania pobierania pomiarów.
        /// </summary>
        /// <param name="state"></param>
        private void MeasurementsRequestLoop(object state)
        {
            while (!tokenSource.IsCancellationRequested)
            {
                string data = null;

                lock (syncObj)
                    data = restConnectionAdapter.GetRequest(configuration.GetMeasurementsRequest);

                if (data != null)
                {
                    Measures measures = JsonConvert.DeserializeObject<Measures>(data);
                    Task.Factory.StartNew(new Action(() =>
                            MeasuresArrived?.Invoke(this, new MeasuresArrivedEventArgs(this, measures, DateTime.Now))));
                }
                else
                    Disconnected?.Invoke(this, this);

                Task.Delay(measurementsRequestInterval).Wait();
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwalnia zasoby.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }
        #endregion
    }
}
