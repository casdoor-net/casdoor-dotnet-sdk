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
    public virtual async Task<IEnumerable<CasdoorTransaction>?> GetTransactionsAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-transactions", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorTransaction>?>();
    }

    public virtual async Task<(IEnumerable<CasdoorTransaction>? transactions, int totalCount)> GetPaginationTransactionsAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-transactions", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorTransaction>?>(), result.DeserializeData2<int?>() ?? 0);
    }

    public virtual async Task<CasdoorTransaction?> GetTransactionAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name)).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-transaction", queryMap), cancellationToken);
        return result.DeserializeData<CasdoorTransaction?>();
    }

    public virtual async Task<IEnumerable<CasdoorTransaction>?> GetUserTransactionsAsync(string userName,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", _options.OrganizationName).Add("user", userName).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-user-transactions", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorTransaction>?>();
    }

    /// <summary>
    ///     Adds the transaction, when dryRun is true it's only validated (e.g. the user's balance) and not saved.
    ///     The data of the response is the name of the transaction.
    /// </summary>
    public virtual Task<CasdoorResponse?> AddTransactionWithDryRunAsync(CasdoorTransaction transaction, bool dryRun,
        CancellationToken cancellationToken = default) =>
        ModifyTransactionAsync("add-transaction", transaction, null, cancellationToken,
            dryRun ? new List<KeyValuePair<string, string?>> { new("dryRun", "1") } : null);

    public virtual Task<CasdoorResponse?> AddTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default) =>
        ModifyTransactionAsync("add-transaction", transaction, null, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default) =>
        ModifyTransactionAsync("update-transaction", transaction, null, cancellationToken);

    public virtual Task<CasdoorResponse?> DeleteTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default) =>
        ModifyTransactionAsync("delete-transaction", transaction, null, cancellationToken);

    private Task<CasdoorResponse?> ModifyTransactionAsync(string action, CasdoorTransaction transaction, IEnumerable<string>? columns,
        CancellationToken cancellationToken, List<KeyValuePair<string, string?>>? queryMap = null)
    {
        transaction.Owner = GetOwner(transaction.Owner);
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("id", $"{transaction.Owner}/{transaction.Name}"));
        if (columns != null && columns.Any())
        {
            queryMap.Add(new KeyValuePair<string, string?>("columns", string.Join(",", columns)));
        }
        return PostAsJsonAsync(_options.GetActionUrl(action, queryMap), transaction, cancellationToken);
    }
}
