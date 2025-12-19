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
    public async void TestApplications()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

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
        Task<CasdoorResponse?> responseAsync = userClient.AddApplicationAsync(application);
        CasdoorResponse? response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorApplication>?> applicationsAsync = userClient.GetApplicationsAsync(ownerName);
        IEnumerable<CasdoorApplication>? getApplications = await applicationsAsync;
        Assert.True(getApplications.Any());
        bool found = false;
        foreach (CasdoorApplication casdoorApplication in getApplications)
        {
            if (casdoorApplication.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorApplication?> applicationAsync = userClient.GetApplicationAsync($"{ownerName}/{appName}");
        CasdoorApplication? getApplication = await applicationAsync;
        Assert.Equal(appName, getApplication.Name);
        //Update the object
        const string updateDescription = "Update Casdoor Website";
        getApplication.Description = updateDescription;
        // Update the object
        responseAsync =
            userClient.UpdateApplicationAsync($"{ownerName}/{appName}", getApplication);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        applicationAsync = userClient.GetApplicationAsync($"{ownerName}/{appName}");
        getApplication = await applicationAsync;
        Assert.Equal(updateDescription, getApplication.Description);

        // Delete the object
        responseAsync = userClient.DeleteApplicationAsync(appName);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        applicationsAsync = userClient.GetOrganizationApplicationsAsync("casbin");
        getApplications = await applicationsAsync;
        Assert.True(getApplications.Any());

    }
}
