using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class SubscriptionTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public SubscriptionTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void TestSubscription()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Subscription");

        CasdoorSubscription subscription = new CasdoorSubscription()
        {
            Owner = "admin",
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            DisplayName = name,
            Description = "Casdoor Website",
        };

        // Add the object
        Task<CasdoorResponse?> responseAsync = userClient.AddSubscriptionAsync(subscription);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorSubscription>?> subscriptionAsyncs = userClient.GetSubscriptionsAsync("admin");
        IEnumerable<CasdoorSubscription>? getSubscriptions = await subscriptionAsyncs;
        Assert.True(getSubscriptions.Any());
        _testOutputHelper.WriteLine($"{getSubscriptions.Count()}");
        bool found = false;
        foreach (CasdoorSubscription casdoorSubscription in getSubscriptions)
        {
            _testOutputHelper.WriteLine(casdoorSubscription.Name);
            if (casdoorSubscription.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorSubscription?> subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        CasdoorSubscription? getSubscription = await subscriptionAsync;
        Assert.Equal(name, getSubscription.Name);

        // Update the object
        string updateDescription = "Updated Casdoor Website";
        subscription.Description = updateDescription;
        responseAsync = userClient.UpdateSubscriptionAsync(subscription);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        getSubscription = await subscriptionAsync;
        Assert.Equal(updateDescription, getSubscription.Description);

        // Delete the object
        responseAsync = userClient.DeleteSubscriptionAsync(subscription);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        getSubscription = await subscriptionAsync;
        Assert.Null(getSubscription);
    }
}
