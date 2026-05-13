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

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json;
using Duende.IdentityModel.Client;

namespace Casdoor.Client;

public partial class CasdoorClient
{
    private readonly ClientCredentialsTokenRequest _credentialsTokenRequest = new ClientCredentialsTokenRequest();

    private async Task<T> ApplyConfigurationAsync<T>(T request, CancellationToken cancellationToken = default) where T : TokenRequest
    {
        var configuration = await _options.GetOpenIdConnectConfigurationAsync(cancellationToken: cancellationToken);
        request.Address = configuration.TokenEndpoint;
        request.ClientId = _options.ClientId;
        request.ClientSecret = _options.ClientSecret;
        return request;
    }

    public virtual async Task<TokenResponse> RequestClientCredentialsTokenAsync(CancellationToken cancellationToken = default)
    {
        var request = _credentialsTokenRequest;
        request.Scope = _options.Scope;
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestClientCredentialsTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenResponse> RequestPasswordTokenAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var request = new PasswordTokenRequest { UserName = username, Password = password };
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestPasswordTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenResponse> RequestAuthorizationCodeTokenAsync(string code, string redirectUri,
        string codeVerifier = "", CancellationToken cancellationToken = default)
    {
        var request = new AuthorizationCodeTokenRequest
        {
            Code = code,
            RedirectUri = redirectUri,
            CodeVerifier = codeVerifier,
            ClientId = _options.ClientId
        };
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestAuthorizationCodeTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var request = new RefreshTokenRequest { RefreshToken = refreshToken };
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestRefreshTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual CasdoorUser? ParseJwtToken(string token, bool validateToken = true)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwtToken;
        if (validateToken)
        {
            handler.ValidateToken(token, _options.Protocols.TokenValidationParameters, out var validatedToken);
            jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken is null)
            {
                throw new InvalidOperationException("Invalid JWT token");
            }
        }
        else
        {
            jwtToken = handler.ReadJwtToken(token);
        }

        var result = JsonSerializer.Deserialize<CasdoorUser>(jwtToken.Payload.SerializeToJson());
        return result;
    }

    public virtual Task<CasdoorResponse?> AddTokenAsync(CasdoorToken casdoorToken,
        CancellationToken cancellationToken = default)
    {
        var url = _options.GetActionUrl("add-token");
        return PostAsJsonAsync(url, casdoorToken, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> DeleteTokenAsync(CasdoorToken casdoorToken,
        CancellationToken cancellationToken = default)
    {
        var url = _options.GetActionUrl("delete-token");
        return PostAsJsonAsync(url, casdoorToken, cancellationToken);
    }

    public virtual async Task<CasdoorResponse?> GetCaptchaStatusAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("id", id).QueryMap;
        var url = _options.GetActionUrl("get-captcha-status", queryMap);
        return await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
    }

    public virtual async Task<CasdoorToken?> GetTokenAsync(string owner, string name,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;

        var url = _options.GetActionUrl("get-token", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<CasdoorToken?>();
    }

    public virtual async Task<IEnumerable<CasdoorToken>?> GetTokensAsync(string owner,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", owner).QueryMap;
        var url = _options.GetActionUrl("get-tokens", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorToken>?>();
    }

    public virtual async Task<IEnumerable<CasdoorToken>?> GetPaginationTokensAsync(string owner, int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default)
    {

        queryMap ??= new List<KeyValuePair<string, string?>>();
        queryMap.Add(new KeyValuePair<string, string?>("owner", owner));
        queryMap.Add(new KeyValuePair<string, string?>("pageSize", pageSize.ToString()));
        queryMap.Add(new KeyValuePair<string, string?>("p", p.ToString()));

        var url = _options.GetActionUrl("get-tokens", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorToken>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateTokenAsync(CasdoorToken token, IEnumerable<string>? propertyNames = default, CancellationToken cancellationToken = default)
        => ModifyTokenAsync("update-token", token, propertyNames, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateTokenColumnsAsync(CasdoorToken token, IEnumerable<string>? columns = default, CancellationToken cancellationToken = default)
        => ModifyTokenAsync("update-token", token, columns, cancellationToken: cancellationToken);

    private Task<CasdoorResponse?> ModifyTokenAsync(string action, CasdoorToken token, IEnumerable<string>? columns, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{token.Owner}/{token.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, token, cancellationToken);
    }
}
