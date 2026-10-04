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


public class CasdoorManagedAccount
{
    [JsonPropertyName("application")]
    public string? Application { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("signinUrl")]
    public string? SigninUrl { get; set; }
}

/// <summary>
///     CasdoorUser has the same definition as https://github.com/casdoor/casdoor/blob/master/object/user.go#L24
/// </summary>
public class CasdoorUser
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("updatedTime")]
    public string? UpdatedTime { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("passwordSalt")]
    public string? PasswordSalt { get; set; }

    [JsonPropertyName("passwordType")]
    public string? PasswordType { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("avatarType")]
    public string? AvatarType { get; set; }

    [JsonPropertyName("permanentAvatar")]
    public string? PermanentAvatar { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("emailVerified")]
    public bool? EmailVerified { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("address")]
    public IEnumerable<string>? Address { get; set; }

    [JsonPropertyName("affiliation")]
    public string? Affiliation { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("idCardType")]
    public string? IdCardType { get; set; }

    [JsonPropertyName("idCard")]
    public string? IdCard { get; set; }

    [JsonPropertyName("homePage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("bio")]
    public string? Bio { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }

    [JsonPropertyName("education")]
    public string? Education { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("karma")]
    public int Karma { get; set; }

    [JsonPropertyName("ranking")]
    public int Ranking { get; set; }

    [JsonPropertyName("isDefaultAvatar")]
    public bool IsDefaultAvatar { get; set; }

    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; set; }

    [JsonPropertyName("isAdmin")]
    public bool IsAdmin { get; set; }

    [JsonPropertyName("isGlobalAdmin")]
    public bool IsGlobalAdmin { get; set; }

    [JsonPropertyName("isForbidden")]
    public bool IsForbidden { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("signupApplication")]
    public string? SignupApplication { get; set; }

    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("preHash")]
    public string? PreHash { get; set; }

    [JsonPropertyName("accessKey")]
    public string? AccessKey { get; set; }

    [JsonPropertyName("accessSecret")]
    public string? AccessSecret { get; set; }

    [JsonPropertyName("createdIp")]
    public string? CreatedIp { get; set; }

    [JsonPropertyName("lastSigninTime")]
    public string? LastSigninTime { get; set; }

    [JsonPropertyName("lastSigninIp")]
    public string? LastSigninIp { get; set; }

    [JsonPropertyName("github")]
    public string? Github { get; set; }

    [JsonPropertyName("google")]
    public string? Google { get; set; }

    [JsonPropertyName("qq")]
    public string? QQ { get; set; }

    [JsonPropertyName("wechat")]
    public string? WeChat { get; set; }

    [JsonPropertyName("facebook")]
    public string? Facebook { get; set; }

    [JsonPropertyName("dingtalk")]
    public string? DingTalk { get; set; }

    [JsonPropertyName("weibo")]
    public string? Weibo { get; set; }

    [JsonPropertyName("gitee")]
    public string? Gitee { get; set; }

    [JsonPropertyName("linkedin")]
    public string? LinkedIn { get; set; }

    [JsonPropertyName("wecom")]
    public string? Wecom { get; set; }

    [JsonPropertyName("lark")]
    public string? Lark { get; set; }

    [JsonPropertyName("gitlab")]
    public string? Gitlab { get; set; }

    [JsonPropertyName("adfs")]
    public string? Adfs { get; set; }

    [JsonPropertyName("baidu")]
    public string? Baidu { get; set; }

    [JsonPropertyName("alipay")]
    public string? Alipay { get; set; }

    [JsonPropertyName("casdoor")]
    public string? Casdoor { get; set; }

    [JsonPropertyName("infoflow")]
    public string? Infoflow { get; set; }

    [JsonPropertyName("apple")]
    public string? Apple { get; set; }

    [JsonPropertyName("azuread")]
    public string? AzureAD { get; set; }

    [JsonPropertyName("slack")]
    public string? Slack { get; set; }

    [JsonPropertyName("steam")]
    public string? Steam { get; set; }

    [JsonPropertyName("bilibili")]
    public string? Bilibili { get; set; }

    [JsonPropertyName("okta")]
    public string? Okta { get; set; }

    [JsonPropertyName("douyin")]
    public string? Douyin { get; set; }

    [JsonPropertyName("line")]
    public string? Line { get; set; }

    [JsonPropertyName("amazon")]
    public string? Amazon { get; set; }

    [JsonPropertyName("auth0")]
    public string? Auth0 { get; set; }

    [JsonPropertyName("battlenet")]
    public string? BattleNet { get; set; }

    [JsonPropertyName("bitbucket")]
    public string? Bitbucket { get; set; }

    [JsonPropertyName("box")]
    public string? Box { get; set; }

    [JsonPropertyName("cloudfoundry")]
    public string? CloudFoundry { get; set; }

    [JsonPropertyName("dailymotion")]
    public string? Dailymotion { get; set; }

    [JsonPropertyName("deezer")]
    public string? Deezer { get; set; }

    [JsonPropertyName("digitalocean")]
    public string? DigitalOcean { get; set; }

    [JsonPropertyName("discord")]
    public string? Discord { get; set; }

    [JsonPropertyName("dropbox")]
    public string? Dropbox { get; set; }

    [JsonPropertyName("eveonline")]
    public string? EveOnline { get; set; }

    [JsonPropertyName("fitbit")]
    public string? Fitbit { get; set; }

    [JsonPropertyName("gitea")]
    public string? Gitea { get; set; }

    [JsonPropertyName("heroku")]
    public string? Heroku { get; set; }

    [JsonPropertyName("influxcloud")]
    public string? InfluxCloud { get; set; }

    [JsonPropertyName("instagram")]
    public string? Instagram { get; set; }

    [JsonPropertyName("intercom")]
    public string? Intercom { get; set; }

    [JsonPropertyName("kakao")]
    public string? Kakao { get; set; }

    [JsonPropertyName("lastfm")]
    public string? Lastfm { get; set; }

    [JsonPropertyName("mailru")]
    public string? Mailru { get; set; }

    [JsonPropertyName("meetup")]
    public string? Meetup { get; set; }

    [JsonPropertyName("microsoftonline")]
    public string? MicrosoftOnline { get; set; }

    [JsonPropertyName("naver")]
    public string? Naver { get; set; }

    [JsonPropertyName("nextcloud")]
    public string? Nextcloud { get; set; }

    [JsonPropertyName("onedrive")]
    public string? OneDrive { get; set; }

    [JsonPropertyName("oura")]
    public string? Oura { get; set; }

    [JsonPropertyName("patreon")]
    public string? Patreon { get; set; }

    [JsonPropertyName("paypal")]
    public string? Paypal { get; set; }

    [JsonPropertyName("salesforce")]
    public string? SalesForce { get; set; }

    [JsonPropertyName("shopify")]
    public string? Shopify { get; set; }

    [JsonPropertyName("soundcloud")]
    public string? Soundcloud { get; set; }

    [JsonPropertyName("spotify")]
    public string? Spotify { get; set; }

    [JsonPropertyName("strava")]
    public string? Strava { get; set; }

    [JsonPropertyName("stripe")]
    public string? Stripe { get; set; }

    [JsonPropertyName("tiktok")]
    public string? TikTok { get; set; }

    [JsonPropertyName("tumblr")]
    public string? Tumblr { get; set; }

    [JsonPropertyName("twitch")]
    public string? Twitch { get; set; }

    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }

    [JsonPropertyName("typetalk")]
    public string? Typetalk { get; set; }

    [JsonPropertyName("uber")]
    public string? Uber { get; set; }

    [JsonPropertyName("vk")]
    public string? VK { get; set; }

    [JsonPropertyName("wepay")]
    public string? Wepay { get; set; }

    [JsonPropertyName("xero")]
    public string? Xero { get; set; }

    [JsonPropertyName("yahoo")]
    public string? Yahoo { get; set; }

    [JsonPropertyName("yammer")]
    public string? Yammer { get; set; }

    [JsonPropertyName("yandex")]
    public string? Yandex { get; set; }

    [JsonPropertyName("zoom")]
    public string? Zoom { get; set; }

    [JsonPropertyName("custom")]
    public string? Custom { get; set; }

    [JsonPropertyName("preferredMfaType")]
    public string? PreferredMfaType { get; set; }

    [JsonPropertyName("recoveryCodes")]
    public string[]? RecoveryCodes { get; set; }

    [JsonPropertyName("totpSecret")]
    public string? TotpSecret { get; set; }

    [JsonPropertyName("mfaPhoneEnabled")]
    public bool? MfaPhoneEnabled { get; set; }

    [JsonPropertyName("mfaEmailEnabled")]
    public bool? MfaEmailEnabled { get; set; }

    [JsonPropertyName("multiFactorAuths")]
    public IEnumerable<CasdoorMfaProps>? MultiFactorAuths { get; set; }

    [JsonPropertyName("ldap")]
    public string? Ldap { get; set; }

    [JsonPropertyName("properties")]
    public IDictionary<string, string>? Properties { get; set; }

    [JsonPropertyName("roles")]
    public IEnumerable<CasdoorRole>? Roles { get; set; }

    [JsonPropertyName("permissions")]
    public IEnumerable<CasdoorPermission>? Permissions { get; set; }

    [JsonPropertyName("groups")]
    public string[]? Groups { get; set; }

    [JsonPropertyName("lastSigninWrongTime")]
    public string? LastSigninWrongTime { get; set; }

    [JsonPropertyName("signinWrongTimes")]
    public int? SigninWrongTimes { get; set; }

    [JsonPropertyName("managedAccounts")]
    public IEnumerable<CasdoorManagedAccount>? ManagedAccounts { get; set; }

    [JsonPropertyName("deletedTime")]
    public string? DeletedTime { get; set; }

    [JsonPropertyName("externalId")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("addresses")]
    public IEnumerable<object>? Addresses { get; set; }

    [JsonPropertyName("realName")]
    public string? RealName { get; set; }

    [JsonPropertyName("isVerified")]
    public bool IsVerified { get; set; }

    [JsonPropertyName("balance")]
    public double Balance { get; set; }

    [JsonPropertyName("balanceCredit")]
    public double BalanceCredit { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("balanceCurrency")]
    public string? BalanceCurrency { get; set; }

    [JsonPropertyName("registerType")]
    public string? RegisterType { get; set; }

    [JsonPropertyName("registerSource")]
    public string? RegisterSource { get; set; }

    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("originalToken")]
    public string? OriginalToken { get; set; }

    [JsonPropertyName("originalRefreshToken")]
    public string? OriginalRefreshToken { get; set; }

    [JsonPropertyName("azureadb2c")]
    public string? Azureadb2c { get; set; }

    [JsonPropertyName("kwai")]
    public string? Kwai { get; set; }

    [JsonPropertyName("telegram")]
    public string? Telegram { get; set; }

    [JsonPropertyName("metamask")]
    public string? Metamask { get; set; }

    [JsonPropertyName("web3onboard")]
    public string? Web3onboard { get; set; }

    [JsonPropertyName("oidc")]
    public string? Oidc { get; set; }

    [JsonPropertyName("custom2")]
    public string? Custom2 { get; set; }

    [JsonPropertyName("custom3")]
    public string? Custom3 { get; set; }

    [JsonPropertyName("custom4")]
    public string? Custom4 { get; set; }

    [JsonPropertyName("custom5")]
    public string? Custom5 { get; set; }

    [JsonPropertyName("custom6")]
    public string? Custom6 { get; set; }

    [JsonPropertyName("custom7")]
    public string? Custom7 { get; set; }

    [JsonPropertyName("custom8")]
    public string? Custom8 { get; set; }

    [JsonPropertyName("custom9")]
    public string? Custom9 { get; set; }

    [JsonPropertyName("custom10")]
    public string? Custom10 { get; set; }

    [JsonPropertyName("webauthnCredentials")]
    public object? WebauthnCredentials { get; set; }

    [JsonPropertyName("mfaRadiusEnabled")]
    public bool MfaRadiusEnabled { get; set; }

    [JsonPropertyName("mfaRadiusUsername")]
    public string? MfaRadiusUsername { get; set; }

    [JsonPropertyName("mfaRadiusProvider")]
    public string? MfaRadiusProvider { get; set; }

    [JsonPropertyName("mfaPushEnabled")]
    public bool MfaPushEnabled { get; set; }

    [JsonPropertyName("mfaPushReceiver")]
    public string? MfaPushReceiver { get; set; }

    [JsonPropertyName("mfaPushProvider")]
    public string? MfaPushProvider { get; set; }

    [JsonPropertyName("invitation")]
    public string? Invitation { get; set; }

    [JsonPropertyName("invitationCode")]
    public string? InvitationCode { get; set; }

    [JsonPropertyName("faceIds")]
    public IEnumerable<object>? FaceIds { get; set; }

    [JsonPropertyName("cart")]
    public IEnumerable<CasdoorProductInfo>? Cart { get; set; }

    [JsonPropertyName("uidNumber")]
    public int UidNumber { get; set; }

    [JsonPropertyName("thirdPartyLinks")]
    public IEnumerable<object>? ThirdPartyLinks { get; set; }

    [JsonPropertyName("lastChangePasswordTime")]
    public string? LastChangePasswordTime { get; set; }

    [JsonPropertyName("mfaAccounts")]
    public IEnumerable<object>? MfaAccounts { get; set; }

    [JsonPropertyName("mfaItems")]
    public IEnumerable<CasdoorMfaItem>? MfaItems { get; set; }

    [JsonPropertyName("mfaRememberDeadline")]
    public string? MfaRememberDeadline { get; set; }

    [JsonPropertyName("needUpdatePassword")]
    public bool NeedUpdatePassword { get; set; }

    [JsonPropertyName("ipWhitelist")]
    public string? IpWhitelist { get; set; }

    [JsonPropertyName("applicationScopes")]
    public IEnumerable<object>? ApplicationScopes { get; set; }
}
