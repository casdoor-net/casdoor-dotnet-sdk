// Copyright 2022 The Casdoor Authors. All Rights Reserved.
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

using System.Net.Http.Json;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<CasdoorResponse?> AddEnforcerAsync(CasdoorEnforcer enforcer, CancellationToken cancellationToken = default) =>
        await PostAsJsonAsync(_options.GetActionUrl("add-enforcer"), enforcer, cancellationToken);

    public virtual async Task<CasdoorResponse?> UpdateEnforcerAsync(CasdoorEnforcer enforcer, string enforcerId,
        CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("update-enforcer", new QueryMapBuilder().Add("id", enforcerId).QueryMap);
        return await PostAsJsonAsync(url, enforcer, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> DeleteEnforcerAsync(CasdoorEnforcer enforcer, CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("delete-enforcer");
        return await PostAsJsonAsync(url, enforcer, cancellationToken);
    }

    public virtual async Task<CasdoorEnforcer?> GetEnforcerAsync(string name, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name, owner)).QueryMap;
        string url = _options.GetActionUrl("get-enforcer", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorEnforcer?>();
    }

    public virtual async Task<IEnumerable<CasdoorEnforcer>?> GetEnforcersAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-enforcers", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorEnforcer>?>();
    }

    public virtual async Task<(IEnumerable<CasdoorEnforcer>? enforcers, int totalCount)> GetPaginationEnforcersAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-enforcers", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorEnforcer>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
