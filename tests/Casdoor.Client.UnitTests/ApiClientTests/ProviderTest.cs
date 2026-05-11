using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace Casdoor.Client.UnitTests.ApiClientTests
{
    public class ProviderTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public ProviderTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestProvider()
        {
            var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

            const string ownerName = "admin";
            string name = TestUtils.GetRandomName("Provider");

            var provider = new CasdoorProvider()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DisplayName = name,
                Category = "Captcha",
                Type = "Default"
            };

            // Add a new object
            Task<CasdoorResponse?> responseAsync = userClient.AddProviderAsync(provider);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            Task<IEnumerable<CasdoorProvider>?> providersAsync = userClient.GetProvidersAsync();
            IEnumerable<CasdoorProvider>? getProviders = await providersAsync;
            Assert.True(getProviders.Any());
            _testOutputHelper.WriteLine($"{getProviders.Count()}");
            bool found = false;
            foreach (CasdoorProvider casdoorProvider in getProviders)
            {
                _testOutputHelper.WriteLine(casdoorProvider.Name);
                if (casdoorProvider.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            Task<CasdoorProvider?> providerAsync = userClient.GetProviderAsync(name);
            CasdoorProvider? getProvider = await providerAsync;
            Assert.Equal(name, getProvider.Name);

            // Update the object
            const string updatedDisplayName = "Updated Casdoor Website";
            provider.DisplayName = updatedDisplayName;

            // Update the object
            responseAsync = userClient.UpdateProviderAsync(provider.Name, provider);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            providerAsync = userClient.GetProviderAsync(name);
            getProvider = await providerAsync;
            Assert.Equal(updatedDisplayName, getProvider.DisplayName);

            // Delete the object
            responseAsync = userClient.DeleteProviderAsync(name);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            providerAsync = userClient.GetProviderAsync(name);
            getProvider = await providerAsync;
            Assert.Null(getProvider);
        }
    }
}
