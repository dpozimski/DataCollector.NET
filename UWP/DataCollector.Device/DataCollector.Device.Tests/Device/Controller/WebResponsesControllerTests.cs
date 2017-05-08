using DataCollector.Device.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Device.Tests.Device.Controller
{
    public class WebResponsesControllerTests
    {
        private readonly WebResponseController webResponesController;

        public WebResponsesControllerTests()
        {
            webResponesController = new WebResponseController();
        }

        [Fact]
        public void TestGetMEasurements()
        {
            var data = webResponesController.GetMeasures();
            Assert.NotNull(data);
        }
    }
}
