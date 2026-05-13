using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class UserTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public UserTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestUser()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("User");
        _testOutputHelper.WriteLine($"test with username {name}");
        string appName = $"admin/{name}";
        string owner = "casbin";

        var user = new CasdoorUser()
        {
            Owner = owner,
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            DisplayName = name,
        };

        // Add the object
        var responseAsync = userClient.AddUserAsync(user);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var userAsyncs = userClient.GetUsersAsync(owner);
        var getUsers = TestUtils.AssertNotNull(await userAsyncs);
        Assert.True(getUsers.Any());
        _testOutputHelper.WriteLine($"{getUsers.Count()}");
        bool found = false;
        foreach (var casdoorUser in getUsers)
        {
            _testOutputHelper.WriteLine(casdoorUser.Name);
            if (casdoorUser.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var userAsync = userClient.GetUserAsync(name, owner);
        var getUser = TestUtils.AssertNotNull(await userAsync);
        Assert.Equal(name, getUser.Name);

        // Update the object
        getUser.DisplayName = "Updated Casdoor Website";
        responseAsync = userClient.UpdateUserAsync(getUser);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        userAsync = userClient.GetUserAsync(name, owner);
        getUser = TestUtils.AssertNotNull(await userAsync);
        Assert.Equal("Updated Casdoor Website", getUser.DisplayName);

        // Delete the object
        responseAsync = userClient.DeleteUserAsync(user.Name);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        userAsync = userClient.GetUserAsync(name, owner);
        var deletedUser = await userAsync;
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task TestUserPagination()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();
        string owner = "casbin";

        // Test pagination
        var (users, totalCount) = await userClient.GetPaginatedUsersAsync(1, 10, null, owner);

        _testOutputHelper.WriteLine($"Total users count: {totalCount}");
        _testOutputHelper.WriteLine($"Retrieved users in page: {users?.Count() ?? 0}");

        // Verify we got results
        Assert.NotNull(users);
        Assert.True(totalCount >= 0);

        // If there are users, verify the page size is respected
        if (users.Any())
        {
            Assert.True(users.Count() <= 10);
            _testOutputHelper.WriteLine($"First user: {users.First().Name}");
        }

        // Test with custom query parameters
        var queryParams = new Dictionary<string, string?>
        {
            { "field", "name" },
            { "value", "" }
        };

        var (filteredUsers, filteredCount) = await userClient.GetPaginatedUsersAsync(1, 5, queryParams, owner);
        Assert.NotNull(filteredUsers);
        Assert.True(filteredCount >= 0);
        _testOutputHelper.WriteLine($"Filtered users count: {filteredCount}");
    }
}
