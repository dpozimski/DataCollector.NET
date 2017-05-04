﻿using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener
{
    /// <summary>
    /// Klasa stanowiąca kontener dla odnalezionych urządzeń w sieci z obsługa ich usuwania w przypadku utraty ważności.
    /// </summary>
    public class CachedDetectedDevicesContainer : IDetectedDevicesContainer
    {
        #region Private Fields
        /// <summary>
        /// Bufor informacji o urządzeniach w sieci.
        /// </summary>
        private readonly ConcurrentDictionary<string, TimestampedDeviceInfo> cachedInfo;
        /// <summary>
        /// Czas po którym następuje sprawdzenie o przekroczeniu ważności informacji o urządzeniu.
        /// </summary>
        private readonly TimeSpan cleanupCacheInterval = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Timer sprawdzający utratę ważności informacji o urządzeniach.
        /// </summary>
        private readonly Timer cleanupCacheTimer;
        #endregion

        #region Events
        /// <summary>
        /// Zdarzenie wyzwalane podczas aktualizacji informacji o urządzeniu.
        /// </summary>
        public event EventHandler<DeviceUpdatedEventArgs> DeviceInfoUpdated;
        #endregion

        #region Public Properties
        /// <summary>
        /// Enumeracja urządzeń w sieci.
        /// </summary>
        public IEnumerable<DeviceBroadcastInfo> Devices
        {
            get
            {
                return cachedInfo.Values.Select(x => x.Info);
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor nowego obiektu klasy.
        /// </summary>
        public CachedDetectedDevicesContainer()
        {
            cachedInfo = new ConcurrentDictionary<string, TimestampedDeviceInfo>();
            cleanupCacheTimer = new Timer(CleanupExpiredDevices, null, cleanupCacheInterval, cleanupCacheInterval);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Usuwa urządzenie z pamięci tymczasowej.
        /// </summary>
        /// <param name="device">urządzenie</param>
        public void ClearDeviceCache(DeviceBroadcastInfo device)
        {
            TimestampedDeviceInfo timestampedDeviceInfo = null;
            cachedInfo.TryRemove(device.MacAddress, out timestampedDeviceInfo);
        }
        /// <summary>
        /// Metoda aktualizująca słownik o nowe informacje o urządzeniu zwracając tym samym status aktualizacji.
        /// Wywołuje zdarzenie aktualizacji danych o urządzeniu.
        /// </summary>
        /// <param name="device">urządzenie</param>
        /// <returns></returns>
        public void Update(DeviceBroadcastInfo device)
        {
            UpdateStatus updateStatus;
            TimestampedDeviceInfo existing;
            if (cachedInfo.TryGetValue(device.MacAddress, out existing))
            {
                updateStatus = UpdateStatus.Updated;
                existing.LastUpdate = DateTime.Now;

                if (existing.Info.Equals(device))
                    return;

                existing.Info = device;
            }
            else
            {
                updateStatus = UpdateStatus.Found;
                cachedInfo.TryAdd(device.MacAddress, new TimestampedDeviceInfo(device));
            }

            DeviceInfoUpdated?.Invoke(this, new DeviceUpdatedEventArgs(device, updateStatus));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Obsługa zdarzenie wywołanego przez cleanupCacheTimer.
        /// </summary>
        /// <param name="state"></param>
        private void CleanupExpiredDevices(object state)
        {
            IEnumerable<DeviceBroadcastInfo> expiredDevices;

            expiredDevices = cachedInfo
                .Where(s => s.Value.IsExpired).Select(s => s.Value.Info);

            foreach (DeviceBroadcastInfo deviceInfo in expiredDevices)
            {
                TimestampedDeviceInfo timeStampedDeviceInfo = null;
                if (cachedInfo.TryRemove(deviceInfo.MacAddress, out timeStampedDeviceInfo))
                    DeviceInfoUpdated?.Invoke(this, new DeviceUpdatedEventArgs(timeStampedDeviceInfo.Info, UpdateStatus.Lost));
                else
                    throw new InvalidOperationException("Próba usunięcia urządzenia z buforu nie powiodła się");
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            cleanupCacheTimer?.Dispose();
            cachedInfo.Clear();
        }
        #endregion
    }
}