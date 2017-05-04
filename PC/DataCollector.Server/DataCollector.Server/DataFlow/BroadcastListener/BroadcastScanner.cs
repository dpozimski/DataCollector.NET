using DataCollector.Server.DataFlow.BroadcastListener.Exceptions;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using DataCollector.Server.DataFlow.BroadcastListener.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DataCollector.Server.DataFlow.BroadcastListener
{
    /// <summary>
    /// Klasa implementująca obsługę nasłuchiwania urządzeń opartych na systemie Windows IoT w sieci.
    /// </summary>
    public class BroadcastScanner : IDisposable
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
        private readonly BroadcastInterfaceMessageHandler[] broadcastListeners;
        /// <summary>
        /// Kontener urządzeń w sieci.
        /// </summary>
        private IDetectedDevicesContainer detectedDeviceContainer;
        /// <summary>
        /// Fabryka obiektów identyfkujących urządzenia.
        /// </summary>
        private IDevicesBroadcastInfoFactory devicesBroadcastInfoFactory;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy DevicesListener
        /// <paramref name="detectedDeviceContainer">Kontener znalezionych urządzeń</paramref>
        /// <paramref name="devicesBroadcastInfoFactory">Fabryka obiektów identyfkujących urządzenia</paramref>
        /// </summary>
        public BroadcastScanner(IDetectedDevicesContainer detectedDeviceContainer, IDevicesBroadcastInfoFactory devicesBroadcastInfoFactory)
        {
            this.devicesBroadcastInfoFactory = devicesBroadcastInfoFactory;
            this.detectedDeviceContainer = detectedDeviceContainer;
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
            foreach (IPAddress ipAddress in GetIpAddresses())
            {
                var broadcastListener = new BroadcastInterfaceMessageHandler(ipAddress, multicastAddress, port);
                broadcastListener.OnReceivedBytes += BroadcastListener_ReceivedMessage;
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
                DeviceBroadcastInfo deviceInfo = devicesBroadcastInfoFactory.From(e);
                detectedDeviceContainer.Update(deviceInfo);
            }
            catch(InvalidFrameException)
            {
                //logger invalid frame exception
            }
        }
        /// <summary>
        /// Zwraca liste wszystkich możliwych adresów IP, na których możliwy jest nasłuch gniazda broadcast.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IPAddress> GetIpAddresses()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(networkInterface =>
                    networkInterface.Supports(NetworkInterfaceComponent.IPv4) &&
                    networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses)
                .Where(unicastIpAddressInformation => unicastIpAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(unicastIpAddressInformation => unicastIpAddressInformation.Address);
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            foreach (var udpMulticastListener in broadcastListeners)
                udpMulticastListener.Dispose();
        }
        #endregion
    }
}
