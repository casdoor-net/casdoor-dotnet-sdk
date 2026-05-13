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
    public async Task TestSubscription()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Subscription");

        var subscription = new CasdoorSubscription()
        {
            Owner = "admin",
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            DisplayName = name,
            Description = "Casdoor Website",
        };

        // Add the object
        var responseAsync = userClient.AddSubscriptionAsync(subscription);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var subscriptionAsyncs = userClient.GetSubscriptionsAsync("admin");
        var getSubscriptions = TestUtils.AssertNotNull(await subscriptionAsyncs);
        Assert.True(getSubscriptions.Any());
        _testOutputHelper.WriteLine($"{getSubscriptions.Count()}");
        bool found = false;
        foreach (var casdoorSubscription in getSubscriptions)
        {
            _testOutputHelper.WriteLine(casdoorSubscription.Name);
            if (casdoorSubscription.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        var getSubscription = TestUtils.AssertNotNull(await subscriptionAsync);
        Assert.Equal(name, getSubscription.Name);

        // Update the object
        string updateDescription = "Updated Casdoor Website";
        subscription.Description = updateDescription;
        responseAsync = userClient.UpdateSubscriptionAsync(subscription);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        getSubscription = TestUtils.AssertNotNull(await subscriptionAsync);
        Assert.Equal(updateDescription, getSubscription.Description);

        // Delete the object
        responseAsync = userClient.DeleteSubscriptionAsync(subscription);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        subscriptionAsync = userClient.GetSubscriptionAsync("admin", name);
        var deletedSubscription = await subscriptionAsync;
        Assert.Null(deletedSubscription);
    }
}
