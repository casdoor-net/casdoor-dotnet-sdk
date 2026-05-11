using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class GroupTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public GroupTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestGroup()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("group");
        const string ownerName = "casbin";

        var group = new CasdoorGroup()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = appName,
        };

        // Add a new object

        Task<CasdoorResponse?> responseAsync = userClient.AddGroupAsync(group);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorGroup>?> groupsAsync = userClient.GetGroupsAsync();
        IEnumerable<CasdoorGroup>? getGroups = await groupsAsync;
        Assert.True(getGroups.Any());
        _testOutputHelper.WriteLine($"{getGroups.Count()}");
        bool found = false;
        foreach (CasdoorGroup casdoorGroup in getGroups)
        {
            _testOutputHelper.WriteLine(casdoorGroup.Name);
            if (casdoorGroup.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorGroup?> groupAsync = userClient.GetGroupAsync($"{appName}");
        CasdoorGroup? getGroup = await groupAsync;
        Assert.Equal(appName, getGroup.Name);
        //Update the object
        const string displayName = "Update Casdoor Website";
        group.DisplayName = displayName;
        // Update the object
        responseAsync =
            userClient.UpdateGroupAsync(group, $"{ownerName}/{appName}");
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        groupAsync = userClient.GetGroupAsync($"{appName}");
        getGroup = await groupAsync;
        Assert.Equal(displayName, getGroup.DisplayName);

        // Delete the object
        responseAsync = userClient.DeleteGroupAsync(group);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        groupAsync = userClient.GetGroupAsync($"{appName}");
        getGroup = await groupAsync;
        Assert.Null(getGroup);

    }
}
