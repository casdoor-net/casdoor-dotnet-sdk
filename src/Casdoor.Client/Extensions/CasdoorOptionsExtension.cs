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

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Casdoor.Client;


public static class CasdoorClientOptionsExtension
{
    internal static string GetSigninUrl(this CasdoorOptions options, string redirectUrl)
    {
        string state = options.ApplicationName;
        string urlEncodeRedirectUrl = Base64UrlEncoder.Encode(redirectUrl);
        return
            $"{options.Endpoint}{options.Path.LoginAuthorizePath}?client_id={options.ClientId}&response_type=code&redirect_uri={redirectUrl}&scope={options.Scope}&state={state}";
    }

    internal static string GetSigninUrl(this CasdoorOptions options, string redirectUrl, string codeVerifier, bool noRedirect)
    {
        var baseUrl = GetSigninUrl(options, redirectUrl);

        var sha256Instance = SHA256.Create();
        byte[] bytes = Encoding.Default.GetBytes(codeVerifier);
        byte[] chanllengeCodeEncoded = sha256Instance.ComputeHash(bytes);
        string chanllengeCodeBase64Encoded = Convert.ToBase64String(chanllengeCodeEncoded).Replace("+", "-").Replace("/", "_").Replace("=", "");

        return $"{baseUrl}&code_challenge={chanllengeCodeBase64Encoded}&code_challenge_method=S256&noRedirect={noRedirect.ToString().ToLower()}";
    }

    internal static string GetActionUrl(this CasdoorOptions options, string action,
        IEnumerable<KeyValuePair<string, string?>>? queryMap = null)
    {
        StringBuilder queryBuilder = new();
        if (queryMap is not null)
        {
            bool first = true;
            foreach (KeyValuePair<string, string?> pair in queryMap)
            {
                queryBuilder.AppendKeyValuePair(in pair, first);
                first = false;
            }
        }
        string query = queryBuilder.ToString();
        return $"{options.Endpoint}/api/{action}{query}";
    }

    private static void AppendKeyValuePair(this StringBuilder builder, in KeyValuePair<string, string?> pair,
        bool first)
    {
        builder.Append(first ? '?' : '&');
        builder.Append(UrlEncoder.Default.Encode(pair.Key));
        builder.Append('=');
        if (string.IsNullOrEmpty(pair.Value) is false)
        {
            builder.Append(UrlEncoder.Default.Encode(pair.Value!));
        }
    }

    internal static string GetSignupUrl(this CasdoorOptions options) => options.GetSignupUrl("", true);

    internal static string GetSignupUrl(this CasdoorOptions options, string redirectUrl, bool enablePassword = false)
    {
        if (enablePassword)
        {
            return $"{options.Endpoint}/signup/{options.ApplicationName}";
        }

        const string scope = "read";
        string state = options.ApplicationName;
        //string urlEncodeRedirectUrl = Base64UrlEncoder.Encode(redirectUrl);
        return
            $"{options.Endpoint}{options.Path.SignupAuthorizePath}?client_id={options.ClientId}&response_type=code&redirect_uri={redirectUrl}&scope={scope}&state={state}";
    }

    internal static string GetUserProfileUrl(this CasdoorOptions options, string username, string accessToken = "")
    {
        string param = "";
        if (string.IsNullOrWhiteSpace(accessToken) is false)
        {
            param = "?access_token=" + accessToken;
        }
        return $"{options.Endpoint}/users/{options.OrganizationName}/{username}{param}";
    }

    internal static string GetMyProfileUrl(this CasdoorOptions options, string accessToken = "")
    {
        string param = "";
        if (string.IsNullOrWhiteSpace(accessToken) is false)
        {
            param = "?access_token=" + accessToken;
        }
        return $"{options.Endpoint}/account{param}";
    }

    internal static TokenValidationParameters DefaultTokenValidationParameters => new TokenValidationParameters
    {
        ValidateActor = true, ValidateIssuer = true, ValidateAudience = true, ValidateIssuerSigningKey = true,
    };

    internal static IConfigurationManager<OpenIdConnectConfiguration> CreateDefaultOpenIdConnectConfigurationManager(
        this CasdoorOptions options, string metaAddress)
    {
        var retriever = new HttpDocumentRetriever
        {
            RequireHttps = options.RequireHttpsMetadata
        };
        return new ConfigurationManager<OpenIdConnectConfiguration>(metaAddress,
            new OpenIdConnectConfigurationRetriever(), retriever);
    }

