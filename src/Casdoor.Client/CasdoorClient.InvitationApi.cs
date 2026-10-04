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
    public virtual async Task<IEnumerable<CasdoorInvitation>?> GetInvitationsAsync(string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner ?? _options.OrganizationName).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-invitations", queryMap), cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorInvitation>?>();
    }

    public virtual async Task<(IEnumerable<CasdoorInvitation>? invitations, int totalCount)> GetPaginationInvitationsAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default)
    {
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", _options.OrganizationName));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-invitations", queryMap), cancellationToken);
        return (result.DeserializeData<IEnumerable<CasdoorInvitation>?>(), result.DeserializeData2<int?>() ?? 0);
    }

    public virtual async Task<CasdoorInvitation?> GetInvitationAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", GetId(name)).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-invitation", queryMap), cancellationToken);
        return result.DeserializeData<CasdoorInvitation?>();
    }

    public virtual async Task<CasdoorInvitation?> GetInvitationInfoAsync(string code, string applicationName,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("applicationId", $"admin/{applicationName}").Add("code", code).QueryMap;
        var result = await GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl("get-invitation-info", queryMap), cancellationToken);
        return result.DeserializeData<CasdoorInvitation?>();
    }

    public virtual Task<CasdoorResponse?> AddInvitationAsync(CasdoorInvitation invitation, CancellationToken cancellationToken = default) =>
        ModifyInvitationAsync("add-invitation", invitation, null, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateInvitationAsync(CasdoorInvitation invitation, CancellationToken cancellationToken = default) =>
        ModifyInvitationAsync("update-invitation", invitation, null, cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateInvitationForColumnsAsync(CasdoorInvitation invitation, IEnumerable<string> columns,
        CancellationToken cancellationToken = default) =>
        ModifyInvitationAsync("update-invitation", invitation, columns, cancellationToken);

    public virtual Task<CasdoorResponse?> DeleteInvitationAsync(CasdoorInvitation invitation, CancellationToken cancellationToken = default) =>
        ModifyInvitationAsync("delete-invitation", invitation, null, cancellationToken);

    private Task<CasdoorResponse?> ModifyInvitationAsync(string action, CasdoorInvitation invitation, IEnumerable<string>? columns,
        CancellationToken cancellationToken, List<KeyValuePair<string, string?>>? queryMap = null)
    {
        invitation.Owner = GetOwner(invitation.Owner);
        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("id", $"{invitation.Owner}/{invitation.Name}"));
        if (columns != null && columns.Any())
        {
            queryMap.Add(new KeyValuePair<string, string?>("columns", string.Join(",", columns)));
        }
        return PostAsJsonAsync(_options.GetActionUrl(action, queryMap), invitation, cancellationToken);
    }
}
