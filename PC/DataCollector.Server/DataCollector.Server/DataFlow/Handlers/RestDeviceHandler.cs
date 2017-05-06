using DataCollector.Device.Models;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
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
        private const string GetMeasurementsRequest = "/api/measurements";
        private const string LedChangeRequest = "/api/ledState?p={0}";
        private const string LedStateRequest = "/api/getLedState";
        private const int Port = 45321;
        #endregion

        #region Private Fields
        private readonly object syncObj = new object();
        private Task measurementsRequestTask;
        private CancellationTokenSource tokenSource;
        private RestClient restClient;
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
        public RestDeviceHandler(DeviceBroadcastInfo broadcastInfo) : base(broadcastInfo)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Zwraca stan diody LED.
        /// </summary>
        /// <returns></returns>
        public bool GetLedState()
        {
            string data = GetRequest(string.Format(LedStateRequest));
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
                string data = GetRequest(string.Format(LedChangeRequest, state));
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
                restClient = new RestClient($"http://{IPv4}:{Port}");
                restClient.ReadWriteTimeout = 3000;
                restClient.Timeout = 3000;
                tokenSource = new CancellationTokenSource();

                success = (GetRequest(GetMeasurementsRequest) != null);

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
        /// Wykonuje zapytanie Get.
        /// </summary>
        /// <param name="restRequest">zapytanie</param>
        /// <returns>odpowiedź</returns>
        private string GetRequest(string restRequest)
        {
            var request = new RestRequest(restRequest, Method.GET);

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                return null;
            else
            {
                string data = response.Content.Replace("\\", string.Empty);
                data = new string(data.Skip(1).Take(data.Length - 2).ToArray());
                return data;
            }
        }
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
                    data = GetRequest(GetMeasurementsRequest);

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
