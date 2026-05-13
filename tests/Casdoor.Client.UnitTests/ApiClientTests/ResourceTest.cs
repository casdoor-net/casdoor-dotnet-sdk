using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class ResourceTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public ResourceTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestResource()
    {
        var resourceClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();
        //string name = "Resource_" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
        
        string owner = "casbin";
        string filename = "ResourceUploadTest.txt";

        string path = $"Examples/{filename}";
        string name = $"/casdoor/{filename}";
        _testOutputHelper.WriteLine($"test with resource name {name}");
        Assert.True(File.Exists(path));

        var fs = File.OpenRead(path);

        var resource = new CasdoorUserResource()
        {
            Owner = owner,
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            Description = "Casdoor Website",
            User = "casbin",
            FileName = filename,
            FileSize = fs.Length,
            Tag = name
        };

        // Add the object
        var responseAsync = resourceClient.UploadResourceAsync(resource.User, resource.Tag, "", resource.FileName, fs);
        var response = TestUtils.AssertNotNull(await responseAsync);
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        await Task.Delay(1000);

        // Get all objects, check if our added object is inside the list
        var resourceAsyncs = resourceClient.GetResourcesAsync(owner, "casbin", "", "", "", "");
        var getResources = TestUtils.AssertNotNull(await resourceAsyncs);
        Assert.True(getResources.Any());
        _testOutputHelper.WriteLine($"{getResources.Count()}");
        bool found = false;
        foreach (var casdoorResource in getResources)
        {
            _testOutputHelper.WriteLine(casdoorResource.Name);
            if (casdoorResource.Tag == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var resourceAsync = resourceClient.GetResourceAsync(name);
        var getResource = TestUtils.AssertNotNull(await resourceAsync);
        Assert.Equal(name, getResource.Tag);

        // Delete the object
        responseAsync = resourceClient.DeleteResourceAsync(name);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        resourceAsync = resourceClient.GetResourceAsync(name);
        var deletedResource = await resourceAsync;
        Assert.Null(deletedResource);
    }
}
