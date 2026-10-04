// Copyright 2024 The Casdoor Authors. All Rights Reserved.
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
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<CasdoorResponse?> AddSyncerAsync(CasdoorSyncer casdoorSyncer,
            CancellationToken cancellationToken = default)
        => await PostAsJsonAsync(_options.GetActionUrl("add-syncer"), casdoorSyncer, cancellationToken);

    public virtual async Task<CasdoorResponse?> DeleteSyncerAsync(CasdoorSyncer casdoorSyncer,
            CancellationToken cancellationToken = default)
        => await PostAsJsonAsync(_options.GetActionUrl("delete-syncer"), casdoorSyncer, cancellationToken);

    public virtual async Task<CasdoorSyncer?> GetSyncerAsync(string owner, string name,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;

        string url = _options.GetActionUrl("get-syncer", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<CasdoorSyncer?>();
    }

    public virtual async Task<IEnumerable<CasdoorSyncer>?> GetSyncersAsync(string owner,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("owner", owner).QueryMap;

        string url = _options.GetActionUrl("get-syncers", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorSyncer>?>();
    }

    public virtual async Task<CasdoorResponse?> RunSyncerAsync(CasdoorSyncer casdoorSyncer,
            CancellationToken cancellationToken = default)
        => await PostAsJsonAsync(_options.GetActionUrl("run-syncer"), casdoorSyncer, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateSyncerAsync(CasdoorSyncer casdoorSyncer,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("id", $"{casdoorSyncer.Owner}/{casdoorSyncer.Name}").QueryMap;
        string url = _options.GetActionUrl("update-syncer", queryMap);
        return PostAsJsonAsync(url, casdoorSyncer, cancellationToken);
    }

    public virtual async Task<(IEnumerable<CasdoorSyncer>? syncers, int totalCount)> GetPaginationSyncersAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-syncers", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorSyncer>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
