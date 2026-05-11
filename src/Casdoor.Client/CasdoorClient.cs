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
using Duende.IdentityModel.Client;

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

    public CasdoorClient SetBearerToken(string accessToken)
    {
        _httpClient.SetBearerToken(accessToken);
        return this;
    }

    public string GetSigninUrl(string redirectUrl) => _options.GetSigninUrl(redirectUrl);
    public string GetSigninUrl(string codeVerifier, bool noRedirect) => _options.GetSigninUrl(_options.CallbackPath, codeVerifier, noRedirect);
}
