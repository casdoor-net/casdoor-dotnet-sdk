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

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<CasdoorResponse?> AddApplicationAsync(CasdoorApplication application, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(application.Owner))
        {
            application.Owner = CasdoorConstants.DefaultCasdoorOwner;
        }

        var url = _options.GetActionUrl("add-application");
        return await PostAsJsonAsync(url, application, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> DeleteApplicationAsync(string name, CancellationToken cancellationToken = default)
    {
        var application = new CasdoorApplication {Owner = CasdoorConstants.DefaultCasdoorOwner, Name = name};
        var url = _options.GetActionUrl("delete-application");
        return await PostAsJsonAsync(url, application, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> UpdateApplicationAsync(string id, CasdoorApplication newApplication, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;

        if (string.IsNullOrEmpty(newApplication.Owner))
        {
            newApplication.Owner = CasdoorConstants.DefaultCasdoorOwner;
        }

        var url = _options.GetActionUrl("update-application", queryMap);
        return await PostAsJsonAsync(url, newApplication, cancellationToken);
    }

    public virtual async Task<CasdoorApplication?> GetApplicationAsync(string id, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;
        var url = _options.GetActionUrl("get-application", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorApplication?>();
    }

    public virtual async Task<IEnumerable<CasdoorApplication>?> GetApplicationsAsync(string owner, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner).QueryMap;
        var url = _options.GetActionUrl("get-applications", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorApplication>?>();
    }

    public virtual async Task<IEnumerable<CasdoorApplication>?> GetOrganizationApplicationsAsync(string organization, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("organization", organization).QueryMap;
        var url = _options.GetActionUrl("get-organization-applications", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorApplication>?>();
    }

    public virtual async Task<CasdoorApplication?> GetUserApplicationAsync(string id, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;
        var url = _options.GetActionUrl("get-user-application", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorApplication?>();
    }
}
