using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class AdapterTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public AdapterTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestAdapter()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();


        const string ownerName = "admin";
        string name = TestUtils.GetRandomName("Adapter");

        var adapter = new CasdoorAdapter()
        {
            Owner = ownerName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            User = name,
            Host = "https://casdoor.org"
        };

        // Add a new object

        var responseAsync = userClient.AddAdapterAsync(adapter);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var adaptersAsync = userClient.GetAdaptersAsync(ownerName);
        var getAdapters = TestUtils.AssertNotNull(await adaptersAsync);
        Assert.True(getAdapters.Any());
        _testOutputHelper.WriteLine($"{getAdapters.Count()}");
        bool found = false;
        foreach (var casdoorAdapter in getAdapters)
        {
            _testOutputHelper.WriteLine(casdoorAdapter.Name);
            if (casdoorAdapter.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        var getAdapter = TestUtils.AssertNotNull(await adapterAsync);
        Assert.Equal(name, getAdapter.Name);
        //Update the object
        const string updatedUser = "Updated Casdoor Website";
        adapter.User = updatedUser;
        // Update the object
        responseAsync =
            userClient.UpdateAdapterAsync(adapter);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        getAdapter = TestUtils.AssertNotNull(await adapterAsync);
        Assert.Equal(updatedUser, getAdapter.User);

        // Delete the object
        responseAsync = userClient.DeleteAdapterAsync(ownerName, name);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        var deletedAdapter = await adapterAsync;
        Assert.Null(deletedAdapter);
    }
}
