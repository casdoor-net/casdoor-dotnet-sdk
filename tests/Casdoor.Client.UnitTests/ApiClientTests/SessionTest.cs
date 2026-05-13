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
    public class SessionTest : IClassFixture<ServicesFixture>
    {
        private readonly ServicesFixture _servicesFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public SessionTest(ServicesFixture servicesFixture, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestSession()
        {
            var userClient = _servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

            const string ownerName = "casbin";
            string name = TestUtils.GetRandomName("Session");

            var session = new CasdoorSession()
            {
                Owner = ownerName,
                Name = name,
                CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Application = "app-built-in",
                SessionId = new string[]{}
            };

            // Add a new object
            var responseAsync = userClient.AddSessionAsync(session);
            var response = TestUtils.AssertNotNull(await responseAsync);
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Get all objects, check if our added object is inside the list
            var sessionsAsync = userClient.GetSessionsAsync();
            var getSessions = TestUtils.AssertNotNull(await sessionsAsync);
            Assert.True(getSessions.Any());
            _testOutputHelper.WriteLine($"{getSessions.Count()}");
            bool found = false;
            foreach (var casdoorSession in getSessions)
            {
                _testOutputHelper.WriteLine(casdoorSession.Name);
                if (casdoorSession.Name == name)
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Get the object
            var sessionAsync = userClient.GetSessionAsync(name, session.Application);
            var getSession = TestUtils.AssertNotNull(await sessionAsync);
            Assert.Equal(name, getSession.Name);

            // Update the object
            string updatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            session.CreatedTime = updatedTime;

            // Update the object
            responseAsync = userClient.UpdateSessionAsync(session);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the update
            sessionAsync = userClient.GetSessionAsync(name, session.Application);
            getSession = TestUtils.AssertNotNull(await sessionAsync);
            Assert.Equal(updatedTime, getSession.CreatedTime);

            // Delete the object
            responseAsync = userClient.DeleteSessionAsync(getSession);
            response = TestUtils.AssertNotNull(await responseAsync);
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            // Validate the deletion
            sessionAsync = userClient.GetSessionAsync(name, session.Application);
            var deletedSession = await sessionAsync;
            Assert.Null(deletedSession);
        }
    }
}
