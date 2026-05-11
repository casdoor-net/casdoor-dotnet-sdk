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

public class CertTest : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _servicesFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public CertTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
    {
        _servicesFixture = servicesFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task TestCert()
    {
        var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();


        const string ownerName = "admin";
        string name = TestUtils.GetRandomName("Cert");

        var cert = new CasdoorCert()
        {
            Owner = ownerName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = name,
            Scope = "JWT",
            Type = "x509",
            CryptoAlgorithm = "RS256",
            BitSize = 4096,
            ExpireInYears = 20,
        };

        // Add a new object

        Task<CasdoorResponse?> responseAsync = userClient.AddCertAsync(cert);
        CasdoorResponse? response = await responseAsync;
        _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

        // Get all objects, check if our added object is inside the list
        Task<IEnumerable<CasdoorCert>?> certsAsync = userClient.GetCertsAsync();
        IEnumerable<CasdoorCert>? getCerts = await certsAsync;
        Assert.True(getCerts.Any());
        _testOutputHelper.WriteLine($"{getCerts.Count()}");
        bool found = false;
        foreach (CasdoorCert casdoorCert in getCerts)
        {
            _testOutputHelper.WriteLine(casdoorCert.Name);
            if (casdoorCert.Name == name)
            {
                found = true;
            }
        }

        Assert.True(found);

        // Get the object
        Task<CasdoorCert?> certAsync = userClient.GetCertAsync(name);
        CasdoorCert? getCert = await certAsync;
        Assert.Equal(name, getCert.Name);
        //Update the object
        const string updatedDisplayName = "Updated Casdoor Website";
        cert.DisplayName = updatedDisplayName;
        // Update the object
        responseAsync =
            userClient.UpdateCertAsync(cert);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the update
        certAsync = userClient.GetCertAsync(name);
        getCert = await certAsync;
        Assert.Equal(updatedDisplayName, getCert.DisplayName);

        // Delete the object
        responseAsync = userClient.DeleteCertAsync(name);
        response = await responseAsync;
        Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);
        // Validate the deletion
        certAsync = userClient.GetCertAsync(name);
        getCert = await certAsync;
        Assert.Null(getCert);
    }
}
