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

using System.Text.Json.Serialization;

namespace Casdoor.Client;

public class CasdoorProviderItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("canSignUp")]
    public bool? CanSignUp { get; set; }

    [JsonPropertyName("canSignIn")]
    public bool? CanSignIn { get; set; }

    [JsonPropertyName("canUnlink")]
    public bool? CanUnlink { get; set; }

    [JsonPropertyName("prompted")]
    public bool? Prompted { get; set; }

    [JsonPropertyName("alertType")]
    public string? AlertType { get; set; }

    [JsonPropertyName("provider")]
    public CasdoorProvider? Provider { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("bindingRule")]
    public IEnumerable<string>? BindingRule { get; set; }

    [JsonPropertyName("countryCodes")]
    public IEnumerable<string>? CountryCodes { get; set; }

    [JsonPropertyName("signupGroup")]
    public string? SignupGroup { get; set; }

    [JsonPropertyName("rule")]
    public string? Rule { get; set; }
}

public class CasdoorSignupItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("prompted")]
    public bool? Prompted { get; set; }

    [JsonPropertyName("rule")]
    public string? Rule { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("customCss")]
    public string? CustomCss { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("options")]
    public IEnumerable<string>? Options { get; set; }

    [JsonPropertyName("regex")]
    public string? Regex { get; set; }
}

