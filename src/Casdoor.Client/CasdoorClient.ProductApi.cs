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

    public virtual Task<CasdoorResponse?> AddProductAsync(CasdoorProduct product,
        CancellationToken cancellationToken = default)
    => ModifyProductAsync("add-product", product, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateProductAsync(CasdoorProduct product, CancellationToken cancellationToken = default)
    => ModifyProductAsync("update-product", product, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> DeleteProductAsync(CasdoorProduct product, CancellationToken cancellationToken = default)
    => ModifyProductAsync("delete-product", product, null, cancellationToken: cancellationToken);

    public virtual async Task<CasdoorProduct?> GetProductAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{_options.OrganizationName}/{name}").QueryMap;
        string url = _options.GetActionUrl("get-product", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorProduct?>();
    }

    public virtual async Task<IEnumerable<CasdoorProduct>?> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-products", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorProduct>?>();
    }

    public virtual async Task<IEnumerable<CasdoorProduct>?> GetPaginationProductsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        string url = _options.GetActionUrl("get-products", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorProduct>?>();
    }

    private Task<CasdoorResponse?> ModifyProductAsync(string action, CasdoorProduct product, IEnumerable<string>? columns = default, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{product.Owner}/{product.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        product.Owner = _options.OrganizationName;

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, product, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> BuyProductAsync(string name, string providerName, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{_options.OrganizationName}/{name}")
            .Add("providerName", providerName).QueryMap;
        string url = _options.GetActionUrl("buy-product", queryMap);
        return await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
    }
}

