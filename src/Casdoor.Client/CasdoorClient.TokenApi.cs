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
using IdentityModel.Client;

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
        var request = new PasswordTokenRequest {UserName = username, Password = password};
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestPasswordTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenResponse> RequestAuthorizationCodeTokenAsync(string code, string redirectUri,
        string codeVerifier = "", CancellationToken cancellationToken = default)
    {
        var request = new AuthorizationCodeTokenRequest
        {
            Code = code, RedirectUri = redirectUri, CodeVerifier = codeVerifier, ClientId = _options.ClientId
        };
        request = await ApplyConfigurationAsync(request, cancellationToken);
        return await _httpClient.RequestAuthorizationCodeTokenAsync(request, cancellationToken: cancellationToken);
    }

    public virtual async Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var request = new RefreshTokenRequest {RefreshToken = refreshToken};
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
        return await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
    }

    public virtual async Task<CasdoorToken?> GetTokenAsync(string owner, string name,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{owner}/{name}").QueryMap;

        var url = _options.GetActionUrl("get-token", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<CasdoorToken?>();
    }

    public virtual async Task<IEnumerable<CasdoorToken>?> GetTokensAsync(string owner,
        CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", owner).QueryMap;
        var url = _options.GetActionUrl("get-tokens", queryMap);
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
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
        var result = await GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorToken>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateTokenAsync(CasdoorToken token, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default)
        => ModifyTokenAsync("update-token", token, propertyNames, cancellationToken: cancellationToken);

    public virtual Task<CasdoorResponse?> UpdateTokenColumnsAsync(CasdoorToken token, IEnumerable<string>? columns, CancellationToken cancellationToken = default)
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

    public virtual Task<CasdoorResponse?> UpdateTokenForColumnsAsync(CasdoorToken token, IEnumerable<string> columns,
        CancellationToken cancellationToken = default) =>
        UpdateTokenColumnsAsync(token, columns, cancellationToken);

    /// <summary>
    ///     Introspects the token (RFC 7662), the result contains "active" and the claims of the token.
    /// </summary>
    public virtual async Task<TokenIntrospectionResponse> IntrospectTokenAsync(string token, string tokenTypeHint = "access_token",
        CancellationToken cancellationToken = default) =>
        await _httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
        {
            Address = $"{_options.Endpoint.TrimEnd('/')}/api/login/oauth/introspect",
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            Token = token,
            TokenTypeHint = tokenTypeHint
        }, cancellationToken);

    /// <summary>
    ///     Gets the token with the Resource Owner Password Credentials grant, the same as RequestPasswordTokenAsync().
    /// </summary>
    public virtual Task<TokenResponse> GetOAuthTokenByPasswordAsync(string username, string password,
        CancellationToken cancellationToken = default) =>
        RequestPasswordTokenAsync(username, password, cancellationToken);

    /// <summary>
    ///     Signs in as any user of the organization with the organization's master password.
    /// </summary>
    public virtual Task<TokenResponse> ImpersonateUserAsync(string username, string masterPassword,
        CancellationToken cancellationToken = default) =>
        RequestPasswordTokenAsync(username, masterPassword, cancellationToken);

    public virtual Task<TokenResponse> RefreshOAuthTokenAsync(string refreshToken, CancellationToken cancellationToken = default) =>
        RequestRefreshTokenAsync(refreshToken, cancellationToken);

    /// <summary>
    ///     Signs the user out of all the applications and devices (SSO logout).
    /// </summary>
    public virtual Task<CasdoorResponse?> LogoutAsync(string accessToken, CancellationToken cancellationToken = default) =>
        SsoLogoutAsync(accessToken, true, cancellationToken);

    /// <summary>
    ///     Only signs the user out of the session of the access token.
    /// </summary>
    public virtual Task<CasdoorResponse?> LogoutCurrentSessionAsync(string accessToken, CancellationToken cancellationToken = default) =>
        SsoLogoutAsync(accessToken, false, cancellationToken);

    private async Task<CasdoorResponse?> SsoLogoutAsync(string accessToken, bool logoutAll, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentException("the access token should not be empty", nameof(accessToken));
        }
        var queryMap = new QueryMapBuilder().Add("logoutAll", logoutAll ? "true" : "false").QueryMap;
        var client = new CasdoorClient(_httpClient, _options) { _accessToken = accessToken };
        return await client.PostAsJsonAsync(_options.GetActionUrl("sso-logout", queryMap), string.Empty, cancellationToken);
    }
}