public class CasdoorApplication
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("homepageUrl")]
    public string? HomepageUrl { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }

    [JsonPropertyName("cert")]
    public string? Cert { get; set; }

    [JsonPropertyName("enablePassword")]
    public bool? EnablePassword { get; set; }

    [JsonPropertyName("enableSignUp")]
    public bool? EnableSignUp { get; set; }

    [JsonPropertyName("enableSigninSession")]
    public bool? EnableSigninSession { get; set; }

    [JsonPropertyName("enableCodeSignin")]
    public bool? EnableCodeSignin { get; set; }

    [JsonPropertyName("enableAutoSignin")]
    public bool? EnableAutoSignin { get; set; }

    [JsonPropertyName("enableSamlCompress")]
    public bool? EnableSamlCompress { get; set; }

    [JsonPropertyName("enableWebAuth")]
    public bool? EnableWebAuth { get; set; }

    [JsonPropertyName("enableLinkWithEmail")]
    public bool? EnableLinkWithEmail { get; set; }

    [JsonPropertyName("orgChoiceMode")]
    public string? OrgChoiceMode { get; set; }

    [JsonPropertyName("samlReplyUrl")]
    public string? SamlReplyUrl { get; set; }

    [JsonPropertyName("providers")]
    public CasdoorProviderItem[]? Providers { get; set; }

    [JsonPropertyName("signupItems")]
    public CasdoorSignupItem[]? SignupItems { get; set; }

    [JsonPropertyName("grantTypes")]
    public string[]? GrantTypes { get; set; }

    [JsonPropertyName("organizationObj")]
    public CasdoorOrganization? OrganizationObj { get; set; }

    [JsonPropertyName("tags")]
    public string[]? Tags { get; set; }

    [JsonPropertyName("clientId")]
    public string? ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("redirectUris")]
    public string[]? RedirectUris { get; set; }

    [JsonPropertyName("tokenFormat")]
    public string? TokenFormat { get; set; }

    [JsonPropertyName("expireInHours")]
    public int? ExpireInHours { get; set; }

    [JsonPropertyName("refreshExpireInHours")]
    public int? RefreshExpireInHours { get; set; }

    [JsonPropertyName("signupUrl")]
    public string? SignupUrl { get; set; }

    [JsonPropertyName("signinUrl")]
    public string? SigninUrl { get; set; }

    [JsonPropertyName("forgetUrl")]
    public string? ForgetUrl { get; set; }

    [JsonPropertyName("affiliationUrl")]
    public string? AffiliationUrl { get; set; }

    [JsonPropertyName("termsOfUse")]
    public string? TermsOfUse { get; set; }

    [JsonPropertyName("signupHtml")]
    public string? SignupHtml { get; set; }

    [JsonPropertyName("signinHtml")]
    public string? SigninHtml { get; set; }

    [JsonPropertyName("themeData")]
    public CasdoorThemeData? ThemeData { get; set; }

    [JsonPropertyName("formCss")]
    public string? FormCss { get; set; }

    [JsonPropertyName("formCssMobile")]
    public string? FormCssMobile { get; set; }

    [JsonPropertyName("formOffset")]
    public int? FormOffset { get; set; }

    [JsonPropertyName("formBackgroundUrl")]
    public string? FormBackgroundUrl { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("scopes")]
    public IEnumerable<object>? Scopes { get; set; }

    [JsonPropertyName("logoDark")]
    public string? LogoDark { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("favicon")]
    public string? Favicon { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("defaultGroup")]
    public string? DefaultGroup { get; set; }

    [JsonPropertyName("defaultTag")]
    public string? DefaultTag { get; set; }

    [JsonPropertyName("headerHtml")]
    public string? HeaderHtml { get; set; }

    [JsonPropertyName("pageHtml")]
    public string? PageHtml { get; set; }

    [JsonPropertyName("enableGuestSignin")]
    public bool EnableGuestSignin { get; set; }

    [JsonPropertyName("disableSignin")]
    public bool DisableSignin { get; set; }

    [JsonPropertyName("enableExclusiveSignin")]
    public bool EnableExclusiveSignin { get; set; }

    [JsonPropertyName("maxSessions")]
    public int MaxSessions { get; set; }

    [JsonPropertyName("enableSamlC14n10")]
    public bool EnableSamlC14n10 { get; set; }

    [JsonPropertyName("enableSamlPostBinding")]
    public bool EnableSamlPostBinding { get; set; }

    [JsonPropertyName("disableSamlAttributes")]
    public bool DisableSamlAttributes { get; set; }

    [JsonPropertyName("enableSamlAssertionSignature")]
    public bool EnableSamlAssertionSignature { get; set; }

    [JsonPropertyName("useEmailAsSamlNameId")]
    public bool UseEmailAsSamlNameId { get; set; }

    [JsonPropertyName("enableWebAuthn")]
    public bool EnableWebAuthn { get; set; }

    [JsonPropertyName("samlSingleLogoutUrl")]
    public string? SamlSingleLogoutUrl { get; set; }

    [JsonPropertyName("signinMethods")]
    public IEnumerable<object>? SigninMethods { get; set; }

    [JsonPropertyName("signinItems")]
    public IEnumerable<object>? SigninItems { get; set; }

    [JsonPropertyName("certPublicKey")]
    public string? CertPublicKey { get; set; }

    [JsonPropertyName("samlAttributes")]
    public IEnumerable<object>? SamlAttributes { get; set; }

    [JsonPropertyName("samlHashAlgorithm")]
    public string? SamlHashAlgorithm { get; set; }

    [JsonPropertyName("samlC14nPrefix")]
    public string? SamlC14nPrefix { get; set; }

    [JsonPropertyName("isShared")]
    public bool IsShared { get; set; }

    [JsonPropertyName("ipRestriction")]
    public string? IpRestriction { get; set; }

    [JsonPropertyName("clientCert")]
    public string? ClientCert { get; set; }

    [JsonPropertyName("backchannelLogoutUri")]
    public string? BackchannelLogoutUri { get; set; }

    [JsonPropertyName("forcedRedirectOrigin")]
    public string? ForcedRedirectOrigin { get; set; }

    [JsonPropertyName("tokenSigningMethod")]
    public string? TokenSigningMethod { get; set; }

    [JsonPropertyName("tokenFields")]
    public IEnumerable<string>? TokenFields { get; set; }

    [JsonPropertyName("tokenAttributes")]
    public IEnumerable<object>? TokenAttributes { get; set; }

    [JsonPropertyName("tokenGroupFormat")]
    public string? TokenGroupFormat { get; set; }

    [JsonPropertyName("cookieExpireInHours")]
    public long CookieExpireInHours { get; set; }

    [JsonPropertyName("ipWhitelist")]
    public string? IpWhitelist { get; set; }

    [JsonPropertyName("footerHtml")]
    public string? FooterHtml { get; set; }

    [JsonPropertyName("formSideHtml")]
    public string? FormSideHtml { get; set; }

    [JsonPropertyName("formBackgroundUrlMobile")]
    public string? FormBackgroundUrlMobile { get; set; }

    [JsonPropertyName("failedSigninLimit")]
    public int FailedSigninLimit { get; set; }

    [JsonPropertyName("failedSigninFrozenTime")]
    public int FailedSigninFrozenTime { get; set; }

    [JsonPropertyName("codeResendTimeout")]
    public int CodeResendTimeout { get; set; }

    [JsonPropertyName("customScopes")]
    public IEnumerable<object>? CustomScopes { get; set; }

    [JsonPropertyName("domain")]
    public string? Domain { get; set; }

    [JsonPropertyName("otherDomains")]
    public IEnumerable<string>? OtherDomains { get; set; }

    [JsonPropertyName("upstreamHost")]
    public string? UpstreamHost { get; set; }

    [JsonPropertyName("sslMode")]
    public string? SslMode { get; set; }

    [JsonPropertyName("sslCert")]
    public string? SslCert { get; set; }

    [JsonPropertyName("CertObj")]
    public CasdoorCert? CertObj { get; set; }

    [JsonPropertyName("registrationAccessToken")]
    public string? RegistrationAccessToken { get; set; }
}
