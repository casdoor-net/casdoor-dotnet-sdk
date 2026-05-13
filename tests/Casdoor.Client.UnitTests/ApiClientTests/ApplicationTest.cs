using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class ApplicationTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public ApplicationTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestApplications()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("Application");
        const string ownerName = CasdoorConstants.DefaultCasdoorOwner;
        var application = new CasdoorApplication()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Description = "Casdoor website",
            Organization = "casbin"
        };
        // Add a new object
        var responseAsync = userClient.AddApplicationAsync(application);
        var response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var applicationsAsync = userClient.GetApplicationsAsync(ownerName);
        var getApplications = TestUtils.AssertNotNull(await applicationsAsync);
        Assert.True(getApplications.Any());
        bool found = false;
        foreach (var casdoorApplication in getApplications)
        {
            if (casdoorApplication.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var applicationAsync = userClient.GetApplicationAsync($"{ownerName}/{appName}");
        var getApplication = TestUtils.AssertNotNull(await applicationAsync);
        Assert.Equal(appName, getApplication.Name);
        //Update the object
        const string updateDescription = "Update Casdoor Website";
        getApplication.Description = updateDescription;
        // Update the object
        responseAsync =
            userClient.UpdateApplicationAsync($"{ownerName}/{appName}", getApplication);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        applicationAsync = userClient.GetApplicationAsync($"{ownerName}/{appName}");
        getApplication = TestUtils.AssertNotNull(await applicationAsync);
        Assert.Equal(updateDescription, getApplication.Description);

        // Delete the object
        responseAsync = userClient.DeleteApplicationAsync(appName);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        applicationsAsync = userClient.GetOrganizationApplicationsAsync("casbin");
        getApplications = TestUtils.AssertNotNull(await applicationsAsync);
        Assert.True(getApplications.Any());

    }
}
