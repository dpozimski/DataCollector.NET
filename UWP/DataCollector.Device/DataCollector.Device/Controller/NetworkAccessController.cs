using System;
using System.IO;
using Restup.Webserver.Rest;
using Restup.Webserver.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataCollector.Device.BusDevice;
using DataCollector.Device.Data;

namespace DataCollector.Device.Controller
{
    /// <summary>
    /// Klasa implementująca obsługę zapytań WebSocket.
    /// </summary>
    public sealed class NetworkAccessController : IDisposable
    {
        #region Constants
        private const int Port = 45321;
        #endregion

        #region Private Fields
        private readonly object syncObject = new object();
        private HttpServer server;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy NetworkAccess.
        /// <param name="ledControl">kontroler diody LED</param>
        /// </summary>
        public NetworkAccessController(ILedController ledController, IMeasuresDataController measuresController)
        {
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<WebResponseController>(ledController, measuresController);

            var configuration = new HttpServerConfiguration()
               .ListenOnPort(Port)
               .RegisterRoute("api", restRouteHandler)
               .EnableCors();

            server = new HttpServer(configuration);
        }
        #endregion

        #region Public Methods
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
