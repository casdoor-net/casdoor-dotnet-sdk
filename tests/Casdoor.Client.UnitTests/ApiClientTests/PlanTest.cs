using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class PlanTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public PlanTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestPlan()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("plan");
        const string ownerName = "casbin";

        var plan = new CasdoorPlan()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = appName,
            Currency="CNY",
        };

        // Add a new object

        var responseAsync = userClient.AddPlanAsync(plan);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var plansAsync = userClient.GetPlansAsync();
        var getPlans = TestUtils.AssertNotNull(await plansAsync);
        Assert.True(getPlans.Any());
        _testOutputHelper.WriteLine($"{getPlans.Count()}");
        bool found = false;
        foreach (var casdoorPlan in getPlans)
        {
            _testOutputHelper.WriteLine(casdoorPlan.Name);
            if (casdoorPlan.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var planAsync = userClient.GetPlanAsync($"{appName}");
        var getPlan = TestUtils.AssertNotNull(await planAsync);
        Assert.Equal(appName, getPlan.Name);
        //Update the object
        const string displayName = "Update Casdoor Website";
        plan.DisplayName = displayName;
        // Update the object
        responseAsync =
            userClient.UpdatePlanAsync(plan, $"{ownerName}/{appName}");
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        planAsync = userClient.GetPlanAsync($"{appName}");
        getPlan = TestUtils.AssertNotNull(await planAsync);
        Assert.Equal(displayName, getPlan.DisplayName);

        // Delete the object
        responseAsync = userClient.DeletePlanAsync(plan);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        planAsync = userClient.GetPlanAsync($"{appName}");
        var deletedPlan = await planAsync;
        Assert.Null(deletedPlan);

    }
}
