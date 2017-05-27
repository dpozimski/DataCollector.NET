using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCollector.Server.BroadcastListener
{
    /// <summary>
    /// Klasa implementująca nasłuch Broadcast na pojedynczym kanale.
    /// </summary>
    public class BroadcastInterfaceMessageHandler : IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Maksymlana wielkość przychodzącego pakietu UDP.
        /// </summary>
        private const int MaxUdpSize = 65536;
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

        #region Public Properties
        /// <summary>
        /// Adres IP nasłuchującego interfejsu.
        /// </summary>
        public IPAddress IP { get; }
        /// <summary>
        /// Numer serwisu nasłuchującego.
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// Adres ip multicast.
        /// </summary>
        public IPAddress MulticastAddress { get; }
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
        public BroadcastInterfaceMessageHandler(IPAddress ip, IPAddress multicastAddress, int port)
        {
            this.tokenSource = new CancellationTokenSource();
            this.IP = ip;
            this.MulticastAddress = multicastAddress;
            this.Port = port;
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
            var endPoint = new IPEndPoint(IP, Port);
            socket.Bind(endPoint);
            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.AddMembership,
                new MulticastOption(MulticastAddress, IP));

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
            socket.Dispose();
            task.Wait();
            socket = null;
        }
        #endregion
    }
}
