using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class ModelTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public ModelTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestModel()
    {
        var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

        string appName = TestUtils.GetRandomName("model");
        const string ownerName = "casbin";
        const string modelText = @"[request_definition]
r = sub, obj, act

[policy_definition]
p = sub, obj, act

[role_definition]
g = _, _

[policy_effect]
e = some(where (p.eft == allow))

[matchers]
m = g(r.sub, p.sub) && r.obj == p.obj && r.act == p.act";
        var model = new CasdoorModel()
        {
            Owner = ownerName,
            Name = appName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Description = "Casdoor website",
            ModelText = modelText
        };
        // Add a new object

        var responseAsync = userClient.AddModelAsync(model);
        var response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        _testOutputHelper.WriteLine(response.Status);
        // Get all objects, check if our added object is inside the list
        var modelsAsync = userClient.GetModelsAsync();
        var getModels = TestUtils.AssertNotNull(await modelsAsync);
        Assert.True(getModels.Any());
        bool found = false;
        foreach (var casdoorModel in getModels)
        {
            _testOutputHelper.WriteLine(casdoorModel.Name);
            if (casdoorModel.Name == appName)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        var modelAsync = userClient.GetModelAsync($"{appName}");
        var getModel = TestUtils.AssertNotNull(await modelAsync);
        Assert.Equal(appName, getModel.Name);
        //Update the object
        const string updateDescription = "Update Casdoor Website";
        model.Description = updateDescription;
        // Update the object
        responseAsync =
            userClient.UpdateModelAsync(model, $"{ownerName}/{appName}");
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        modelAsync = userClient.GetModelAsync($"{appName}");
        getModel = TestUtils.AssertNotNull(await modelAsync);
        Assert.Equal(updateDescription, getModel.Description);

        // Delete the object
        responseAsync = userClient.DeleteModelAsync(model);
        response = TestUtils.AssertNotNull(await responseAsync);
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        modelAsync = userClient.GetModelAsync($"{appName}");
        var deletedModel = await modelAsync;
        Assert.Null(deletedModel);

    }
}
