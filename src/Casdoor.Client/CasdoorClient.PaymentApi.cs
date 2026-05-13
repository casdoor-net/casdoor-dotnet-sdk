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

    public virtual Task<CasdoorResponse?> AddPaymentAsync(CasdoorPayment payment,
        CancellationToken cancellationToken = default)
    => ModifyPaymentAsync("add-payment", payment, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdatePaymentAsync(CasdoorPayment payment, CancellationToken cancellationToken = default)
    => ModifyPaymentAsync("update-payment", payment, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> NotifyPaymentAsync(CasdoorPayment payment, CancellationToken cancellationToken = default)
    => ModifyPaymentAsync("notify-payment", payment, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> InvoicePaymentAsync(CasdoorPayment payment, CancellationToken cancellationToken = default)
    => ModifyPaymentAsync("invoice-payment", payment, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> DeletePaymentAsync(CasdoorPayment payment, CancellationToken cancellationToken = default)
    => ModifyPaymentAsync("delete-payment", payment, null, cancellationToken: cancellationToken);

    public virtual async Task<CasdoorPayment?> GetPaymentAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{_options.OrganizationName}/{name}").QueryMap;
        string url = _options.GetActionUrl("get-payment", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorPayment?>();
    }

    public virtual async Task<IEnumerable<CasdoorPayment>?> GetUserPaymentsAsync(string userName, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner",_options.OrganizationName)
            .Add("organization", _options.OrganizationName)
            .Add("user", userName).QueryMap;
        string url = _options.GetActionUrl("get-user-payment", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorPayment>?>();
    }

    public virtual async Task<IEnumerable<CasdoorPayment>?> GetPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-payments", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorPayment>?>();
    }

    public virtual async Task<IEnumerable<CasdoorPayment>?> GetPaginationPaymentsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        string url = _options.GetActionUrl("get-payments", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorPayment>?>();
    }

    private Task<CasdoorResponse?> ModifyPaymentAsync(string action, CasdoorPayment payment, IEnumerable<string>? columns = default, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{payment.Owner}/{payment.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        payment.Owner = _options.OrganizationName;

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, payment, cancellationToken);
    }
}
