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
        public async void TestPayment()
        {
            var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

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
            Task<CasdoorResponse?> responseAsync = userClient.AddPaymentAsync(payment);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            Task<IEnumerable<CasdoorPayment>?> paymentsAsync = userClient.GetPaymentsAsync();
            IEnumerable<CasdoorPayment>? getPayments = await paymentsAsync;
            Assert.True(getPayments.Any());
            _testOutputHelper.WriteLine($"{getPayments.Count()}");
            bool found = false;
            foreach (CasdoorPayment casdoorPayment in getPayments)
            {
                _testOutputHelper.WriteLine(casdoorPayment.Name);
                if (casdoorPayment.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            Task<CasdoorPayment?> paymentAsync = userClient.GetPaymentAsync(name);
            CasdoorPayment? getPayment = await paymentAsync;
            Assert.Equal(name, getPayment.Name);

            // Update the object
            const string updatedProductName = "Updated Casdoor Payment";
            payment.ProductName = updatedProductName;

            // Update the object
            responseAsync = userClient.UpdatePaymentAsync(payment);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            paymentAsync = userClient.GetPaymentAsync(name);
            getPayment = await paymentAsync;
            Assert.Equal(updatedProductName, getPayment.ProductName);

            // Delete the object
            responseAsync = userClient.DeletePaymentAsync(getPayment);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            paymentAsync = userClient.GetPaymentAsync(name);
            getPayment = await paymentAsync;
            Assert.Null(getPayment);
        }
    }
}
