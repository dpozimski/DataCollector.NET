using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollector.Device.Models;
using RestSharp;
using System.Net;
using DataCollector.Server.DeviceHandlers.Interfaces;

namespace DataCollector.Server.DeviceHandlers.Adapters
{
    /// <summary>
    /// Adapter komunikacji REST z wykorzystaniem biblioteki RestSharp
    /// </summary>
    public class RestSharpLibConnectionAdapter : IRestConnectionAdapter
    {
        private RestClient restClient;
        private IPAddress ip;
        private int port;

        /// <summary>
        /// Konstruktor nowej instancji klasy.
        /// </summary>
        /// <param name="configuration">Konfiguracja protokołu</param>
        public RestSharpLibConnectionAdapter(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        #region Public Methods
        /// <summary>
        /// Metoda nawiązująca komunikację
        /// </summary>
        public void Connect()
        {
            restClient = new RestClient($"http://{ip}:{port}");
            restClient.ReadWriteTimeout = 3000;
            restClient.Timeout = 3000;
        }

        /// <summary>
        /// Metoda rozłączająca połączenie.
        /// </summary>
        public void Disconnect()
        {
            //not necessary
        }
        /// <summary>
        /// Wykonuje zapytanie Get.
        /// </summary>
        /// <param name="restRequest">zapytanie</param>
        /// <returns>odpowiedź</returns>
        public string GetRequest(string restRequest)
        {
            var request = new RestRequest(restRequest, Method.GET);

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                return null;
            else
            {
                string data = response.Content.Replace("\\", string.Empty);
                data = new string(data.Skip(1).Take(data.Length - 2).ToArray());
                return data;
            }
        }
        #endregion
    }
}
