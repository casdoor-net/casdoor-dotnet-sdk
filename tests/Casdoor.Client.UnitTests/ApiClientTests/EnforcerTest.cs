using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class EnforcerTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public EnforcerTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestEnforcer()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("enforcer");
        const string ownerName = "casbin";

        var enforcer = new CasdoorEnforcer()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = appName,
            Model = "built-in/user-model-built-in",
            Adapter = "built-in/user-adapter-built-in",
            Description = "Casdoor website"
        };
        // Add a new object

        Task<CasdoorResponse?> responseAsync = userClient.AddEnforcerAsync(enforcer);
        CasdoorResponse? response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        _testOutputHelper.WriteLine(response.Status);
        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorEnforcer>?> enforcesAsync = userClient.GetEnforcersAsync();
        IEnumerable<CasdoorEnforcer>? getEnforcers = await enforcesAsync;
        Assert.True(getEnforcers.Any());
        bool found = false;
        foreach (CasdoorEnforcer casdoorEnforcer in getEnforcers)
        {
            _testOutputHelper.WriteLine(casdoorEnforcer.Name);
            if (casdoorEnforcer.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorEnforcer?> enforcerAsync = userClient.GetEnforcerAsync($"{appName}");
        CasdoorEnforcer? getEnforcer = await enforcerAsync;
        Assert.Equal(appName, getEnforcer.Name);
        //Update the object
        const string updateDescription = "Update Casdoor Website";
        enforcer.Description = updateDescription;
        // Update the object
        responseAsync =
            userClient.UpdateEnforcerAsync(enforcer, $"{ownerName}/{appName}");
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        enforcerAsync = userClient.GetEnforcerAsync($"{appName}");
        getEnforcer = await enforcerAsync;
        Assert.Equal(updateDescription, getEnforcer.Description);

        // Delete the object
        responseAsync = userClient.DeleteEnforcerAsync(enforcer);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        enforcerAsync = userClient.GetEnforcerAsync($"{appName}");
        getEnforcer = await enforcerAsync;
        Assert.Null(getEnforcer);

    }
}
