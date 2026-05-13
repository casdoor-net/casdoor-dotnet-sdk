// Copyright 2024 The Casdoor Authors.All Rights Reserved.
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

using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Casdoor.Client;

public partial class CasdoorClient
{

    public virtual Task<CasdoorResponse?> AddAdapterAsync(CasdoorAdapter casdoorUserAdapter, CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("add-adapter");
        return PostAsJsonAsync(url, casdoorUserAdapter, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> DeleteAdapterAsync(string owner, string name, CancellationToken cancellationToken = default)
    {
        CasdoorAdapter adapter = new() { Owner = owner, Name = name };
        var url = _options.GetActionUrl("delete-adapter");
        return PostAsJsonAsync(url, adapter, cancellationToken);
    }

    public virtual async Task<CasdoorAdapter?> GetAdapterAsync(string owner, string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;
        string url = _options.GetActionUrl("get-adapter", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorAdapter?>();
    }

    public virtual async Task<IEnumerable<CasdoorAdapter>?> GetAdaptersAsync(string owner, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", owner).QueryMap;
        string url = _options.GetActionUrl("get-adapters", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorAdapter>?>();
    }

    public virtual async Task<IEnumerable<CasdoorAdapter>?> GetPaginationAdaptersAsync(string owner, int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", owner));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        string url = _options.GetActionUrl("get-adapters", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorAdapter>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateAdapterAsync(CasdoorAdapter adapter, IEnumerable<string>? propertyNames = default, CancellationToken cancellationToken = default)
        => ModifyAdapterAsync("update-adapter", adapter, propertyNames, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateAdapterColumnsAsync(CasdoorAdapter adapter, IEnumerable<string>? columns = default, CancellationToken cancellationToken = default)
        => ModifyAdapterAsync("update-adapter", adapter, columns, cancellationToken: cancellationToken);

    private Task<CasdoorResponse?> ModifyAdapterAsync(string action, CasdoorAdapter adapter, IEnumerable<string>? columns = default, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{adapter.Owner}/{adapter.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, adapter, cancellationToken);
    }
}
