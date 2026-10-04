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
    public virtual async Task<CasdoorResponse?> AddPlanAsync(CasdoorPlan plan, CancellationToken cancellationToken = default) =>
        await PostAsJsonAsync(_options.GetActionUrl("add-plan"), plan, cancellationToken);

    public virtual async Task<CasdoorResponse?> UpdatePlanAsync(CasdoorPlan plan, string modelId,
        CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("update-plan", new QueryMapBuilder().Add("id", modelId).QueryMap);
        return await PostAsJsonAsync(url, plan, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> DeletePlanAsync(CasdoorPlan plan, CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("delete-plan");
        return await PostAsJsonAsync(url, plan, cancellationToken);
    }

    public virtual async Task<CasdoorPlan?> GetPlanAsync(string name, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name, owner)).QueryMap;
        string url = _options.GetActionUrl("get-plan", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorPlan?>();
    }

    public virtual async Task<IEnumerable<CasdoorPlan>?> GetPlansAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-plans", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorPlan>?>();
    }

    public virtual async Task<(IEnumerable<CasdoorPlan>? plans, int totalCount)> GetPaginationPlansAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-plans", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorPlan>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
