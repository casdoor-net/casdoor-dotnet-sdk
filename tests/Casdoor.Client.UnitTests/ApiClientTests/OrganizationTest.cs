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

public class OrganizationTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public OrganizationTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestOrganization()
    {
        var organizationClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("Organization");
        _testOutputHelper.WriteLine($"test with organization name {name}");
        string owner = "admin";

        CasdoorOrganization organization = new CasdoorOrganization()
        {
            Owner = owner,
            Name = name,
            CreatedTime = DateTime.Now.ToString(),
            DisplayName = name,
            WebsiteUrl = "https://example.com",
            PasswordType = "plain",
            PasswordOptions = new string[]{ "AtLeast6"},
            CountryCodes = new string[]{ "US", "ES", "FR", "DE", "GB", "CN", "JP", "KR", "VN", "ID", "SG", "IN"},
            Tags = new string[] { },
            Languages = new string[] { "en", "zh", "es", "fr", "de", "id", "ja", "ko", "ru", "vi", "pt" },
            InitScore = 2000,
            EnableSoftDeletion = false,
            IsProfilePublic = false,
        };

        // Add the object
        Task<CasdoorResponse?> responseAsync = organizationClient.AddOrganizationAsync(organization);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get the object
        Task<CasdoorOrganization?> organizationAsync = organizationClient.GetOrganizationAsync($"admin/{name}");
        CasdoorOrganization? getOrganization = await organizationAsync;
        Assert.Equal(name, getOrganization.Name);

        // Update the object
        string updatedDisplayName = "Updated Casdoor Website";
        getOrganization.DisplayName = updatedDisplayName;
        responseAsync = organizationClient.UpdateOrganizationAsync($"admin/{name}",getOrganization);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the update
        organizationAsync = organizationClient.GetOrganizationAsync($"admin/{name}");
        getOrganization = await organizationAsync;
        Assert.Equal(updatedDisplayName, getOrganization.DisplayName);

        // Delete the object
        responseAsync = organizationClient.DeleteOrganizationAsync(name);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Validate the deletion
        organizationAsync = organizationClient.GetOrganizationAsync($"admin/{name}");
        getOrganization = await organizationAsync;
        Assert.Null(getOrganization);
    }
}
