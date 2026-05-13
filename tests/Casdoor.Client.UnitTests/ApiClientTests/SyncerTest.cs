using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class SyncerTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public SyncerTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestSyncer()
    {

        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Syncer");
        string appName = $"admin/{name}";

        var syncer = new CasdoorSyncer()
        {
            Owner = "admin",
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            Organization = "casbin",
            Host = "localhost",
            Port = 3306,
            User = "root",
            Password = "123",
            DatabaseType = "mysql",
            Database = "syncer_db",
            Table = "user-table",
            SyncInterval = 1,
        };

        // Add the object
        var responseAsync = userClient.AddSyncerAsync(syncer);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        var syncerAsyncs = userClient.GetSyncersAsync("admin");
        var getSyncers = TestUtils.AssertNotNull(await syncerAsyncs);
        Assert.True(getSyncers.Any());
        _testOutputHelper.WriteLine($"{getSyncers.Count()}");
        bool found = false;
        foreach (var casdoorSyncer in getSyncers)
        {
            _testOutputHelper.WriteLine(casdoorSyncer.Name);
            if (casdoorSyncer.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var syncerAsync = userClient.GetSyncerAsync("admin", name);
        var getSyncer = TestUtils.AssertNotNull(await syncerAsync);
        Assert.Equal(name, getSyncer.Name);

        // Update the object
        getSyncer.SyncInterval = 200;
        responseAsync = userClient.UpdateSyncerAsync(getSyncer);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        //Validate the update
        syncerAsync = userClient.GetSyncerAsync("admin", name);
        getSyncer = TestUtils.AssertNotNull(await syncerAsync);
        Assert.Equal(200, getSyncer.SyncInterval);

        // Delete the object
        responseAsync = userClient.DeleteSyncerAsync(syncer);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        syncerAsync = userClient.GetSyncerAsync("admin", name);
        var deletedSyncer = await syncerAsync;
        Assert.Null(deletedSyncer);

    }
}
