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
using Duende.IdentityModel.Client;

namespace Casdoor.Client;

public static class HttpClientExtensions
{
    internal static void SetCasdoorAuthentication(this HttpClient client, CasdoorOptions options) =>
        client.SetBasicAuthenticationOAuth(options.ClientId, options.ClientSecret);

    internal static async Task<CasdoorResponse?> PostFileAsync(this HttpClient client, string? url,
        StreamContent postStream, CancellationToken cancellationToken = default)
    {
        using MultipartFormDataContent formData = new MultipartFormDataContent();
        formData.Add(postStream, "file", "file");

        HttpResponseMessage resp = await client.PostAsync(url, formData, cancellationToken);
        return await resp.ToCasdoorResponse(cancellationToken);
    }

    internal static Task<CasdoorResponse?> ToCasdoorResponse(this HttpResponseMessage response, CancellationToken cancellationToken) =>
        response.Content.ReadFromJsonAsync<CasdoorResponse>(cancellationToken: cancellationToken);

}
