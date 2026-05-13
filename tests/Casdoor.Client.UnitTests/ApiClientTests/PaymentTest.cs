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

namespace Casdoor.Client.UnitTests.ApiClientTests
{
    public class PaymentTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public PaymentTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestPayment()
        {
            var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

            const string ownerName = "admin";
            string name = TestUtils.GetRandomName("Payment");

            var payment = new CasdoorPayment()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DisplayName = name,
                ProductName = "casbin",
            };

            // Add a new object
            var responseAsync = userClient.AddPaymentAsync(payment);
            var response = TestUtils.AssertNotNull(await responseAsync);
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            var paymentsAsync = userClient.GetPaymentsAsync();
            var getPayments = TestUtils.AssertNotNull(await paymentsAsync);
            Assert.True(getPayments.Any());
            _testOutputHelper.WriteLine($"{getPayments.Count()}");
            bool found = false;
            foreach (var casdoorPayment in getPayments)
            {
                _testOutputHelper.WriteLine(casdoorPayment.Name);
                if (casdoorPayment.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            var paymentAsync = userClient.GetPaymentAsync(name);
            var getPayment = TestUtils.AssertNotNull(await paymentAsync);
            Assert.Equal(name, getPayment.Name);

            // Update the object
            const string updatedProductName = "Updated Casdoor Payment";
            payment.ProductName = updatedProductName;

            // Update the object
            responseAsync = userClient.UpdatePaymentAsync(payment);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            paymentAsync = userClient.GetPaymentAsync(name);
            getPayment = TestUtils.AssertNotNull(await paymentAsync);
            Assert.Equal(updatedProductName, getPayment.ProductName);

            // Delete the object
            responseAsync = userClient.DeletePaymentAsync(getPayment);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            paymentAsync = userClient.GetPaymentAsync(name);
            var deletedPayment = await paymentAsync;
            Assert.Null(deletedPayment);
        }
    }
}
