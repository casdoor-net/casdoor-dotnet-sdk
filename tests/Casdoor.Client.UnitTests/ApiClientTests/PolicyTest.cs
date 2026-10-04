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

public class PolicyTest : IClassFixture<ServicesFixture>
{
    private readonly ICasdoorClient _client;

    public PolicyTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    [Fact]
    public async Task TestPolicy()
    {
        string org = TestConfig.OrganizationName;
        string name = TestUtils.GetRandomName("Policy");
        string now = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        var model = new CasdoorModel
        {
            Owner = org, Name = name, CreatedTime = now, DisplayName = name,
            ModelText = "[request_definition]\nr = sub, obj, act\n\n[policy_definition]\np = sub, obj, act\n\n" +
                        "[policy_effect]\ne = some(where (p.eft == allow))\n\n" +
                        "[matchers]\nm = r.sub == p.sub && r.obj == p.obj && r.act == p.act"
        };
        Assert.Equal("ok", (await _client.AddModelAsync(model))?.Status);

        var adapter = new CasdoorAdapter
        {
            Owner = org, Name = name, CreatedTime = now, Table = $"casbin_rule_{name.Split('_')[1]}", UseSameDb = true
        };
        Assert.Equal("ok", (await _client.AddAdapterAsync(adapter))?.Status);

        var enforcer = new CasdoorEnforcer
        {
            Owner = org, Name = name, CreatedTime = now, DisplayName = name, Model = $"{org}/{name}", Adapter = $"{org}/{name}"
        };
        Assert.Equal("ok", (await _client.AddEnforcerAsync(enforcer))?.Status);

        var policy = new CasdoorCasbinRule { Ptype = "p", V0 = "alice", V1 = "data1", V2 = "read" };
        Assert.Equal("ok", (await _client.AddPolicyAsync(enforcer, policy))?.Status);
        Assert.Contains(await _client.GetPoliciesAsync(name) ?? Enumerable.Empty<CasdoorCasbinRule>(), item => item.V0 == "alice");

        var filtered = await _client.GetFilteredPoliciesAsync($"{org}/{name}",
            new[] { new CasdoorPolicyFilter { Ptype = "p", FieldIndex = 0, FieldValues = new[] { "alice" } } });
        Assert.Single(filtered ?? Enumerable.Empty<CasdoorCasbinRule>());

        var newPolicy = new CasdoorCasbinRule { Ptype = "p", V0 = "alice", V1 = "data1", V2 = "write" };
        Assert.Equal("ok", (await _client.UpdatePolicyAsync(enforcer, policy, newPolicy))?.Status);
        Assert.Contains(await _client.GetPoliciesAsync(name) ?? Enumerable.Empty<CasdoorCasbinRule>(), item => item.V2 == "write");

        Assert.Equal("ok", (await _client.RemovePolicyAsync(enforcer, newPolicy))?.Status);
        Assert.DoesNotContain(await _client.GetPoliciesAsync(name) ?? Enumerable.Empty<CasdoorCasbinRule>(), item => item.V2 == "write");

        await _client.DeleteEnforcerAsync(enforcer);
        await _client.DeleteAdapterAsync(org, name);
        await _client.DeleteModelAsync(model);
    }
}
