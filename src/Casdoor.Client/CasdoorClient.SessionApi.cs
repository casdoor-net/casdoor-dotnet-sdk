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

    public virtual Task<CasdoorResponse?> AddSessionAsync(CasdoorSession session,
        CancellationToken cancellationToken = default)
    => ModifySessionAsync("add-session", session, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateSessionAsync(CasdoorSession session, CancellationToken cancellationToken = default)
    => ModifySessionAsync("update-session", session, null, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateSessionForColumnsAsync(CasdoorSession session, IEnumerable<string>? columns=default, CancellationToken cancellationToken = default)
    => ModifySessionAsync("update-session", session, columns, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> DeleteSessionAsync(CasdoorSession session, CancellationToken cancellationToken = default)
    => ModifySessionAsync("delete-session", session, null, cancellationToken: cancellationToken);

    public virtual async Task<CasdoorSession?> GetSessionAsync(string name, string application, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("sessionPkId", $"{_options.OrganizationName}/{name}/{application}").QueryMap;
        string url = _options.GetActionUrl("get-session", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorSession?>();
    }

    public virtual async Task<IEnumerable<CasdoorSession>?> GetSessionsAsync(CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-sessions", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorSession>?>();
    }

    public virtual async Task<IEnumerable<CasdoorSession>?> GetPaginationSessionsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        string url = _options.GetActionUrl("get-sessions", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorSession>?>();
    }

    private Task<CasdoorResponse?> ModifySessionAsync(string action, CasdoorSession session, IEnumerable<string>? columns = default, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{session.Owner}/{session.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        session.Owner = _options.OrganizationName;

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, session, cancellationToken);
    }
}
