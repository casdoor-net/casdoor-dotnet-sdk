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
    public async void TestPlan()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("plan");
        const string ownerName = "casbin";

        var plan = new CasdoorPlan()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = appName,
        };

        // Add a new object

        Task<CasdoorResponse?> responseAsync = userClient.AddPlanAsync(plan);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorPlan>?> plansAsync = userClient.GetPlansAsync();
        IEnumerable<CasdoorPlan>? getPlans = await plansAsync;
        Assert.True(getPlans.Any());
        _testOutputHelper.WriteLine($"{getPlans.Count()}");
        bool found = false;
        foreach (CasdoorPlan casdoorPlan in getPlans)
        {
            _testOutputHelper.WriteLine(casdoorPlan.Name);
            if (casdoorPlan.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorPlan?> planAsync = userClient.GetPlanAsync($"{appName}");
        CasdoorPlan? getPlan = await planAsync;
        Assert.Equal(appName, getPlan.Name);
        //Update the object
        const string displayName = "Update Casdoor Website";
        plan.DisplayName = displayName;
        // Update the object
        responseAsync =
            userClient.UpdatePlanAsync(plan, $"{ownerName}/{appName}");
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        planAsync = userClient.GetPlanAsync($"{appName}");
        getPlan = await planAsync;
        Assert.Equal(displayName, getPlan.DisplayName);

        // Delete the object
        responseAsync = userClient.DeletePlanAsync(plan);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        planAsync = userClient.GetPlanAsync($"{appName}");
        getPlan = await planAsync;
        Assert.Null(getPlan);

    }
}
