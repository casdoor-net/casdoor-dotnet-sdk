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
using System.Xml.Linq;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<CasdoorResponse?> AddWebhookAsync(CasdoorWebhook webhook,
            CancellationToken cancellationToken = default) =>
        await PostAsJsonAsync(_options.GetActionUrl("add-webhook"), webhook, cancellationToken);

    public virtual async Task<CasdoorResponse?> DeleteWebhookAsync(CasdoorWebhook webhook,
            CancellationToken cancellationToken = default) =>
        await PostAsJsonAsync(_options.GetActionUrl("delete-webhook"), webhook, cancellationToken);

    public virtual async Task<CasdoorWebhook?> GetWebhookAsync(string owner, string name,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;

        string url = _options.GetActionUrl("get-webhook", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<CasdoorWebhook?>();
    }

    public virtual async Task<IEnumerable<CasdoorWebhook>?> GetWebhooksAsync(string owner,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("owner",owner).QueryMap;

        string url = _options.GetActionUrl("get-webhooks", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorWebhook>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateWebhookAsync(CasdoorWebhook casdoorWebhook,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("id", $"{casdoorWebhook.Owner}/{casdoorWebhook.Name}").QueryMap;
        string url = _options.GetActionUrl("update-webhook", queryMap);
        return PostAsJsonAsync(url, casdoorWebhook, cancellationToken);
    }

    public virtual async Task<(IEnumerable<CasdoorWebhook>? webhooks, int totalCount)> GetPaginationWebhooksAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-webhooks", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorWebhook>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
