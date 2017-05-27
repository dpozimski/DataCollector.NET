using AutoMapper;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using DataCollector.Server.DeviceHandlers.Interfaces;
using DataCollector.Server.DeviceHandlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollector.Server.DeviceHandlers
{
    /// <summary>
    /// Klasa implementująca funckjonalność symulatora urządzenia pomiarowego.
    /// </summary>
    public class SimulatorDeviceHandler : DeviceBroadcastInfo, IDeviceHandler
    {
        #region Private Fields
        /// <summary>
        /// Stan diody LED.
        /// </summary>
        private bool ledState;
        /// <summary>
        /// Zadanie pobierania danych.
        /// </summary>
        private Task testDataTask;
        /// <summary>
        /// Obiekt anulujący zadanie <see cref="testDataTask"/>.
        /// </summary>
        private CancellationTokenSource tokenSource;
        #endregion

        #region Public Properties
        /// <summary>
        /// Zwraca czy metoda <see cref="Connect"/> została wykonana.
        /// </summary>
        public bool IsConnected { get; private set; }
        /// <summary>
        /// Interwał pobierania pomiarów z urządzenia.
        /// </summary>
        public double MeasurementsMsRequestInterval { get; set; } = 3000;
        #endregion

        #region Public Events
        /// <summary>
        /// Zdarzenie wyzwalane podczas pomyslnego pobrania pomiarów.
        /// </summary>
        public event EventHandler<MeasuresArrivedEventArgs> MeasuresArrived;
        /// <summary>
        /// Zdarzenie rozłaczenia urządzenia.
        /// </summary>
        public event EventHandler<IDeviceHandler> Disconnected;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor symulatora komuikacji z urządzeniem.
        /// </summary>
        /// <param name="info"></param>
        public SimulatorDeviceHandler(IDeviceBroadcastInfo info) : base(info)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Metoda symulująca proces połączenia z urządzeniem.
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            tokenSource = new CancellationTokenSource();
            testDataTask = new Task(TestDataLoop, tokenSource.Token);
            testDataTask.Start();
            IsConnected = true;
            return true;
        }
        /// <summary>
        /// Zwraca wirtualny stan diody LED.
        /// </summary>
        /// <returns></returns>
        public bool GetLedState()
        {
            return ledState;
        }
        /// <summary>
        /// Symulowana zmiana stanu diody LED.
        /// </summary>
        /// <param name="state">stan</param>
        /// <returns></returns>
        public bool ChangeLedState(bool state)
        {
            return ledState = state;
        }
        /// <summary>
        /// Symulacja procesu rozłączenia z urządzeniem pomiarowym.
        /// </summary>
        /// <returns>False jeśli wystąpił błąd podczas anulowania wątku</returns>
        public bool Disconnect()
        {
            try
            {
                tokenSource.Cancel();
                testDataTask.Wait();
                return true;
            }
            catch(AggregateException errors)
            {
                errors.Handle(e => e is TaskCanceledException);
                return false;
            }
            finally
            {
                IsConnected = false;
                Disconnected?.Invoke(this, this);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda pobierająca symulowane dane pomiarowe.
        /// </summary>
        private void TestDataLoop()
        {
            try
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    Task.Delay(TimeSpan.FromMilliseconds(MeasurementsMsRequestInterval), tokenSource.Token).Wait(tokenSource.Token);
                    MeasuresArrived?.Invoke(this, new MeasuresArrivedEventArgs(Mapper.Map<DeviceInfo>(this), new Device.Models.Measures()
                    {
                        Accelerometer = new Device.Models.SpherePoint()
                        {
                            X = (float)rand.NextDouble(),
                            Y = (float)rand.NextDouble(),
                            Z = (float)rand.NextDouble()
                        },
                        AirPressure = (float)rand.NextDouble(),
                        Gyroscope = new Device.Models.SpherePoint()
                        {
                            X = (float)rand.NextDouble(),
                            Y = (float)rand.NextDouble(),
                            Z = (float)rand.NextDouble()
                        },
                        Humidity = (float)rand.NextDouble(),
                        IsLedActive = rand.Next(0, 255) > 127,
                        Temperature = (float)rand.NextDouble()
                    }, DateTime.Now));
                }
            }
            catch (TaskCanceledException)
            {
                //anulowanie zadania
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zajmowanych zasobów.
        /// </summary>
        public void Dispose()
        {
            if (IsConnected)
                Disconnect();
        }
        #endregion
    }
}
