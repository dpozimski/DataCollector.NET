using DataCollector.Server.DataFlow.BroadcastListener.Factories;
using DataCollector.Server.DataFlow.BroadcastListener.Interfaces;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests.DataFlow.BroadcastListener.Factories
{
    /// <summary>
    /// Klasa testująca <see cref=">AvailableNetworkAddressFactory"/>
    /// </summary>
    public class AvailableNetworkAddressFactoryTests
    {
        private readonly INetworkAddressFactory addressFactory;

        public AvailableNetworkAddressFactoryTests()
        {
            addressFactory = new AvailableNetworkAddressFactory();
        }

        [Fact]
        public void TestAvailableAddressesWithInternalMethods()
        {
            var availableForUseAddresses = addressFactory.Create().Select(s=>s.ToString());
            var systemAddressess = NetworkInterface.GetAllNetworkInterfaces().
                SelectMany(networkInterface => networkInterface.GetIPProperties().UnicastAddresses).
                Select(s => s.Address.ToString());
            Assert.True(availableForUseAddresses.All(s => systemAddressess.Contains(s)));
        }
    }
}
