using DataCollector.Device.BusDevice;
using DataCollector.Device.Data;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Device.Controller
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class WebResponseController
    {
        #region Private Fields
        private ILedController ledController;
        private IMeasuresDataController measuresHandler;
        #endregion

        #region ctor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="args">dependencies collection</param>
        public WebResponseController(params object[] args)
        {
            IEnumerable<object> dep = args.AsEnumerable();
            this.ledController = dep.OfType<ILedController>().First();
            this.measuresHandler = dep.OfType<IMeasuresDataController>().First();
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
            string json = measuresHandler.NewestMeasureOutput;
            return new GetResponse(GetResponse.ResponseStatus.OK, json);
        }
    }
}
