using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class PermissionTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public PermissionTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestPermission()
    {
        var permissionClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Permission");
        _testOutputHelper.WriteLine($"test with Permission name {name}");
        string owner = "casbin";

        CasdoorPermission permission = new CasdoorPermission()
        {
            Owner = owner,
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            DisplayName = name,
            Description = "Casdoor Website",
            Users = new string[] { "casbin/*" },
            Groups = new string[] { },
            Roles = new string[] { },
            Domains = new string[] { },
            Model = "admin/user-model-built-in",
            ResourceType = "Application",
            Resources = new string[] { "app-casbin" },
            Actions = new string[] { "Read", "Write" },
            Effect = "Allow",
            IsEnabled = true,
        };

        // Add the object
        Task<CasdoorResponse?> responseAsync = permissionClient.AddPermissionAsync(permission);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        await Task.Delay(1000);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorPermission>?> permissionAsyncs = permissionClient.GetPermissionsAsync(owner);
        IEnumerable<CasdoorPermission>? getPermissions = await permissionAsyncs;
        Assert.True(getPermissions.Any());
        _testOutputHelper.WriteLine($"{getPermissions.Count()}");
        bool found = false;
        foreach (CasdoorPermission casdoorPermission in getPermissions)
        {
            _testOutputHelper.WriteLine(casdoorPermission.Name);
            if (casdoorPermission.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorPermission?> PermissionAsync = permissionClient.GetPermissionAsync($"{owner}/{ name}");
        CasdoorPermission ? getPermission = await PermissionAsync;
        Assert.Equal(name, getPermission.Name);

        // Update the object
        string updatedDescription = "Updated Code";
        getPermission.Description = updatedDescription;
        responseAsync = permissionClient.UpdatePermissionAsync(getPermission, "");
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        PermissionAsync = permissionClient.GetPermissionAsync($"{owner}/{name}");
        getPermission = await PermissionAsync;
        Assert.Equal(updatedDescription, getPermission.Description);

        // Delete the object
        responseAsync = permissionClient.DeletePermissionAsync(permission);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        PermissionAsync = permissionClient.GetPermissionAsync($"{owner}/{name}");
        getPermission = await PermissionAsync;
        Assert.Null(getPermission);
    }
}
