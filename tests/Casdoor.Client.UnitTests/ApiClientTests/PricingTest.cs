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
        public async Task TestPricing()
        {
            var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

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
            var responseAsync = userClient.AddPricingAsync(pricing);
            var response = TestUtils.AssertNotNull(await responseAsync);
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            var pricingsAsync = userClient.GetPricingsAsync();
            var getPricings = TestUtils.AssertNotNull(await pricingsAsync);
            Assert.True(getPricings.Any());
            _testOutputHelper.WriteLine($"{getPricings.Count()}");
            bool found = false;
            foreach (var casdoorPricing in getPricings)
            {
                _testOutputHelper.WriteLine(casdoorPricing.Name);
                if (casdoorPricing.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            var pricingAsync = userClient.GetPricingAsync(name);
            var getPricing = TestUtils.AssertNotNull(await pricingAsync);
            Assert.Equal(name, getPricing.Name);

            // Update the object
            const string updatedDescription = "Updated Casdoor Pricing";
            pricing.Description = updatedDescription;

            // Update the object
            responseAsync = userClient.UpdatePricingAsync(pricing);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            pricingAsync = userClient.GetPricingAsync(name);
            getPricing = TestUtils.AssertNotNull(await pricingAsync);
            Assert.Equal(updatedDescription, getPricing.Description);

            // Delete the object
            responseAsync = userClient.DeletePricingAsync(getPricing);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            pricingAsync = userClient.GetPricingAsync(name);
            var deletedPricing = await pricingAsync;
            Assert.Null(deletedPricing);
        }
    }
}
