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

using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual Task<CasdoorResponse?> UploadResourceAsync(
        string user, string tag, string parent, string fullFilePath,
        Stream fileStream, string createdTime = "", string description = "", CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName)
            .Add("user", user)
            .Add("application", _options.ApplicationName)
            .Add("tag", tag)
            .Add("parent", parent)
            .Add("fullFilePath", fullFilePath)
            .Add("createdTime", createdTime)
            .Add("description", description).QueryMap;
        string url = _options.GetActionUrl("upload-resource", queryMap);
        return PostFileAsync(url, new StreamContent(fileStream), cancellationToken: cancellationToken);
    }

    public virtual Task<CasdoorResponse?> DeleteResourceAsync(string name, CancellationToken cancellationToken = default)
    {
        CasdoorUserResource resource = new() {Owner = _options.OrganizationName, Name = name};
        var url = _options.GetActionUrl("delete-resource");
        return PostAsJsonAsync(url, resource, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> AddResourceAsync(CasdoorUserResource casdoorUserResource,
        CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("add-resource");
        return PostAsJsonAsync(url, casdoorUserResource, cancellationToken);
    }

    public virtual async Task<CasdoorUserResource?> GetResourceAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", GetId(name)).QueryMap;
        string url = _options.GetActionUrl("get-resource", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorUserResource?>();
    }

    public virtual async Task<IEnumerable<CasdoorUserResource>?> GetResourcesAsync(string owner, string user, 
        string field, string value, string sortField, string sortOrder, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", owner)
            .Add("user", user)
            .Add("field", field)
            .Add("value", value)
            .Add("sortField", sortField)
            .Add("sortOrder", sortOrder)
            .QueryMap;
        string url = _options.GetActionUrl("get-resources", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorUserResource>?>();
    }

    public virtual async Task<IEnumerable<CasdoorUserResource>?> GetPaginationResourcesAsync(string owner, string user, int pageSize, int p,
        string field, string value, string sortField, string sortOrder, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", owner)
            .Add("user", user)
            .Add("pageSize", pageSize.ToString())
            .Add("p", p.ToString())
            .Add("field", field)
            .Add("value", value)
            .Add("sortField", sortField)
            .Add("sortOrder", sortOrder)
            .QueryMap;
        string url = _options.GetActionUrl("get-resources", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorUserResource>?>();
    }

    public virtual Task<CasdoorUserResource?> GetResourceExAsync(string owner, string name, CancellationToken cancellationToken = default) =>
        GetResourceAsync($"{owner}/{name}", cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateResourceAsync(CasdoorUserResource resource, CancellationToken cancellationToken = default)
    {
        resource.Owner = GetOwner(resource.Owner);
        var queryMap = new QueryMapBuilder().Add("id", $"{resource.Owner}/{resource.Name}").QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("update-resource", queryMap), resource, cancellationToken);
    }

    /// <summary>
    ///     Deletes the resource, the "Direct" tag also deletes the file from the storage provider.
    /// </summary>
    public virtual Task<CasdoorResponse?> DeleteResourceWithTagAsync(CasdoorUserResource resource, string tag,
        CancellationToken cancellationToken = default)
    {
        resource.Owner = GetOwner(resource.Owner);
        var queryMap = new QueryMapBuilder().Add("tag", tag).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("delete-resource", queryMap), resource, cancellationToken);
    }
}
