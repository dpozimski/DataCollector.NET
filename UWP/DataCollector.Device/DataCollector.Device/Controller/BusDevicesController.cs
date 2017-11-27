using DataCollector.Device.BusDevice;
using DataCollector.Device.Data;
using DataCollector.Device.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace DataCollector.Device.Controller
{
    /// <summary>
    /// The i2C bus management class.
    /// </summary>
    public sealed class BusDevicesController : IDisposable
    {
        #region Private Fields
        private IMeasuresDataController measuresHandler;
        private CancellationTokenSource tokenSource;
        private List<I2CBusDevice> busDevices;
        private Task updaterTask;
        private const int UpdateMsInterval = 50;
        private GpioPin measureBusyLedIndicator;
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// <paramref name="busDevices">the bus devices collection</paramref>
        /// <paramref name="measuresHandler">measures handler</paramref>
        /// </summary>
        public BusDevicesController(IEnumerable<I2CBusDevice> busDevices, IMeasuresDataController measuresHandler)
        {
            this.busDevices = new List<I2CBusDevice>(busDevices);
            this.measuresHandler = measuresHandler;

            measureBusyLedIndicator = GpioController.GetDefault().OpenPin(47);
            measureBusyLedIndicator.SetDriveMode(GpioPinDriveMode.Output);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// The loop for getting a new measures.
        /// </summary>
        private void UpdaterLoop()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                measureBusyLedIndicator.Write(GpioPinValue.High);

                Measures measures = new Measures();
                foreach (var item in busDevices)
                {
                    try
                    {
                        item.UpdateData(ref measures);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"There was an error of getting measures from module {item.GetType()}\r\n" + ex.Message);
                    }
                }
                //notify about new measures
                measuresHandler.NewestMeasure = measures;

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
