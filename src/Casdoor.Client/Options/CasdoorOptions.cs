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

using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Casdoor.Client;

/// <summary>
///     CasdoorOptions is the core configuration.
///     The first step to use this SDK is to config an instance of CasdoorOptions.
/// </summary>
public class CasdoorOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = string.Empty;
    public string ApplicationType { get; set; } = string.Empty;
    public string CallbackPath { get; set; } = "/casdoor/signin-callback";
    public string Scope { get; set; } = "openid profile email";
    public bool RequireHttpsMetadata { get; set; } = false;
    public CasdoorProtocolsOptions Protocols { get; set; } = new();
    public CasdoorPathOptions Path { get; set; } = new();

    /// <summary>
    ///     The HTTP headers added to all the API requests, e.g. "Accept-Language".
    /// </summary>
    public Dictionary<string, string> CustomHeaders { get; set; } = new();
}

public class CasdoorPathOptions
{
    public string ApiPath { get; set; } = "/api";
    public string LoginAuthorizePath { get; set; } = "/login/oauth/authorize";
    public string SignupAuthorizePath { get; set; } = "/signup/oauth/authorize";
    public string TokenPath { get; set; } = "/api/login/oauth/access_token";
}

public class CasdoorProtocolsOptions
{
    public bool AutoDiscovery { get; set; } = true;
    public string Authority { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public IReadOnlyList<CasdoorJwtCertOptions> JwtCert { get; set; } = Array.Empty<CasdoorJwtCertOptions>();
    public TokenValidationParameters? TokenValidationParameters { get; set; }
    public OpenIdConnectConfiguration? OpenIdConnectConfiguration { get; set; } = new();
    public IConfigurationManager<OpenIdConnectConfiguration>? OpenIdConnectConfigurationManager { get; set; }
}

public class CasdoorJwtCertOptions
{
    public string FilePath { get; set; } = string.Empty;
    public string? Password { get; set; }
}
