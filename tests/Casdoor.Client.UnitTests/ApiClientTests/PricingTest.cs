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
    public class PricingTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public PricingTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async void TestPricing()
        {
            var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

            const string ownerName = "casbin";
            string name = TestUtils.GetRandomName("Pricing");

            var pricing = new CasdoorPricing()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DisplayName = name,
                Application = "app-admin",
		        Description = "Casdoor Website",
            };

            // Add a new object
            Task<CasdoorResponse?> responseAsync = userClient.AddPricingAsync(pricing);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            Task<IEnumerable<CasdoorPricing>?> pricingsAsync = userClient.GetPricingsAsync();
            IEnumerable<CasdoorPricing>? getPricings = await pricingsAsync;
            Assert.True(getPricings.Any());
            _testOutputHelper.WriteLine($"{getPricings.Count()}");
            bool found = false;
            foreach (CasdoorPricing casdoorPricing in getPricings)
            {
                _testOutputHelper.WriteLine(casdoorPricing.Name);
                if (casdoorPricing.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            Task<CasdoorPricing?> pricingAsync = userClient.GetPricingAsync(name);
            CasdoorPricing? getPricing = await pricingAsync;
            Assert.Equal(name, getPricing.Name);

            // Update the object
            const string updatedDescription = "Updated Casdoor Pricing";
            pricing.Description = updatedDescription;

            // Update the object
            responseAsync = userClient.UpdatePricingAsync(pricing);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            pricingAsync = userClient.GetPricingAsync(name);
            getPricing = await pricingAsync;
            Assert.Equal(updatedDescription, getPricing.Description);

            // Delete the object
            responseAsync = userClient.DeletePricingAsync(getPricing);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            pricingAsync = userClient.GetPricingAsync(name);
            getPricing = await pricingAsync;
            Assert.Null(getPricing);
        }
    }
}
