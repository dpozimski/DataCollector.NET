using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollector.Server.DataFlow.BroadcastListener
{
    /// <summary>
    /// Klasa implementująca nasłuch Broadcast na pojedynczym kanale.
    /// </summary>
    public class DeviceBroadcastMessageHandler : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Maksymlana wielkość przychodzącego pakietu UDP.
        /// </summary>
        private const int MaxUdpSize = 65536;
        /// <summary>
        /// Numer serwisu nasłuchującego.
        /// </summary>
        private readonly int port = 6;
        /// <summary>
        /// Adres IP nasłuchującego interfejsu.
        /// </summary>
        private readonly IPAddress ip;
        /// <summary>
        /// Adres ip multicast.
        /// </summary>
        private readonly IPAddress multicastAddress;
        /// <summary>
        /// Zadanie główne.
        /// </summary>
        private Task task;
        /// <summary>
        /// Token anulujący zadanie.
        /// </summary>
        private readonly CancellationTokenSource tokenSource;
        /// <summary>
        /// Gniazdo połączeniowe.
        /// </summary>
        private Socket socket;
        #endregion

        #region Events
        /// <summary>
        /// Zdarzenie wyzwalane podczas nadejścia nowej ramki z kanału broadcast.
        /// </summary>
        public event EventHandler<byte[]> OnReceivedBytes;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy BroadcastListener.
        /// </summary>
        /// <param name="ip">adres ip nasłuchu</param>
        /// <param name="multicastAddress">multicast address for frames listening</param>
        /// <param name="port">listening port</param>
        public DeviceBroadcastMessageHandler(IPAddress ip, IPAddress multicastAddress, int port)
        {
            this.tokenSource = new CancellationTokenSource();
            this.ip = ip;
            this.multicastAddress = multicastAddress;
            this.port = port;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Uruchamia nasłuchiwanie na wskazanym interfejsie sieciowym.
        /// </summary>
        public void StartListening()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            var endPoint = new IPEndPoint(ip, port);
            socket.Bind(endPoint);
            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.AddMembership,
                new MulticastOption(multicastAddress, ip));

            task = Task.Factory.StartNew(ReceiverMethod, tokenSource.Token);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Metoda implementująca obsługę odbierania pakietów sieciowych ze wskazanego gniazda.
        /// </summary>
        /// <param name="socket">gniazdo</param>
        private void ReceiverMethod()
        {
            var buffer = new byte[MaxUdpSize];

            try
            {
                while (!tokenSource.IsCancellationRequested)
                {
                    int receivedLength = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                    if (receivedLength == 0)
                        return;

                    if (receivedLength < 0)
                        continue;

                    var receivedBytes = new byte[receivedLength];

                    Array.Copy(buffer, receivedBytes, receivedLength);

                    OnReceivedBytes?.Invoke(this, receivedBytes);
                }
            }
            catch (SocketException ex)
            {
                //rozłaczanie socketa
                if (ex.ErrorCode != 10004)
                    throw ex;
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            socket.Shutdown(SocketShutdown.Both);
            tokenSource.Cancel();
            socket?.Dispose();
            task.Wait();
            socket = null;
        }
        #endregion
    }
}
