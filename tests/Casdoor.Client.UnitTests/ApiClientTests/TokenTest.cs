using System.Globalization;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class TokenTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public TokenTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void TestToken()
    {
        var tokenClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Token");
        _testOutputHelper.WriteLine($"test with token name {name}");
        string owner = "admin";

        CasdoorToken token = new CasdoorToken()
        {
            Owner = owner,
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            Organization = "casbin",
            Code = "abc",
            AccessToken = "123456"
        };

        // Add the object
        Task<CasdoorResponse?> responseAsync = tokenClient.AddTokenAsync(token);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        await Task.Delay(1000);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorToken>?> tokenAsyncs = tokenClient.GetTokensAsync(owner);
        IEnumerable<CasdoorToken>? getTokens = await tokenAsyncs;
        Assert.True(getTokens.Any());
        _testOutputHelper.WriteLine($"{getTokens.Count()}");
        bool found = false;
        foreach (CasdoorToken casdoorToken in getTokens)
        {
            _testOutputHelper.WriteLine(casdoorToken.Name);
            if (casdoorToken.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);
        
        // Get the object
        Task<CasdoorToken?> tokenAsync = tokenClient.GetTokenAsync(owner, name);
        CasdoorToken? getToken = await tokenAsync;
        Assert.Equal(name, getToken.Name);

        // Update the object
        string updatedCode = "Updated Code";
        getToken.Code = updatedCode;
        responseAsync = tokenClient.UpdateTokenAsync(getToken, new List<string>());
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        tokenAsync = tokenClient.GetTokenAsync(owner, name);
        getToken = await tokenAsync;
        Assert.Equal(updatedCode, getToken.Code);

        // Delete the object
        responseAsync = tokenClient.DeleteTokenAsync(token);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        tokenAsync = tokenClient.GetTokenAsync(owner, name);
        getToken = await tokenAsync;
        Assert.Null(getToken);
    }
}
