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
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using IdentityModel.Client;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    // set by WithAccessToken() to call the APIs as the user who owns the token instead of as the application
    private string? _accessToken;

    /// <summary>
    ///     Creates a request with the authentication of this client: the user's access token (Bearer) if set,
    ///     otherwise the application's client ID and secret (Basic), and the custom headers of the options.
    ///     The headers are set per request, so the shared HttpClient is never changed.
    /// </summary>
    internal HttpRequestMessage CreateRequest(HttpMethod method, string? requestUri, HttpContent? content = null)
    {
        var request = new HttpRequestMessage(method, requestUri) { Content = content };
        if (!string.IsNullOrEmpty(_accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        else if (!string.IsNullOrEmpty(_options.ClientSecret))
        {
            request.SetBasicAuthenticationOAuth(_options.ClientId, _options.ClientSecret);
        }

        foreach (var header in _options.CustomHeaders)
        {
            request.Headers.Remove(header.Key);
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        return request;
    }

    internal async Task<TValue?> GetFromJsonAsync<TValue>(string? requestUri, CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Get, requestUri);
        var response = await _httpClient.SendAsync(request, cancellationToken);
        using var stream = await response.Content.ReadAsStreamAsync();
        TValue? successResult;
        try
        {
            successResult = await JsonSerializer.DeserializeAsync<TValue>(stream, cancellationToken: cancellationToken);
        }
        catch (JsonException e)
        {
            throw new CasdoorApiException($"Server response cannot be deserialized as type {typeof(TValue).FullName}. Server API and SDK implementation are inconsistent.", e);
        }
        return successResult;
    }

    internal async Task<CasdoorResponse?> PostAsJsonAsync<TValue>(string? requestUri, TValue value, CancellationToken cancellationToken = default)
    {
        using var request = CreateRequest(HttpMethod.Post, requestUri, JsonContent.Create(value));
        HttpResponseMessage resp = await _httpClient.SendAsync(request, cancellationToken);
        return await resp.ToCasdoorResponse(cancellationToken);
    }

    internal async Task<CasdoorResponse?> PostFileAsync(string? requestUri, StreamContent postStream, CancellationToken cancellationToken = default)
    {
        using var formData = new MultipartFormDataContent();
        formData.Add(postStream, "file", "file");
        using var request = CreateRequest(HttpMethod.Post, requestUri, formData);
        HttpResponseMessage resp = await _httpClient.SendAsync(request, cancellationToken);
        return await resp.ToCasdoorResponse(cancellationToken);
    }

    internal async Task<CasdoorResponse?> PostFormAsync(string? requestUri, IEnumerable<KeyValuePair<string, string?>> form, CancellationToken cancellationToken = default)
    {
        using var formData = new MultipartFormDataContent();
        foreach (var field in form)
        {
            formData.Add(new StringContent(field.Value ?? string.Empty), field.Key);
        }
        using var request = CreateRequest(HttpMethod.Post, requestUri, formData);
        HttpResponseMessage resp = await _httpClient.SendAsync(request, cancellationToken);
        return await resp.ToCasdoorResponse(cancellationToken);
    }

    /// <summary>
    ///     Returns name as is if it's already an "owner/name" ID, otherwise prefixes it with owner, or the organization of the options.
    /// </summary>
    public string GetId(string name, string? owner = null) =>
        name.Contains("/") ? name : $"{owner ?? _options.OrganizationName}/{name}";

    private string GetOwner(string? owner, string? defaultOwner = null) =>
        string.IsNullOrEmpty(owner) ? defaultOwner ?? _options.OrganizationName : owner!;
}
