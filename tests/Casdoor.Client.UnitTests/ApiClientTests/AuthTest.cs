// Copyright 2026 The Casdoor Authors. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class AuthTest : IClassFixture<ServicesFixture>
{
    // the CI user "admin" of the CI application's organization (built-in) has the password "123"
    private const string Username = "admin";
    private const string Password = "123";

    private readonly ICasdoorClient _client;

    public AuthTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    [Fact]
    public async Task TestOAuthTokenByPassword()
    {
        var token = await _client.GetOAuthTokenByPasswordAsync(Username, Password);
        Assert.False(token.IsError, token.Error);
        Assert.False(string.IsNullOrEmpty(token.AccessToken));

        var introspection = await _client.IntrospectTokenAsync(token.AccessToken!);
        Assert.True(introspection.IsActive);

        var refreshed = await _client.RefreshOAuthTokenAsync(token.RefreshToken!);
        Assert.False(string.IsNullOrEmpty(refreshed.AccessToken));
    }

    [Fact]
    public async Task TestWithAccessToken()
    {
        var token = await _client.GetOAuthTokenByPasswordAsync(Username, Password);

        var account = await _client.WithAccessToken(token.AccessToken!).GetAccountAsync();
        Assert.Equal(Username, account?.cassdoorUser?.Name);

        // the original client still calls the APIs as the application
        await Assert.ThrowsAnyAsync<Exception>(() => _client.GetAccountAsync());

        Assert.Equal("ok", (await _client.LogoutCurrentSessionAsync(token.AccessToken!))?.Status);
        var another = await _client.GetOAuthTokenByPasswordAsync(Username, Password);
        Assert.Equal("ok", (await _client.LogoutAsync(another.AccessToken!))?.Status);
    }

    [Fact]
    public async Task TestUserExtra()
    {
        string name = TestUtils.GetRandomName("User");
        var user = new CasdoorUser
        {
            Owner = TestConfig.OrganizationName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = name,
            Password = "123456"
        };
        Assert.Equal("ok", (await _client.AddUserAsync(user))?.Status);

        var retrieved = await _client.GetUserAsync(name);
        Assert.Equal(name, (await _client.GetUserByUserIdAsync(retrieved!.Id!))?.Name);
        Assert.Contains(await _client.GetGlobalUsersAsync() ?? Enumerable.Empty<CasdoorUser>(), item => item.Name == name);
        Assert.True((await _client.GetPaginationUsersAsync(1, 100)).totalCount > 0);

        retrieved.DisplayName = "Updated by columns";
        Assert.Equal("ok", (await _client.UpdateUserForColumnsAsync(retrieved, new[] { "displayName" }))?.Status);
        retrieved.DisplayName = "Updated by user id";
        Assert.Equal("ok", (await _client.UpdateUserByUserIdAsync(retrieved.Owner!, retrieved.Id!, retrieved))?.Status);
        Assert.Equal("Updated by user id", (await _client.GetUserAsync(name))?.DisplayName);

        await _client.DeleteUserAsync(name);
    }

    [Fact]
    public async Task TestPagination()
    {
        Assert.True((await _client.GetPaginationRolesAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationGroupsAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationModelsAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationEnforcersAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationPlansAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationProvidersAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationSyncersAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationWebhooksAsync(1, 10)).totalCount >= 0);
        Assert.True((await _client.GetPaginationSubscriptionsAsync(1, 10)).totalCount >= 0);
    }

    [Fact]
    public void TestGetId()
    {
        Assert.Equal($"{TestConfig.OrganizationName}/role", _client.GetId("role"));
        Assert.Equal("other/role", _client.GetId("other/role"));
    }
}
