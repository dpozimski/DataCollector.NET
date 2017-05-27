using DataCollector.Server.BroadcastListener.Exceptions;
using DataCollector.Server.BroadcastListener.Factories;
using DataCollector.Server.BroadcastListener.Interfaces;
using DataCollector.Server.BroadcastListener.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DataCollector.Server.BroadcastListener
{
    /// <summary>
    /// Klasa implementująca obsługę nasłuchiwania urządzeń opartych na systemie Windows IoT w sieci.
    /// </summary>
    public class BroadcastScanner : IBroadcastScanner
    {
        #region Constants
        /// <summary>
        /// Adres nasłuchu multicast.
        /// </summary>
        private readonly IPAddress multicastAddress = IPAddress.Parse("239.0.0.222");
        /// <summary>
        /// Port nasłuchu pakietów UDP.
        /// </summary>
        private int port = 8;
        #endregion

        #region Private Fields
        /// <summary>
        /// Lista nasłuchująych kanałów Broadcast.
        /// </summary>
        private BroadcastInterfaceMessageHandler[] broadcastListeners;
        /// <summary>
        /// Kontener urządzeń w sieci.
        /// </summary>
        private IDetectedDevicesContainer detectedDeviceContainer;
        /// <summary>
        /// Fabryka obiektów identyfkujących urządzenia.
        /// </summary>
        private IDevicesBroadcastInfoFactory devicesBroadcastInfoFactory;
        /// <summary>
        /// Fabryka adresów sieciowych.
        /// </summary>
        private INetworkAddressFactory networkAddressFactory;
        #endregion

        #region Events
        /// <summary>
        /// Zadzrenie wyzwalane podczas zmiany stanu urządzenia wykrytego w sieci.
        /// </summary>
        public event EventHandler<DeviceUpdatedEventArgs> DeviceInfoUpdated
        {
            add
            {
                detectedDeviceContainer.DeviceInfoUpdated += value;
            }
            remove
            {
                detectedDeviceContainer.DeviceInfoUpdated -= value;
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DevicesListener
        /// <paramref name="detectedDeviceContainer">Kontener znalezionych urządzeń</paramref>
        /// <paramref name="devicesBroadcastInfoFactory">Fabryka obiektów identyfkujących urządzenia</paramref>
        /// <paramref name="networkAddresFactory">Fabryka adresów sieciowych</paramref>
        /// </summary>
        public BroadcastScanner(INetworkAddressFactory networkAddressFactory, IDetectedDevicesContainer detectedDeviceContainer, IDevicesBroadcastInfoFactory devicesBroadcastInfoFactory)
        {
            this.networkAddressFactory = networkAddressFactory;
            this.devicesBroadcastInfoFactory = devicesBroadcastInfoFactory;
            this.detectedDeviceContainer = detectedDeviceContainer;
            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Rozpoczyna nasłuchiwanie na wybranych w konstruktorze adresach.
        /// </summary>
        public void StartListening()
        {
            broadcastListeners = GetUdpMulticastListeners().ToArray();
        }
        #endregion  

        #region Private Methods
        /// <summary>
        /// Zwraca listę wszystkich możliwych kanałów nasłuchujących.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BroadcastInterfaceMessageHandler> GetUdpMulticastListeners()
        {
            foreach (IPAddress ipAddress in networkAddressFactory.Create())
            {
                var broadcastListener = new BroadcastInterfaceMessageHandler(ipAddress, multicastAddress, port);
                broadcastListener.OnReceivedBytes += BroadcastListener_ReceivedMessage;
                broadcastListener.StartListening();
                yield return broadcastListener;
            }
        }

        /// <summary>
        /// Obsługa zdarzenia OnReceivedBytes dla kanału nasłuchującego.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BroadcastListener_ReceivedMessage(object sender, byte[] e)
        {
            try
            {
                IDeviceBroadcastInfo deviceInfo = devicesBroadcastInfoFactory.From(e);
                detectedDeviceContainer.Update(deviceInfo);
            }
            catch(InvalidFrameException)
            {
                //logger invalid frame exception
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            if(broadcastListeners != null)
            {
                foreach (var udpMulticastListener in broadcastListeners)
                    udpMulticastListener.Dispose();
            }
        }
        #endregion
    }
}
