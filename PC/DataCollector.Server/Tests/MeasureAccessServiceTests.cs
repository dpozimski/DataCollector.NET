using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests
{
    /// <summary>
    /// Klasa testująca <see cref="MeasureAccessService"/>.
    /// </summary>
    public class MeasureAccessServiceTests
    {
        private readonly MeasureAccessService accessService;
        private readonly MeasureDevice device;

        public MeasureAccessServiceTests()
        {
            accessService = new MeasureAccessService(TestModelsFactory.TestConnectionString);
            device = new MeasureDevice()
            {
                MacAddress = "123"
            };
        }
         
        [Fact]
        public void GetMeasuresTest()
        {
            var measures = accessService.GetMeasures(MeasureType.AirPressure, device, DateTime.Now, DateTime.Now);
            Assert.Equal(0, measures.Count());
        }

        [Fact]
        public void GetSphereMeasuresTest()
        {
            var measures = accessService.GetSphereMeasures(SphereMeasureType.Accelerometer, device, DateTime.Now, DateTime.Now);
            Assert.Equal(0, measures.Count());
        }

        [Fact]
        public void GetMeasureDevicesTest()
        {
            var devices = accessService.GetMeasureDevices();
            Assert.Equal(0, devices.Count());
        }

        [Fact]
        public void UpdateMeasureDeviceFailTest()
        {
            Assert.Throws<InvalidOperationException>(() => accessService.UpdateDeviceRequestInterval("111", 0));
        }
    }
}
