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
    /// Class provides a network functionality.
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
        /// The constructor.
        /// <param name="ledController">the led controller</param>
        /// <paramref name="measuresController">the measures controller</paramref>
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
        /// Runs the WebSocket server.
        /// </summary>
        public void Start()
        {
            server.StartServerAsync().Wait();
        }
        /// <summary>
        /// Stops the server,
        /// </summary>
        public void Stop()
        {
            server.StopServer();
        }
        #endregion

        #region IDispoable
        /// <summary>
        /// Releases managed resources.
        /// </summary>
        public void Dispose()
        {
            server?.StopServer();
        }
        #endregion
    }
}
