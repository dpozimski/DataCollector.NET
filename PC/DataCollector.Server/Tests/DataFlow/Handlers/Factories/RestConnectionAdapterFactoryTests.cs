using DataCollector.Server.DeviceHandlers.Factories;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.Handlers.Factories
{
    /// <summary>
    /// Klasa testująca <see cref="RestConnectionAdapterFactory"/>
    /// </summary>
    public class RestConnectionAdapterFactoryTests
    {
        private readonly RestConnectionAdapterFactory factory;

        public RestConnectionAdapterFactoryTests()
        {
            factory = new RestConnectionAdapterFactory();
        }

        [Fact]
        public void CreateArgumentNullExceptionTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => factory.Create(null, 100));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(65537)]
        [InlineData(165536)]
        public void CreateArgumentExceptionTest(int port)
        {
            Assert.Throws(typeof(ArgumentException), () => factory.Create(NetworkTestsUtils.Localhost, port));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(65536)]
        [InlineData(8214)]
        public void CreateWithoutExceptionsTest(int port)
        {
            Assert.NotNull(factory.Create(NetworkTestsUtils.Localhost, port));
        }
    }
}
