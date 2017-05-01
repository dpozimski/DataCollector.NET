using System;
using System.IO;
using Restup.Webserver.Rest;
using Restup.Webserver.Http;
using System.Threading.Tasks;
using DataCollector.Device.Network;
using Newtonsoft.Json;
using DataCollector.Device.BusDevice;

namespace DataCollector.Device.Network
{
    /// <summary>
    /// Klasa implementująca obsługę zapytań WebSocket.
    /// </summary>
    public sealed class NetworkAccess : IDisposable
    {
        #region Constants
        private const int Port = 45321;
        #endregion

        #region Private Fields
        private readonly object syncObject = new object();
        private HttpServer server;
        private string data;
        #endregion

        #region Public Properties
        /// <summary>
        /// Kontroler diody LED.
        /// </summary>
        public ILedControl LedController { get; private set; }
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy NetworkAccess.
        /// <param name="ledControl">kontroler diody LED</param>
        /// </summary>
        public NetworkAccess(ILedControl ledControl)
        {
            this.LedController = ledControl;

            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<WebResponseController>();

            var configuration = new HttpServerConfiguration()
               .ListenOnPort(Port)
               .RegisterRoute("api", restRouteHandler)
               .EnableCors();

            server = new HttpServer(configuration);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ustawia nową odpowiedź dla klienta.
        /// </summary>
        /// <param name="data">obiekt wyjściowy</param>
        public void SetResponseData(object data)
        {
            lock (syncObject)
                this.data = JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// Zwraca wiadomość zwrotną do klienta.
        /// </summary>
        /// <returns></returns>
        public string GetResponseData()
        {
            lock (syncObject)
                return data;
        }
        /// <summary>
        /// Uruchamia serwer WebSocket.
        /// </summary>
        public void Start()
        {
            server.StartServerAsync().Wait();
        }
        /// <summary>
        /// Zatrzymuje serwer WebSocket.
        /// </summary>
        public void Stop()
        {
            server.StopServer();
        }
        #endregion

        #region IDispoable
        /// <summary>
        /// Zwolnienie zasobów.
        /// </summary>
        public void Dispose()
        {
            server?.StopServer();
        }
        #endregion
    }
}
