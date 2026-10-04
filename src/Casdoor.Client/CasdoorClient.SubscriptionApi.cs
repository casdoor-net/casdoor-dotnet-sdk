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
    public virtual async Task<CasdoorResponse?> AddSubscriptionAsync(CasdoorSubscription casdoorSubscription,
            CancellationToken cancellationToken = default)
        => await PostAsJsonAsync(_options.GetActionUrl("add-subscription"), casdoorSubscription, cancellationToken);

    public virtual async Task<CasdoorResponse?> DeleteSubscriptionAsync(CasdoorSubscription casdoorSubscription,
            CancellationToken cancellationToken = default)
        => await PostAsJsonAsync(_options.GetActionUrl("delete-subscription"), casdoorSubscription, cancellationToken);

    public virtual async Task<CasdoorSubscription?> GetSubscriptionAsync(string owner, string name,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;

        string url = _options.GetActionUrl("get-subscription", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<CasdoorSubscription?>();
    }

    public virtual async Task<IEnumerable<CasdoorSubscription>?> GetSubscriptionsAsync(string owner,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("owner", owner).QueryMap;

        string url = _options.GetActionUrl("get-subscriptions", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorSubscription>?>();
    }

    public virtual async Task<IEnumerable<CasdoorSubscription>?> GetPaginationSubscriptions(string owner, int p, int pageSize,
        List<KeyValuePair<string, string?>> queryMap, CancellationToken cancellationToken = default)
    {

        queryMap.Add(new KeyValuePair<string, string?>("owner", owner));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));

        string url = _options.GetActionUrl("get-subscriptions", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorSubscription>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateSubscriptionAsync(CasdoorSubscription casdoorSubscription,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
             .Add("id", $"{casdoorSubscription.Owner}/{casdoorSubscription.Name}").QueryMap;
        string url = _options.GetActionUrl("update-subscription", queryMap);
        return PostAsJsonAsync(url, casdoorSubscription, cancellationToken);
    }

    public virtual async Task<(IEnumerable<CasdoorSubscription>? subscriptions, int totalCount)> GetPaginationSubscriptionsAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-subscriptions", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorSubscription>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
