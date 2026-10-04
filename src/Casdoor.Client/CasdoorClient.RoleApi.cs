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
    public virtual async Task<IEnumerable<CasdoorRole>?> GetRolesAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-roles", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorRole>?>();
    }

    public virtual async Task<CasdoorRole?> GetRoleAsync(string name, string? owner = null,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name, owner)).QueryMap;
        string url = _options.GetActionUrl("get-role", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorRole?>();
    }

    public virtual async Task<CasdoorResponse?> AddRoleAsync(CasdoorRole role, CancellationToken cancellationToken = default)
    {
        role.Owner = GetOwner(role.Owner);

        string url = _options.GetActionUrl("add-role");
        return await PostAsJsonAsync(url, role, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> UpdateRoleAsync(CasdoorRole role, string name, string? owner = null,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name, owner)).QueryMap;
        string url = _options.GetActionUrl("update-role", queryMap);
        return await PostAsJsonAsync(url, role, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> DeleteRoleAsync(CasdoorRole role, CancellationToken cancellationToken = default) =>
        await PostAsJsonAsync(_options.GetActionUrl("delete-role"), role, cancellationToken);

    public virtual async Task<(IEnumerable<CasdoorRole>? roles, int totalCount)> GetPaginationRolesAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-roles", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorRole>?>(), result.DeserializeData2<int?>() ?? 0);
    }

    public virtual Task<CasdoorResponse?> UpdateRoleForColumnsAsync(CasdoorRole role, IEnumerable<string> columns,
        CancellationToken cancellationToken = default)
    {
        role.Owner = GetOwner(role.Owner);
        var queryMap = new QueryMapBuilder().Add("id", $"{role.Owner}/{role.Name}").Add("columns", string.Join(",", columns)).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("update-role", queryMap), role, cancellationToken);
    }
}
