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

public class WebhookTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public WebhookTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestWebhook()
    {

        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Webhook");
        string appName = $"admin/{name}";

        CasdoorWebhook webhook = new CasdoorWebhook()
        {
            Owner= "casbin",
		    Name = name,
		    CreatedTime = DateTime.Now.ToString(),
		    Organization = "casbin",
        };

        // Add the object
        Task<CasdoorResponse?> responseAsync = userClient.AddWebhookAsync(webhook); 
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorWebhook>?> webhookAsyncs = userClient.GetWebhooksAsync("casbin"); 
        IEnumerable<CasdoorWebhook>? getWebhooks = await webhookAsyncs;
        Assert.True(getWebhooks.Any());
        _testOutputHelper.WriteLine($"{getWebhooks.Count()}");
        bool found = false;
        foreach (CasdoorWebhook casdoorWebhook in getWebhooks)
        {
            _testOutputHelper.WriteLine(casdoorWebhook.Name);
            if (casdoorWebhook.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorWebhook?> webhookAsync = userClient.GetWebhookAsync("casbin", name);  
        CasdoorWebhook? getWebhook = await webhookAsync;
        Assert.Equal(name, getWebhook.Name);

        // Update the object
        webhook.Organization = "admin";
        responseAsync = userClient.UpdateWebhookAsync(webhook); 
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        //Validate the update
        webhookAsync = userClient.GetWebhookAsync("casbin", name); 
        getWebhook = await webhookAsync;
        Assert.Equal("admin", getWebhook.Organization);

        // Delete the object
        responseAsync = userClient.DeleteWebhookAsync(webhook);  // 修改为 DeleteWebhookAsync
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        webhookAsync = userClient.GetWebhookAsync("casbin", name);  // 修改为 GetWebhookAsync
        getWebhook = await webhookAsync;
        Assert.Null(getWebhook);

    }
}
