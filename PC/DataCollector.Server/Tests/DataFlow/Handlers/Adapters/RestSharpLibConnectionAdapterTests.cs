using DataCollector.Server.DataFlow.Handlers.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.Handlers.Adapters
{
    /// <summary>
    /// Klasa testująca <see cref="RestSharpLibConnectionAdapter"/>
    /// </summary>
    public class RestSharpLibConnectionAdapterTests
    {
        private const string testRestService = "jsonplaceholder.typicode.com";
        private const int port = 443;
        private readonly RestSharpLibConnectionAdapter restSharpLibConnectionAdapter;
        
        public RestSharpLibConnectionAdapterTests()
        {
            var host = Dns.GetHostAddresses(testRestService)[0];
            this.restSharpLibConnectionAdapter = new RestSharpLibConnectionAdapter(host, port);
        }

        [Fact]
        public void TestConnectMethodWithoutAnyExceptions()
        {
            restSharpLibConnectionAdapter.Connect();
        }

        [Fact]
        public void ResponseTestBySourceIntegrity()
        {
            const string testRequest = "posts/1";
            restSharpLibConnectionAdapter.Connect();
            string response = restSharpLibConnectionAdapter.GetRequest(testRequest);
            Assert.NotNull(response);
        }

        [Fact]
        public void TestDisconnectMethodWithoutAnyExceptionsAfterConnect()
        {
            restSharpLibConnectionAdapter.Connect();
            restSharpLibConnectionAdapter.Disconnect();
        }
    }
}
