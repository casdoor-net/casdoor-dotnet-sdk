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
using IdentityModel.Client;

namespace Casdoor.Client;

public partial class CasdoorClient : ICasdoorClient
{
    private readonly HttpClient _httpClient;
    private readonly CasdoorOptions _options;

    public CasdoorClient(HttpClient httpClient, CasdoorOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        options.Validate();
    }

    /// <summary>
    ///     Makes this client call the APIs as the user who owns the access token.
    ///     Prefer WithAccessToken(), which doesn't change this client.
    /// </summary>
    public CasdoorClient SetBearerToken(string accessToken)
    {
        _accessToken = accessToken;
        return this;
    }

    /// <summary>
    ///     Returns a new client that calls the APIs as the user who owns the access token
    ///     (Authorization: Bearer) instead of as the application. This client is not changed,
    ///     so it's safe to create one per request.
    /// </summary>
    public ICasdoorClient WithAccessToken(string accessToken) =>
        new CasdoorClient(_httpClient, _options) { _accessToken = accessToken };

    public string GetSigninUrl(string redirectUrl) => _options.GetSigninUrl(redirectUrl);
    public string GetSigninUrl(string codeVerifier, bool noRedirect) => _options.GetSigninUrl(_options.CallbackPath, codeVerifier, noRedirect);
}
