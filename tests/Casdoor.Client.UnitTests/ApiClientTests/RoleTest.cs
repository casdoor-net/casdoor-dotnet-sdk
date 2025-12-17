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
using static System.Net.Mime.MediaTypeNames;

namespace Casdoor.Client.UnitTests.ApiClientTests
{
    public class RoleTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public RoleTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async void TestRole()
        {
            var userClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();

            const string ownerName = "admin";
            string name = TestUtils.GetRandomName("Role");
            _testOutputHelper.WriteLine($"start with name: {name}");

            var role = new CasdoorRole()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DisplayName = name,
                Description = "Casdoor Website"
            };

            // Add a new object
            Task<CasdoorResponse?> responseAsync = userClient.AddRoleAsync(role);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            Task<IEnumerable<CasdoorRole>?> rolesAsync = userClient.GetRolesAsync();
            IEnumerable<CasdoorRole>? getRoles = await rolesAsync;
            Assert.True(getRoles.Any());
            _testOutputHelper.WriteLine($"{getRoles.Count()}");
            bool found = false;
            foreach (CasdoorRole casdoorRole in getRoles)
            {
                _testOutputHelper.WriteLine(casdoorRole.Name);
                if (casdoorRole.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            Task<CasdoorRole?> roleAsync = userClient.GetRoleAsync(name);
            CasdoorRole? getRole = await roleAsync;
            Assert.Equal(name, getRole.Name);

            // Update the object
            const string updatedDescription = "Updated Casdoor Website";
            role.Description = updatedDescription;

            // Update the object
            responseAsync = userClient.UpdateRoleAsync(role, role.Name);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            roleAsync = userClient.GetRoleAsync(name);
            getRole = await roleAsync;
            Assert.Equal(updatedDescription, getRole.Description);

            // Delete the object
            responseAsync = userClient.DeleteRoleAsync(getRole);
            response = await responseAsync;
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            roleAsync = userClient.GetRoleAsync(name);
            getRole = await roleAsync;
            Assert.Null(getRole);
        }
    }
}
