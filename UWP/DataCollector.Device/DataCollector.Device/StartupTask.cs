using Windows.ApplicationModel.Background;
using System;
using System.Diagnostics;
using DataCollector.Device.BusDevice;
using DataCollector.Device.Network;
using DataCollector.Device.Models;

namespace DataCollector.Device
{
    /// <summary>
    /// Klasa stanowiąca punkt wejścia do programu.
    /// </summary>
    public sealed class StartupTask : IBackgroundTask
    {
        #region Fields
        private BackgroundTaskDeferral deferral;
        #endregion

        #region Public Properties
        public static BusDevicesAccess BusDevices { get; private set; }
        public static NetworkAccess NetworkController { get; private set; }
        #endregion

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //pobranie przydzielonych zasobów na zadanie
            deferral = taskInstance.GetDeferral();
            //inicjalizacja dostępów do modułów
            BusDevices = new BusDevicesAccess();
            NetworkController = new NetworkAccess(BusDevices.GetLedController());
            //uruchomienie zadania odpytywania urządzeń I2C o dane
            BusDevices.Init();
            BusDevices.OnMeasuresArrived += new EventHandler<Measures>(OnMeasuresArrived);
            //uruchomienie serwisu WebSocket
            NetworkController.Start();
        }

        /// <summary>
        /// Obsługa zdarzenia nadejścia nowych danych pomiarowych.
        /// </summary>
        /// <param name="sender">źródło</param>
        /// <param name="e">pomiary</param>
        private void OnMeasuresArrived(object sender, Measures e)
        {
            Debug.WriteLine("--------------------");
            Debug.WriteLine(string.Empty);
            Debug.WriteLine(DateTime.Now);
            Debug.WriteLine(e.ToString());
            //zapisanie wartości w cache
            NetworkController.SetResponseData(e);
        }
    }
}
