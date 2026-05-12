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
    public class ProductTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public ProductTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestProduct()
        {
            var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

            const string ownerName = "casbin";
            string name = TestUtils.GetRandomName("Product");

            var product = new CasdoorProduct()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DisplayName = name,
                Image = "https://cdn.casbin.org/img/casdoor-logo_1185x256.png",
                Description = "Casdoor Website",
                Tag = "auto_created_product_for_plan",
                Quantity = 999,
                Sold = 0,
                State = "Published",
                Currency= "CNY"
            };

            // Add a new object
            Task<CasdoorResponse?> responseAsync = userClient.AddProductAsync(product);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            Task<IEnumerable<CasdoorProduct>?> productsAsync = userClient.GetProductsAsync();
            IEnumerable<CasdoorProduct>? getProducts = await productsAsync;
            Assert.True(getProducts.Any());
            _testOutputHelper.WriteLine($"{getProducts.Count()}");
            bool found = false;
            foreach (CasdoorProduct casdoorProduct in getProducts)
            {
                _testOutputHelper.WriteLine(casdoorProduct.Name);
                if (casdoorProduct.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            Task<CasdoorProduct?> productAsync = userClient.GetProductAsync(name);
            CasdoorProduct? getProduct = await productAsync;
            Assert.Equal(name, getProduct.Name);

            // Update the object
            const string updatedDescription = "Updated Casdoor Product";
            product.Description = updatedDescription;

            // Update the object
            responseAsync = userClient.UpdateProductAsync(product);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            productAsync = userClient.GetProductAsync(name);
            getProduct = await productAsync;
            Assert.Equal(updatedDescription, getProduct.Description);

            // Delete the object
            responseAsync = userClient.DeleteProductAsync(getProduct);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            productAsync = userClient.GetProductAsync(name);
            getProduct = await productAsync;
            Assert.Null(getProduct);
        }
    }
}
