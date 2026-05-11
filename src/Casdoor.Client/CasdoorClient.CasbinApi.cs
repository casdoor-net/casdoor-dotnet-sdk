// Copyright 2023 The Casdoor Authors. All Rights Reserved.
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

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Duende.IdentityModel.Client;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    public virtual async Task<IReadOnlyCollection<bool>?> EnforceAsync(
        IEnumerable<string> permissionRule,
        string? permissionId,
        string? modelId,
        string? resourceId,
        string? enforcerId,
        CancellationToken cancellationToken = default)
    {
        var response = await DoEnforceAsync(
            "enforce",
            JsonSerializer.Serialize(permissionRule),
            permissionId,
            modelId,
            resourceId,
            enforcerId,
            cancellationToken);

        return response.DeserializeData<IReadOnlyCollection<bool>?>();
    }

    public virtual async Task<IReadOnlyCollection<IReadOnlyCollection<bool>>?> BatchEnforceAsync(
        IEnumerable<IEnumerable<string>> permissionRule,
        string? permissionId,
        string? modelId,
        string? enforcerId,
        CancellationToken cancellationToken = default)
    {
        var response = await DoEnforceAsync(
            "batch-enforce",
            JsonSerializer.Serialize(permissionRule),
            permissionId,
            modelId,
            null,
            enforcerId,
            cancellationToken);

        return response.DeserializeData<IReadOnlyCollection<IReadOnlyCollection<bool>>?>();
    }

    private async Task<CasdoorResponse> DoEnforceAsync(
        string url,
        string data,
        string? permissionId,
        string? modelId,
        string? resourceId,
        string? enforcerId,
        CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder();

        if (!string.IsNullOrEmpty(permissionId))
        {
            queryMapBuilder.Add(permissionId!);
            queryMapBuilder.Add("permissionId", permissionId!);
        }

        if (!string.IsNullOrEmpty(modelId))
        {
            queryMapBuilder.Add(modelId!);
            queryMapBuilder.Add("modelId", modelId!);
        }

        if (!string.IsNullOrEmpty(resourceId))
        {
            queryMapBuilder.Add(resourceId!);
            queryMapBuilder.Add("resourceId", resourceId!);
        }

        if (!string.IsNullOrEmpty(enforcerId))
        {
            queryMapBuilder.Add(enforcerId!);
            queryMapBuilder.Add("enforcerId", enforcerId!);
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(_options.GetActionUrl(url, queryMapBuilder.QueryMap)),
            Content = new StringContent(
                data,
                Encoding.UTF8,
                "application/json")
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        if (_options.ClientSecret != "")
        {
            request.SetBasicAuthentication(_options.ClientId, _options.ClientSecret);
        }

        var response = await _httpClient.SendAsync(request, cancellationToken);
        string responseContent = await response.Content.ReadAsStringAsync(); // netstandard2.0 does not support cancellationToken
        return JsonSerializer.Deserialize<CasdoorResponse>(responseContent)!;
    }

    public virtual Task<IEnumerable<string>?> GetAllObjectsAsync(CancellationToken cancellationToken = default) =>
        GetAllAsync("get-all-objects", cancellationToken);

    public virtual Task<IEnumerable<string>?> GetAllActionsAsync(CancellationToken cancellationToken = default) =>
        GetAllAsync("get-all-actions", cancellationToken);


    public virtual Task<IEnumerable<string>?> GetAllRolesAsync(CancellationToken cancellationToken = default) =>
        GetAllAsync("get-all-roles", cancellationToken);

    private async Task<IEnumerable<string>?> GetAllAsync(string url, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(_options.GetActionUrl(url), cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<string>>();
    }
}
