using DataCollector.Device.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace DataCollector.Device.BusDevice
{
    /// <summary>
    /// Klasa zarządzająca szyną I2C.
    /// </summary>
    public sealed class BusDevicesAccess : IDisposable
    {
        #region Private Fields
        private readonly object syncObject = new object();
        private CancellationTokenSource tokenSource;
        private List<I2CBusDevice> busDevices;
        private Task updaterTask;
        private const int UpdateMsInterval = 50;
        private GpioPin measureBusyLedIndicator;
        #endregion

        #region Public Events
        /// <summary>
        /// Zdarzenie wyzwalane podczas 
        /// </summary>
        public event EventHandler<Measures> OnMeasuresArrived;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy BusDevicesAccess.
        /// </summary>
        public BusDevicesAccess()
        {
            busDevices = new List<I2CBusDevice>();
            busDevices.Add(new BMP085());
            busDevices.Add(new MPU_6050());
            ILedControl ledControl = new PCF8574();
            busDevices.Add(ledControl as PCF8574);
            busDevices.Add(new Sensirion_SHT21());

            measureBusyLedIndicator = GpioController.GetDefault().OpenPin(47);
            measureBusyLedIndicator.SetDriveMode(GpioPinDriveMode.Output);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda implementująca obsługę pobierania pomiarów.
        /// </summary>
        private void UpdaterLoop()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                measureBusyLedIndicator.Write(GpioPinValue.High);

                Measures measures = new Measures();
                foreach (var item in busDevices)
                {
                    lock (syncObject)
                    {
                        try
                        {
                            item.UpdateData(ref measures);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Wystąpił błąd podczas aktualizacji danych pomiarowych dla {item.GetType()}\r\n" + ex.Message);
                        }
                    }
                }
                //powiadomienie subskrybujących o nadejściu nowego zestawu danych.
                OnMeasuresArrived?.Invoke(this, measures);

                measureBusyLedIndicator.Write(GpioPinValue.Low);
            }

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Metoda inicjująca serwis komunikacji szyny I2C.
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            measureBusyLedIndicator.Write(GpioPinValue.High);

            List<I2CBusDevice> inactiveDevices = new List<I2CBusDevice>();
            foreach (var item in busDevices)
            {
                bool success = item.InitCommunication().Result;
                if (!success)
                {
                    Debug.WriteLine($"Nie powiodła się konfiguracja urządzenia: {item.GetType()}");
                    inactiveDevices.Add(item);
                }
            }
            //usunięcie niekatywnych urządzeń
            foreach (var inactiveDevice in inactiveDevices)
                busDevices.Remove(inactiveDevice);

            tokenSource = new CancellationTokenSource();
            updaterTask = new Task(UpdaterLoop, tokenSource.Token);
            updaterTask.Start();

            measureBusyLedIndicator.Write(GpioPinValue.Low);
        }
        /// <summary>
        /// Zwraca kontroler diody LED.
        /// </summary>
        /// <returns></returns>
        public ILedControl GetLedController()
        {
            lock (syncObject)
            {
                return busDevices.Single(s => s is PCF8574) as PCF8574;
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Czyszczenie zasobów.
        /// </summary>
        public void Dispose()
        {
            tokenSource?.Cancel();
            updaterTask?.Wait();
            foreach (var item in busDevices)
                item.Dispose();
        }
        #endregion
    }
}
