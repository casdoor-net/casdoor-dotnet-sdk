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

using Duende.IdentityModel.Client;

namespace Casdoor.Client;

public interface ICasdoorTokenClient
{
    public Task<TokenResponse> RequestClientCredentialsTokenAsync(CancellationToken cancellationToken = default);
    public Task<TokenResponse> RequestPasswordTokenAsync(string username, string password, CancellationToken cancellationToken = default);
    public Task<TokenResponse> RequestAuthorizationCodeTokenAsync(string code, string redirectUri, string codeVerifier = "", CancellationToken cancellationToken = default);
    public Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> AddTokenAsync(CasdoorToken casdoorToken, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteTokenAsync(CasdoorToken casdoorToken, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> GetCaptchaStatusAsync(string id, CancellationToken cancellationToken = default);
    public Task<CasdoorToken?> GetTokenAsync(string owner, string name, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorToken>?> GetTokensAsync(string owner,CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorToken>?> GetPaginationTokensAsync(string owner, int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateTokenAsync(CasdoorToken casdoorToken, IEnumerable<string>? propertyNames = default, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateTokenColumnsAsync(CasdoorToken token, IEnumerable<string>? columns = default, CancellationToken cancellationToken = default);

}
