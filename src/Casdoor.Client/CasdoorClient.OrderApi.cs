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
    public virtual async Task<IEnumerable<CasdoorOrder>?> GetOrdersAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-orders", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorOrder>?>();
    }

    public virtual async Task<(IEnumerable<CasdoorOrder>? orders, int totalCount)> GetPaginationOrdersAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-orders", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorOrder>?>(), result.DeserializeData2<int?>() ?? 0);
    }

    public virtual async Task<CasdoorOrder?> GetOrderAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name)).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-order", queryMap), cancellationToken);
        return result.DeserializeData<CasdoorOrder?>();
    }

    public virtual async Task<IEnumerable<CasdoorOrder>?> GetUserOrdersAsync(string userName, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", _options.OrganizationName).Add("user", userName).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-user-orders", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorOrder>?>();
    }

    /// <summary>
    ///     Places an order of the products for the user, userName defaults to the current user.
    /// </summary>
    public virtual async Task<CasdoorOrder?> PlaceOrderAsync(IEnumerable<CasdoorProductInfo> productInfos, string? userName = null,
        CancellationToken cancellationToken = default)
    {
        var builder = new QueryMapBuilder().Add("owner", _options.OrganizationName);
        if (!string.IsNullOrEmpty(userName))
        {
            builder.Add("userName", userName!);
        }
        var result = await PostAsJsonAsync(_options.GetActionUrl("place-order", builder.QueryMap),
            new Dictionary<string, object> { ["productInfos"] = productInfos }, cancellationToken);
        return result.DeserializeData<CasdoorOrder?>();
    }

    /// <summary>
    ///     Creates a payment of the order with the payment provider.
    /// </summary>
    public virtual async Task<CasdoorPayment?> PayOrderAsync(string orderName, string providerName, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(orderName)).Add("providerName", providerName).QueryMap;
        var result = await PostAsJsonAsync(_options.GetActionUrl("pay-order", queryMap), string.Empty, cancellationToken);
        return result.DeserializeData<CasdoorPayment?>();
    }

    public virtual Task<CasdoorResponse?> CancelOrderAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name)).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("cancel-order", queryMap), string.Empty, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> AddOrderAsync(CasdoorOrder order, CancellationToken cancellationToken = default) =>
        ModifyOrderAsync("add-order", order, null, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateOrderAsync(CasdoorOrder order, CancellationToken cancellationToken = default) =>
        ModifyOrderAsync("update-order", order, null, cancellationToken);

    public virtual Task<CasdoorResponse?> DeleteOrderAsync(CasdoorOrder order, CancellationToken cancellationToken = default) =>
        ModifyOrderAsync("delete-order", order, null, cancellationToken);

    private Task<CasdoorResponse?> ModifyOrderAsync(string action, CasdoorOrder order, IEnumerable<string>? columns,
        CancellationToken cancellationToken, List<KeyValuePair<string, string?>>? queryMap = null)
    {
        order.Owner = GetOwner(order.Owner);
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("id", $"{order.Owner}/{order.Name}"));
        if (columns != null && columns.Any())
        {
            queryMap.Add(new KeyValuePair<string, string?>("columns", string.Join(",", columns)));
        }
        return PostAsJsonAsync(_options.GetActionUrl(action, queryMap), order, cancellationToken);
    }
}
