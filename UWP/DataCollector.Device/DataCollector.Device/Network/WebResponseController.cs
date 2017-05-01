using DataCollector.Device.BusDevice;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.Network
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class WebResponseController
    {
        #region Private Fields
        private ILedControl ledController;
        private NetworkAccess networkAccess;
        #endregion

        #region ctor
        /// <summary>
        /// Konstruktor klasy WebResponseController.
        /// </summary>
        public WebResponseController()
        {
            this.ledController = StartupTask.NetworkController.LedController;
            this.networkAccess = StartupTask.NetworkController;
        }
        #endregion

        [UriFormat("/getLedState")]
        public IGetResponse GetLedState()
        {
            bool state = ledController.GetLedState();

            return new GetResponse(GetResponse.ResponseStatus.OK, state.ToString());
        }
        [UriFormat("/ledState?p={val}")]
        public IGetResponse ChangeLedState(string val)
        {
            bool bLedState = false;
            bool ledStateChangeRequest = bool.TryParse(val, out bLedState);

            bool success = false;

            if (ledStateChangeRequest)
                success = ledController.ChangeLedState(bLedState);

            return new GetResponse(GetResponse.ResponseStatus.OK, success.ToString());
        }
        [UriFormat("/measurements")]
        public IGetResponse GetMeasures()
        {
            string json = networkAccess.GetResponseData();

            return new GetResponse(GetResponse.ResponseStatus.OK, json);
        }
    }
}
