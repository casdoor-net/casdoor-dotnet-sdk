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
#if NETFRAMEWORK
using System.Net;
#else
using System.Web;
#endif

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<CasdoorResponse?> AddProviderAsync(
        CasdoorProvider provider, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(provider.Owner))
        {
            provider.Owner = CasdoorConstants.DefaultCasdoorOwner;
        }
        var url = _options.GetActionUrl("add-provider");
        return await PostAsJsonAsync(url, provider, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> DeleteProviderAsync(
        string name, string owner = CasdoorConstants.DefaultCasdoorOwner, CancellationToken cancellationToken = default)
    {
        var provider = new CasdoorProvider { Owner = owner, Name = name };
        var url = _options.GetActionUrl("delete-provider");
        return await PostAsJsonAsync(url, provider, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> UpdateProviderAsync(
        string name, CasdoorProvider newProvider, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(newProvider.Owner))
        {
            newProvider.Owner = CasdoorConstants.DefaultCasdoorOwner;
        }
        var id = $"{newProvider.Owner}/{HtmlEncode(name)}";
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;
        var url = _options.GetActionUrl("update-provider", queryMap);
        return await PostAsJsonAsync(url, newProvider, cancellationToken);
    }

    public virtual async Task<CasdoorProvider?> GetProviderAsync(
        string name, string owner = CasdoorConstants.DefaultCasdoorOwner, CancellationToken cancellationToken = default)
    {
        var id = $"{owner}/{HtmlEncode(name)}";
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;
        var url = _options.GetActionUrl("get-provider", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorProvider?>();
    }

    public virtual async Task<IEnumerable<CasdoorProvider>?> GetProvidersAsync(
        string owner = CasdoorConstants.DefaultCasdoorOwner, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner).QueryMap;
        var url = _options.GetActionUrl("get-providers", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorProvider>?>();
    }

    public virtual async Task<IEnumerable<CasdoorProvider>?> GetGlobalProvidersAsync(CancellationToken cancellationToken = default)
    {
        var url = _options.GetActionUrl("get-global-providers");
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorProvider>?>();
    }

    private static string HtmlEncode(string value)
    {
#if NETFRAMEWORK
        return WebUtility.HtmlEncode(value);
#else
        return HttpUtility.HtmlEncode(value);
#endif
    }

    public virtual async Task<(IEnumerable<CasdoorProvider>? providers, int totalCount)> GetPaginationProvidersAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-providers", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorProvider>?>(), result.DeserializeData2<int?>() ?? 0);
    }
}
