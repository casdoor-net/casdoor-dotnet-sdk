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


public class CasdoorMfaItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rule")]
    public string? Rule { get; set; }
}

public class CasdoorAccountItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("viewRule")]
    public string? ViewRule { get; set; }

    [JsonPropertyName("modifyRule")]
    public string? ModifyRule { get; set; }

    [JsonPropertyName("regex")]
    public string? Regex { get; set; }

    [JsonPropertyName("tab")]
    public string? Tab { get; set; }
}

public class CasdoorOrganization
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("websiteUrl")]
    public string? WebsiteUrl { get; set; }

    [JsonPropertyName("favicon")]
    public string? Favicon { get; set; }

    [JsonPropertyName("passwordType")]
    public string? PasswordType { get; set; }

    [JsonPropertyName("passwordSalt")]
    public string? PasswordSalt { get; set; }

    [JsonPropertyName("passwordOptions")]
    public string[]? PasswordOptions { get; set; }

    [JsonPropertyName("countryCodes")]
    public string[]? CountryCodes { get; set; }

    [JsonPropertyName("defaultAvatar")]
    public string? DefaultAvatar { get; set; }

    [JsonPropertyName("defaultApplication")]
    public string? DefaultApplication { get; set; }

    [JsonPropertyName("tags")]
    public string[]? Tags { get; set; }

    [JsonPropertyName("languages")]
    public string[]? Languages { get; set; }

    [JsonPropertyName("themeData")]
    public CasdoorThemeData? ThemeData { get; set; }

    [JsonPropertyName("masterPassword")]
    public string? MasterPassword { get; set; }

    [JsonPropertyName("initScore")]
    public int? InitScore { get; set; }

    [JsonPropertyName("enableSoftDeletion")]
    public bool? EnableSoftDeletion { get; set; }

    [JsonPropertyName("isProfilePublic")]
    public bool? IsProfilePublic { get; set; }

    [JsonPropertyName("mfaItems")]
    public CasdoorMfaItem[]? MfaItems { get; set; }

    [JsonPropertyName("accountItems")]
    public CasdoorAccountItem[]? AccountItems { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("logoDark")]
    public string? LogoDark { get; set; }

    [JsonPropertyName("hasPrivilegeConsent")]
    public bool HasPrivilegeConsent { get; set; }

    [JsonPropertyName("passwordObfuscatorType")]
    public string? PasswordObfuscatorType { get; set; }

    [JsonPropertyName("passwordObfuscatorKey")]
    public string? PasswordObfuscatorKey { get; set; }

    [JsonPropertyName("passwordExpireDays")]
    public int PasswordExpireDays { get; set; }

    [JsonPropertyName("passwordHistoryCount")]
    public int PasswordHistoryCount { get; set; }

    [JsonPropertyName("tokenRetentionDays")]
    public int TokenRetentionDays { get; set; }

    [JsonPropertyName("recordRetentionDays")]
    public int RecordRetentionDays { get; set; }

    [JsonPropertyName("usePermanentAvatar")]
    public bool UsePermanentAvatar { get; set; }

    [JsonPropertyName("defaultTokenFormat")]
    public string? DefaultTokenFormat { get; set; }

    [JsonPropertyName("defaultTokenFields")]
    public IEnumerable<string>? DefaultTokenFields { get; set; }

    [JsonPropertyName("userTypes")]
    public IEnumerable<string>? UserTypes { get; set; }

    [JsonPropertyName("defaultPassword")]
    public string? DefaultPassword { get; set; }

    [JsonPropertyName("masterVerificationCode")]
    public string? MasterVerificationCode { get; set; }

    [JsonPropertyName("ipWhitelist")]
    public string? IpWhitelist { get; set; }

    [JsonPropertyName("useEmailAsUsername")]
    public bool UseEmailAsUsername { get; set; }

    [JsonPropertyName("enableTour")]
    public bool EnableTour { get; set; }

    [JsonPropertyName("disableSignin")]
    public bool DisableSignin { get; set; }

    [JsonPropertyName("enableExclusiveSignin")]
    public bool EnableExclusiveSignin { get; set; }

    [JsonPropertyName("maxSessions")]
    public int MaxSessions { get; set; }

    [JsonPropertyName("disableConsole")]
    public bool DisableConsole { get; set; }

    [JsonPropertyName("ipRestriction")]
    public string? IpRestriction { get; set; }

    [JsonPropertyName("navItems")]
    public IEnumerable<string>? NavItems { get; set; }

    [JsonPropertyName("userNavItems")]
    public IEnumerable<string>? UserNavItems { get; set; }

    [JsonPropertyName("widgetItems")]
    public IEnumerable<string>? WidgetItems { get; set; }

    [JsonPropertyName("mfaRememberInHours")]
    public int MfaRememberInHours { get; set; }

    [JsonPropertyName("accountMenu")]
    public string? AccountMenu { get; set; }

    [JsonPropertyName("dcrPolicy")]
    public string? DcrPolicy { get; set; }

    [JsonPropertyName("ldapAttributes")]
    public IEnumerable<string>? LdapAttributes { get; set; }

    [JsonPropertyName("kerberosRealm")]
    public string? KerberosRealm { get; set; }

    [JsonPropertyName("kerberosKdcHost")]
    public string? KerberosKdcHost { get; set; }

    [JsonPropertyName("kerberosKeytab")]
    public string? KerberosKeytab { get; set; }

    [JsonPropertyName("kerberosServiceName")]
    public string? KerberosServiceName { get; set; }

    [JsonPropertyName("orgBalance")]
    public double OrgBalance { get; set; }

    [JsonPropertyName("userBalance")]
    public double UserBalance { get; set; }

    [JsonPropertyName("balanceCredit")]
    public double BalanceCredit { get; set; }

    [JsonPropertyName("balanceCurrency")]
    public string? BalanceCurrency { get; set; }
}