    internal static CasdoorOptions LoadJwtPublicKey(this CasdoorOptions options)
    {
        if (options.Protocols.JwtCert.Count is 0)
        {
            throw new ArgumentException("JwtCert must has 1 at least.");
        }

        var signKeys = new List<SecurityKey>();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var cert in options.Protocols.JwtCert)
        {
            X509Certificate2 certificate2 = X509CertificateLoader.LoadPkcs12FromFile(
                cert.FilePath, cert.Password
            );
            X509SecurityKey securityKey = new(certificate2);
            signKeys.Add(securityKey);
        }
        options.Protocols.TokenValidationParameters ??= DefaultTokenValidationParameters;
        options.Protocols.TokenValidationParameters.IssuerSigningKeys = signKeys;
        return options;
    }

    public static Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(this CasdoorOptions options, CancellationToken cancellationToken = default)
    {
        if (options.Protocols.OpenIdConnectConfigurationManager is null)
        {
            throw new NullReferenceException(nameof(options.Protocols.OpenIdConnectConfigurationManager));
        }
        return options.Protocols.OpenIdConnectConfigurationManager.GetConfigurationAsync(cancellationToken);
    }
    public static async Task LoadRemoteJwtPublicKeyAsync(this CasdoorOptions options)
    {
        var configuration = await options.GetOpenIdConnectConfigurationAsync();
        options.Protocols.TokenValidationParameters ??= DefaultTokenValidationParameters;
        options.Protocols.TokenValidationParameters.IssuerSigningKeys = configuration.SigningKeys;
    }

    public static CasdoorOptions Validate(this CasdoorOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Endpoint))
        {
            throw new ArgumentException("Endpoint is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            throw new ArgumentException("ClientId is required.");
        }

        if (string.IsNullOrWhiteSpace(options.OrganizationName))
        {
            throw new ArgumentException("OrganizationName is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApplicationName))
        {
            throw new ArgumentException("ApplicationName is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApplicationType))
        {
            throw new ArgumentException("ApplicationType is required.");
        }

        if (string.IsNullOrWhiteSpace(options.Protocols.Authority))
        {
            options.Protocols.Authority = options.Endpoint;
        }

        if (string.IsNullOrWhiteSpace(options.Protocols.Issuer))
        {
            options.Protocols.Issuer = options.Endpoint;
        }

        if (string.IsNullOrWhiteSpace(options.Protocols.Audience))
        {
            options.Protocols.Audience = options.ClientId;
        }

        options.Protocols.TokenValidationParameters ??= DefaultTokenValidationParameters;

        if (options.Protocols.AutoDiscovery is false)
        {
            options = LoadJwtPublicKey(options);

            if (options.Protocols.TokenValidationParameters is null)
            {
                throw new ArgumentException("TokenValidationParameters is required when AutoDiscovery is false.");
            }

            if (options.Protocols.OpenIdConnectConfiguration is null)
            {
                throw new ArgumentException("OpenIdConnectConfiguration is required when AutoDiscovery is false.");
            }

            options.Protocols.OpenIdConnectConfiguration.Issuer ??= options.Protocols.Issuer;

            options.Protocols.OpenIdConnectConfiguration.AuthorizationEndpoint ??=
                $"{options.Endpoint}{options.Path.LoginAuthorizePath}";

            options.Protocols.OpenIdConnectConfiguration.TokenEndpoint ??=
                $"{options.Endpoint}{options.Path.TokenPath}";

            options.Protocols.OpenIdConnectConfiguration.JsonWebKeySet ??= new JsonWebKeySet();
            foreach (var key in options.Protocols.TokenValidationParameters.IssuerSigningKeys)
            {
                var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(key);
                options.Protocols.OpenIdConnectConfiguration.JsonWebKeySet.Keys.Add(jwk);
            }
            options.Protocols.OpenIdConnectConfigurationManager ??=
                new StaticConfigurationManager<OpenIdConnectConfiguration>(options.Protocols
                    .OpenIdConnectConfiguration);
        }
        else
        {
            string metadataAddress = options.Protocols.Authority.TrimEnd('/') + "/.well-known/openid-configuration";
            options.Protocols.OpenIdConnectConfigurationManager ??= options.CreateDefaultOpenIdConnectConfigurationManager(metadataAddress);
            _ = options.LoadRemoteJwtPublicKeyAsync();
            options.Protocols.TokenValidationParameters.ValidAudience = options.Protocols.Audience;
            options.Protocols.TokenValidationParameters.ValidIssuer = options.Protocols.Issuer;
        }
        return options;
    }
}
