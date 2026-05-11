using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using Xunit.Abstractions;

namespace Casdoor.Client.UnitTests.ApiClientTests;

    public class ClientTest : IClassFixture<ServicesFixture>, IClassFixture<ServicesFixtureWithoutSecret>
{
        private readonly ServicesFixture _servicesFixture;
        private readonly ServicesFixtureWithoutSecret _servicesFixtureWithoutSecret;
        private readonly ITestOutputHelper _testOutputHelper;

        public ClientTest(ServicesFixture servicesFixture, ServicesFixtureWithoutSecret servicesFixtureWithoutSecret, ITestOutputHelper testOutputHelper)
        {
            _servicesFixture = servicesFixture;
            _servicesFixtureWithoutSecret = servicesFixtureWithoutSecret;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestClient()
        {
            var tokenClient = _servicesFixture.ServiceProvider.GetService<ICasdoorClient>();
        string name = TestUtils.GetRandomName("TokenClient");
        string code = TestUtils.GetRandomName("Code");
        _testOutputHelper.WriteLine($"test with token name {name}");
            string owner = "admin";
            var verifier = "test";

            var sha256Instance = SHA256.Create();
            byte[] bytes = Encoding.Default.GetBytes(verifier);
            byte[] chanllengeCodeEncoded = sha256Instance.ComputeHash(bytes);
            string chanllengeCodeBase64Encoded = Convert.ToBase64String(chanllengeCodeEncoded).Replace("+", "-").Replace("/", "_").Replace("=", "");

            CasdoorToken token = new CasdoorToken()
            {
                Owner = owner,
                Name = name,
                CreatedTime = DateTime.Now.ToString(),
                Code = code,
                AccessToken = code + "123456",
                CodeChallenge = chanllengeCodeBase64Encoded,
                Application = "app-example",
                Organization = "casbin",
                User = "admin",
                CodeExpireIn = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + 19600
            };

            // Add the object
            Task<CasdoorResponse?> responseAsync = tokenClient.AddTokenAsync(token);
            CasdoorResponse? response = await responseAsync;
            _testOutputHelper.WriteLine($"{response.Status} {response.Msg}");
            Assert.Equal(CasdoorConstants.DefaultCasdoorSuccessStatus, response.Status);

            var userClient = _servicesFixtureWithoutSecret.ServiceProvider.GetService<ICasdoorClient>();

            if (userClient == null)
            {
                Assert.NotNull(userClient);
                return;
            }

            var requestedToken = await userClient.RequestAuthorizationCodeTokenAsync(token.Code, "http://localhost:5000/callback", verifier);

            Assert.Equal(requestedToken.AccessToken, token.AccessToken);
        }

    }

