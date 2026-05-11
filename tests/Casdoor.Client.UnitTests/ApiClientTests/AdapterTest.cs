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
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();


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

        Task<CasdoorResponse?> responseAsync = userClient.AddAdapterAsync(adapter);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorAdapter>?> adaptersAsync = userClient.GetAdaptersAsync(ownerName);
        IEnumerable<CasdoorAdapter>? getAdapters = await adaptersAsync;
        Assert.True(getAdapters.Any());
        _testOutputHelper.WriteLine($"{getAdapters.Count()}");
        bool found = false;
        foreach (CasdoorAdapter casdoorAdapter in getAdapters)
        {
            _testOutputHelper.WriteLine(casdoorAdapter.Name);
            if (casdoorAdapter.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorAdapter?> adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        CasdoorAdapter? getAdapter = await adapterAsync;
        Assert.Equal(name, getAdapter.Name);
        //Update the object
        const string updatedUser = "Updated Casdoor Website";
        adapter.User = updatedUser;
        // Update the object
        responseAsync =
            userClient.UpdateAdapterAsync(adapter, null);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        getAdapter = await adapterAsync;
        Assert.Equal(updatedUser, getAdapter.User);

        // Delete the object
        responseAsync = userClient.DeleteAdapterAsync(ownerName, name);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        adapterAsync = userClient.GetAdapterAsync(ownerName, name);
        getAdapter = await adapterAsync;
        Assert.Null(getAdapter);
    }
}
