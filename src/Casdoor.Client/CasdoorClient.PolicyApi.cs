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

namespace Casdoor.Client;

public partial class CasdoorClient
{
    /// <summary>
    ///     Gets the policies of the enforcer, or of the adapter when adapterId is given.
    /// </summary>
    public virtual async Task<IEnumerable<CasdoorCasbinRule>?> GetPoliciesAsync(string enforcerName, string? adapterId = null,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(enforcerName)).Add("adapterId", adapterId ?? string.Empty).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-policies", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorCasbinRule>?>();
    }

    /// <summary>
    ///     Gets the policies of the enforcer that match all the filters.
    /// </summary>
    public virtual async Task<IEnumerable<CasdoorCasbinRule>?> GetFilteredPoliciesAsync(string enforcerId,
        IEnumerable<CasdoorPolicyFilter> filters, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(enforcerId)).QueryMap;
        var result = await PostAsJsonAsync(_options.GetActionUrl("get-filtered-policies", queryMap), filters, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorCasbinRule>?>();
    }

    public virtual Task<CasdoorResponse?> AddPolicyAsync(CasdoorEnforcer enforcer, CasdoorCasbinRule policy,
        CancellationToken cancellationToken = default) =>
        ModifyPolicyAsync("add-policy", enforcer, policy, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdatePolicyAsync(CasdoorEnforcer enforcer, CasdoorCasbinRule oldPolicy,
        CasdoorCasbinRule newPolicy, CancellationToken cancellationToken = default) =>
        ModifyPolicyAsync("update-policy", enforcer, new[] { oldPolicy, newPolicy }, cancellationToken);

    public virtual Task<CasdoorResponse?> RemovePolicyAsync(CasdoorEnforcer enforcer, CasdoorCasbinRule policy,
        CancellationToken cancellationToken = default) =>
        ModifyPolicyAsync("remove-policy", enforcer, policy, cancellationToken);

    private Task<CasdoorResponse?> ModifyPolicyAsync<T>(string action, CasdoorEnforcer enforcer, T body,
        CancellationToken cancellationToken)
    {
        enforcer.Owner = GetOwner(enforcer.Owner);
        var queryMap = new QueryMapBuilder().Add("id", $"{enforcer.Owner}/{enforcer.Name}").QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl(action, queryMap), body, cancellationToken);
    }
}
