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

    public virtual Task<CasdoorResponse?> AddRecordAsync(CasdoorRecord record,
        CancellationToken cancellationToken = default)
    => ModifyRecordAsync("add-record", record, null, cancellationToken: cancellationToken);

    [Obsolete("Casdoor has no update-record API, so this call always fails.")]
    public virtual Task<CasdoorResponse?> UpdateRecordAsync(CasdoorRecord record, CancellationToken cancellationToken = default)
    => ModifyRecordAsync("update-record", record, null, cancellationToken: cancellationToken);

    [Obsolete("Casdoor has no delete-record API, so this call always fails.")]
    public virtual Task<CasdoorResponse?> DeleteRecordAsync(CasdoorRecord record, CancellationToken cancellationToken = default)
    => ModifyRecordAsync("delete-record", record, null, cancellationToken: cancellationToken);

    public virtual async Task<CasdoorRecord?> GetRecordAsync(string name, CancellationToken cancellationToken = default)
    {
        string recordName = name.Substring(name.LastIndexOf('/') + 1);

        // The name filter matches the records whose names contain the given name
        var queryMap = new List<KeyValuePair<string, string?>> { new("field", "name"), new("value", recordName) };
        var records = await GetPaginationRecordsAsync(100, 1, queryMap, cancellationToken);
        return records?.FirstOrDefault(record => record.Name == recordName);
    }

    public virtual async Task<IEnumerable<CasdoorRecord>?> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-records", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorRecord>?>();
    }

    public virtual async Task<IEnumerable<CasdoorRecord>?> GetPaginationRecordsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        string url = _options.GetActionUrl("get-records", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorRecord>?>();
    }

    private Task<CasdoorResponse?> ModifyRecordAsync(string action, CasdoorRecord record, IEnumerable<string>? columns, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{record.Owner}/{record.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        record.Owner = GetOwner(record.Owner);

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, record, cancellationToken);
    }
}
