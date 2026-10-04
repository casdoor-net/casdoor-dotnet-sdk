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

public class InvitationTest : IClassFixture<ServicesFixture>
{
    private readonly ICasdoorClient _client;

    public InvitationTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    [Fact]
    public async Task TestInvitation()
    {
        string name = TestUtils.GetRandomName("Invitation");
        string code = $"TEST{TestUtils.GetRandomName("Code").Split('_')[1]}";
        var invitation = new CasdoorInvitation
        {
            Owner = TestConfig.OrganizationName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = "Test Invitation",
            Code = code,
            DefaultCode = code,
            Quota = 10,
            Application = "app-casbin",
            Email = "test@example.com",
            State = "Active"
        };
        Assert.Equal("Affected", (await _client.AddInvitationAsync(invitation))?.Data?.ToString());

        Assert.Contains(await _client.GetInvitationsAsync() ?? Enumerable.Empty<CasdoorInvitation>(), item => item.Name == name);
        var (invitations, total) = await _client.GetPaginationInvitationsAsync(1, 100);
        Assert.True(total > 0);
        Assert.NotNull(invitations);

        var retrieved = await _client.GetInvitationAsync(name);
        Assert.Equal(code, retrieved?.Code);

        invitation.DisplayName = "Updated Invitation";
        Assert.Equal("Affected", (await _client.UpdateInvitationForColumnsAsync(invitation, new[] { "display_name" }))?.Data?.ToString());
        Assert.Equal("Updated Invitation", (await _client.GetInvitationAsync(name))?.DisplayName);

        var info = await _client.GetInvitationInfoAsync(code, "app-casbin");
        Assert.Equal(name, info?.Name);

        Assert.Equal("Affected", (await _client.DeleteInvitationAsync(invitation))?.Data?.ToString());
        Assert.Null(await _client.GetInvitationAsync(name));
    }
}
