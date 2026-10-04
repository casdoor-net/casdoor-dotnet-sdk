# Casdoor .NET SDK

<p align="center">
  <a href="#badge">
    <img alt="semantic-release" src="https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg">
  </a>
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/build.yml">
    <img alt="Build" src="https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/build.yml/badge.svg">
  </a>
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/release.yml">
    <img alt="Release" src="https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/release.yml/badge.svg">
  </a>
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/releases/latest">
    <img alt="GitHub Release" src="https://img.shields.io/github/v/release/casdoor-net/casdoor-dotnet-sdk">
  </a>
  <a href="https://www.nuget.org/packages/Casdoor.Client">
    <img alt="NuGet Version" src="https://img.shields.io/nuget/v/Casdoor.Client?logo=nuget">
  </a>
  <a href="https://www.nuget.org/packages/Casdoor.Client">
    <img alt="NuGet Downloads" src="https://img.shields.io/nuget/dt/Casdoor.Client?logo=nuget">
  </a>
</p>

<p align="center">
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/blob/master/LICENSE">
    <img src="https://img.shields.io/github/license/casdoor-net/casdoor-dotnet-sdk?style=flat-square" alt="license">
  </a>
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/issues">
    <img alt="GitHub issues" src="https://img.shields.io/github/issues/casdoor-net/casdoor-dotnet-sdk?style=flat-square">
  </a>
  <a href="#">
    <img alt="GitHub stars" src="https://img.shields.io/github/stars/casdoor-net/casdoor-dotnet-sdk?style=flat-square">
  </a>
  <a href="https://github.com/casdoor-net/casdoor-dotnet-sdk/network">
    <img alt="GitHub forks" src="https://img.shields.io/github/forks/casdoor-net/casdoor-dotnet-sdk?style=flat-square">
  </a>
  <a href="https://discord.gg/5rPsrAzK7S">
    <img alt="Casdoor" src="https://img.shields.io/discord/1022748306096537660?style=flat-square&logo=discord&label=discord&color=5865F2">
  </a>
</p>

Casdoor .NET SDK is the official .NET client library for [Casdoor](https://casdoor.ai/). It lets your .NET and ASP.NET Core applications sign users in with Casdoor (OAuth 2.0 / OIDC), verify the JWT tokens issued by Casdoor, and manage users, organizations, applications, roles, permissions and all the other Casdoor objects through the Casdoor APIs.

The SDK has the same features as [casdoor-go-sdk](https://github.com/casdoor/casdoor-go-sdk).

## 📋 Table of Contents

- [Packages](#-packages)
- [Features](#-features)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [Configuration](#️-configuration)
- [Authentication](#-authentication)
- [Resource Management](#-resource-management)
- [API Reference](#-api-reference)
- [ASP.NET Core](#-aspnet-core)
- [Development](#-development)
- [Documentation](#-documentation)
- [License](#-license)

## 📦 Packages

| Package Name         | NuGet                                                                                                               | Description          | Supported frameworks                   |
|----------------------|---------------------------------------------------------------------------------------------------------------------|----------------------|----------------------------------------|
| `Casdoor.Client`     | [![NuGet](https://img.shields.io/nuget/vpre/Casdoor.Client)](https://www.nuget.org/packages/Casdoor.Client)         | SDK for .NET         | .NET Standard 2.0/.NET 4.6.1 and newer |
| `Casdoor.AspNetCore` | [![NuGet](https://img.shields.io/nuget/vpre/Casdoor.AspNetCore)](https://www.nuget.org/packages/Casdoor.AspNetCore) | SDK for ASP.NET Core | .NET Core 3.1 and newer                |

## ✨ Features

- **OAuth 2.0 Authentication**: authorization code, password, client credentials and refresh token grants, token introspection, SSO logout
- **JWT Verification**: verify the tokens signed by Casdoor
- **Calling APIs as the User**: `WithAccessToken()` calls the APIs with the user's own permissions
- **User Management**: CRUD, lookup by email / phone / user ID, pagination, password check and change
- **Organization & Application Management**: organizations, applications, groups, certificates, providers, LDAP
- **Authorization**: roles, permissions, models, adapters, enforcers, policies, `EnforceAsync()` and `BatchEnforceAsync()`
- **Billing**: products, orders, payments, plans, pricings, subscriptions and transactions
- **Messaging**: send emails, SMS and notifications
- **Multi-Factor Authentication (MFA)**: TOTP, email and SMS MFA setup
- **Other Objects**: sessions, tokens, webhooks, syncers, invitations, resources (file upload) and records
- **Dependency Injection**: `services.AddCasdoorClient()` and the ASP.NET Core authentication handler

## 📥 Installation

```bash
dotnet add package Casdoor.Client
# for ASP.NET Core authentication
dotnet add package Casdoor.AspNetCore
```

## 🚀 Quick Start

```csharp
using Casdoor.Client;

var client = new CasdoorClient(new HttpClient(), new CasdoorOptions
{
    Endpoint = "http://localhost:8000",
    OrganizationName = "my-organization",
    ApplicationName = "my-application",
    ApplicationType = "webapp",   // webapp, webapi or native
    ClientId = "<client-id>",
    ClientSecret = "<client-secret>",
});

var users = await client.GetUsersAsync();
Console.WriteLine($"Found {users?.Count()} users");
```

Or register it with dependency injection and inject `ICasdoorClient`:

```csharp
services.AddCasdoorClient(options =>
{
    options.Endpoint = "http://localhost:8000";
    options.OrganizationName = "my-organization";
    options.ApplicationName = "my-application";
    options.ClientId = "<client-id>";
    options.ClientSecret = "<client-secret>";
});
```

## ⚙️ Configuration

### Options

| Option           | Required | Description                                                                      |
|------------------|----------|----------------------------------------------------------------------------------|
| Endpoint         | Yes      | Casdoor server URL, such as `http://localhost:8000`                              |
| ClientId         | Yes      | Client ID of the Casdoor application                                             |
| ClientSecret     | Yes      | Client secret of the Casdoor application                                         |
| OrganizationName | Yes      | Name of the Casdoor organization                                                 |
| ApplicationName  | Yes      | Name of the Casdoor application                                                  |
| ApplicationType  | No       | `webapp`, `webapi` or `native`                                                   |
| CustomHeaders    | No       | HTTP headers added to all the API requests, e.g. `["Accept-Language"] = "de"`   |
| CallbackPath     | No       | The callback path of the OAuth flow, defaults to `/casdoor/signin-callback`      |
| Scope            | No       | The OAuth scopes, defaults to `openid profile email`                             |

### Getting the Configuration from Casdoor

1. **Endpoint**: the URL of your Casdoor server
2. **ClientId** and **ClientSecret**: the application's edit page in the Casdoor admin panel
3. **OrganizationName**: the organization that owns your users
4. **ApplicationName**: the name of your application

### Custom HTTP Headers

```csharp
options.CustomHeaders["Accept-Language"] = "de";   // localized error messages
options.CustomHeaders["X-Trace-ID"] = "trace-abc-123";
```

The `Authorization` header is set by the SDK on each request, the shared `HttpClient` is never changed.

### Responses and Errors

- `GetXxxsAsync()` and `GetXxxAsync()` return the objects (`CasdoorUser`, `CasdoorRole`, ...), `GetXxxAsync()` returns `null` when the object doesn't exist
- `GetPaginationXxxsAsync()` returns a tuple of the objects and the total count
- `AddXxxAsync()`, `UpdateXxxAsync()` and `DeleteXxxAsync()` return Casdoor's `CasdoorResponse`, whose `Status` is `"ok"` and `Data` is `"Affected"` when the object is changed
- The `GetXxx` methods throw `CasdoorApiException` with Casdoor's error message when Casdoor returns an error

## 🔐 Authentication

### OAuth 2.0 Flow

```csharp
// 1. Redirect the user to Casdoor
string signinUrl = client.GetSigninUrl("http://localhost:8080/callback");

// 2. Exchange the code of the callback for the tokens
var token = await client.RequestAuthorizationCodeTokenAsync(code, "http://localhost:8080/callback");

// 3. Verify the access token and get the user
CasdoorUser? user = client.ParseJwtToken(token.AccessToken!);
```

### Other Grants, Token Refresh and Introspection

```csharp
// Resource Owner Password Credentials grant, the application must enable the "Password" grant type
var token = await client.GetOAuthTokenByPasswordAsync("alice", "password");

// Sign in as any user of the organization with the organization's master password
var token2 = await client.ImpersonateUserAsync("alice", "<master password>");

// Client Credentials grant: the token belongs to the application instead of a user
var appToken = await client.RequestClientCredentialsTokenAsync();

var refreshed = await client.RefreshOAuthTokenAsync(token.RefreshToken!);

var introspection = await client.IntrospectTokenAsync(token.AccessToken!);
bool active = introspection.IsActive;
```

### Calling APIs With the User's Access Token

By default, the SDK calls the Casdoor APIs as the application itself: it authenticates with the client ID and client secret, so the calls have the application's (admin) permissions.

To call the APIs on behalf of the signed-in user instead, use `WithAccessToken()` with the user's access token. It returns a new client that sends the `Authorization: Bearer <access_token>` header, so Casdoor treats the requests as being made by that user and the user's own permissions apply:

```csharp
var userClient = client.WithAccessToken(token.AccessToken!);

// "Who am I"
var account = await userClient.GetAccountAsync();

// Any other API can be called in the same way
var users = await userClient.GetUsersAsync();
```

The original client is not changed, so it's safe to create one such client per incoming HTTP request. (`SetBearerToken()` still exists, but it changes the client it's called on.)

**Note**: a non-admin user can only access their own data. If an API returns a permission error, the user simply isn't allowed to call it — use the application's client (without `WithAccessToken()`) for admin operations.

### Logout

```csharp
await client.LogoutAsync(accessToken);                // sign the user out of all the applications and devices (SSO logout)
await client.LogoutCurrentSessionAsync(accessToken);  // only sign out the session of this access token
```

## 📦 Resource Management

### Object Owner

Every object in Casdoor is identified by an ID of the form `owner/name`, where the owner is an organization (`role`, `group`, `user`, `product`, ...) or the built-in `admin` owner (`organization`, `application`, `token`).

By default the SDK fills in the owner for you: the `OrganizationName` of the options, or `admin` for the object types listed above. You can address an object in another organization by passing a qualified `owner/name` ID instead of a plain name (or the `owner` parameter), and by setting the `Owner` property explicitly when creating or updating an object:

```csharp
await client.GetRoleAsync("my-role");            // "my-organization/my-role"
await client.GetRoleAsync("other-org/my-role");  // "other-org/my-role"

await client.AddRoleAsync(new CasdoorRole { Owner = "other-org", Name = "my-role" });  // created in "other-org"
```

> [!IMPORTANT]
> **Behavior change:** `AddXxxAsync()` used to overwrite the `Owner` property of the object with the organization of the options, and to ignore any owner set by the caller. It now only fills `Owner` in when it is empty. If your code sets `Owner` to a value other than the organization of the options (for example the literal `"admin"`), the request is now sent to that owner instead of being silently redirected, so clear the property or set it to the intended organization.

### Users

```csharp
await client.GetUsersAsync();
var (users, total) = await client.GetPaginationUsersAsync(1, 10);
await client.GetUserAsync("alice");
await client.GetUserByEmailAsync("alice@example.com");
await client.GetUserByPhoneAsync("2025550123");
await client.GetUserByUserIdAsync("<user id>");
await client.GetSortedUsersAsync("created_time", 10);
await client.GetGlobalUsersAsync();  // users of all organizations

await client.AddUserAsync(user);
await client.UpdateUserForColumnsAsync(user, new[] { "displayName", "email" });
await client.UpdateUserByUserIdAsync("my-organization", "<user id>", user);
await client.DeleteUserAsync("alice");
```

### Permissions, Enforcers and Policies

```csharp
await client.GetPermissionsByRoleAsync("admin");

var enforcer = await client.GetEnforcerAsync("my-enforcer");
await client.GetPoliciesAsync("my-enforcer");
await client.GetFilteredPoliciesAsync("my-organization/my-enforcer",
    new[] { new CasdoorPolicyFilter { Ptype = "p", FieldIndex = 0, FieldValues = new[] { "alice" } } });
await client.AddPolicyAsync(enforcer!, new CasdoorCasbinRule { Ptype = "p", V0 = "alice", V1 = "data1", V2 = "read" });
await client.UpdatePolicyAsync(enforcer!, oldPolicy, newPolicy);
await client.RemovePolicyAsync(enforcer!, policy);
```

### Billing: Products, Orders, Payments and Transactions

```csharp
// Place an order of products for a user and pay it with a payment provider
var order = await client.PlaceOrderAsync(new[] { new CasdoorProductInfo { Name = "my-product", Quantity = 1 } }, "alice");
var payment = await client.PayOrderAsync(order!.Name!, "my-payment-provider");
await client.CancelOrderAsync(order.Name!);

await client.GetUserOrdersAsync("alice");
await client.GetUserPaymentsAsync("alice");
await client.GetUserTransactionsAsync("alice");

// Validate a transaction (e.g. the balance) without saving it
await client.AddTransactionWithDryRunAsync(transaction, true);
```

### Email, SMS and Notifications

```csharp
await client.SendEmailAsync("Hello", "Hello world", "Casdoor", new[] { "alice@example.com" });
await client.SendEmailByProviderAsync("Hello", "Hello world", "Casdoor", "my-email-provider", new[] { "alice@example.com" });
await client.SendSmsAsync("123456", new[] { "+12025550123" });
await client.SendSmsByProviderAsync("123456", "my-sms-provider", new[] { "+12025550123" });
await client.SendNotificationAsync("Hello", "alice");
```

### Multi-Factor Authentication

```csharp
var setup = await client.InitiateMfaAsync("my-organization", "app", "alice");
await client.VerifyMfaAsync("my-organization", "app", "alice", "<secret>", "<passcode>");
await client.EnableMfaAsync("my-organization", "app", "alice", "<secret>", "<recovery code>");
await client.SetPreferredMfaAsync("my-organization", "app", "alice");
await client.DeleteMfaAsync("my-organization", "alice");
```

## 📚 API Reference

| Object           | Methods                                                                                                                                     |
|------------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| **Auth**         | `RequestAuthorizationCodeTokenAsync`, `GetOAuthTokenByPasswordAsync`, `ImpersonateUserAsync`, `RequestClientCredentialsTokenAsync`, `RefreshOAuthTokenAsync`, `IntrospectTokenAsync`, `ParseJwtToken`, `LogoutAsync`, `LogoutCurrentSessionAsync`, `WithAccessToken`, `GetAccountAsync`, `GetSigninUrl` |
| **User**         | CRUD + pagination, `GetUserByEmailAsync`, `GetUserByPhoneAsync`, `GetUserByUserIdAsync`, `GetSortedUsersAsync`, `GetGlobalUsersAsync`, `GetUserCount`, `UpdateUserForColumnsAsync`, `UpdateUserByUserIdAsync`, `CheckUserPasswordAsync`, `SetPasswordAsync` |
| **Organization** | CRUD, `GetOrganizationNamesAsync`                                                                                                           |
| **Application**  | CRUD, `GetOrganizationApplicationsAsync`, `GetUserApplicationAsync`                                                                         |
| **Group / Model / Enforcer / Adapter / Plan / Pricing / Subscription / Syncer / Webhook / Provider / Product / Payment / Session / Record** | CRUD + pagination |
| **Cert**         | CRUD, `GetGlobalCertsAsync`                                                                                                                 |
| **Role**         | CRUD + pagination, `UpdateRoleForColumnsAsync`                                                                                              |
| **Permission**   | CRUD + pagination, `UpdatePermissionForColumnsAsync`, `GetPermissionsByRoleAsync`                                                           |
| **Policy**       | `GetPoliciesAsync`, `GetFilteredPoliciesAsync`, `AddPolicyAsync`, `UpdatePolicyAsync`, `RemovePolicyAsync`                                  |
| **Enforce**      | `EnforceAsync`, `BatchEnforceAsync`                                                                                                         |
| **Token**        | CRUD + pagination, `UpdateTokenForColumnsAsync`                                                                                             |
| **Order**        | CRUD + pagination, `GetUserOrdersAsync`, `PlaceOrderAsync`, `PayOrderAsync`, `CancelOrderAsync`                                             |
| **Payment**      | `GetUserPaymentsAsync`, `NotifyPaymentAsync`, `InvoicePaymentAsync`                                                                         |
| **Transaction**  | CRUD + pagination, `GetUserTransactionsAsync`, `AddTransactionWithDryRunAsync`                                                              |
| **Invitation**   | CRUD + pagination, `UpdateInvitationForColumnsAsync`, `GetInvitationInfoAsync`                                                              |
| **LDAP**         | CRUD, `GetLdapUsersAsync`, `SyncLdapUsersAsync`, `SyncLdapUsersFromServerAsync`                                                             |
| **Resource**     | `GetResourcesAsync`, `GetPaginationResourcesAsync`, `GetResourceAsync`, `GetResourceExAsync`, `AddResourceAsync`, `UpdateResourceAsync`, `UploadResourceAsync`, `DeleteResourceAsync`, `DeleteResourceWithTagAsync` |
| **Email / SMS / Notification** | `SendEmailAsync`, `SendEmailByProviderAsync`, `SendSmsAsync`, `SendSmsByProviderAsync`, `SendNotificationAsync`                |
| **MFA**          | `InitiateMfaAsync`, `VerifyMfaAsync`, `EnableMfaAsync`, `SetPreferredMfaAsync`, `DeleteMfaAsync`                                            |

## 🌐 ASP.NET Core

1. Add the Casdoor settings to your `appsettings.json`:

```json5
{
    "Casdoor": {
        "Endpoint": "<your Casdoor endpoint>",
        "OrganizationName": "<your Casdoor organization>",
        "ApplicationName": "<your Casdoor application>",
        "ApplicationType": "webapp", // webapp or webapi (webapi is not yet supported)
        "ClientId": "<your Casdoor application's client ID>",
        "ClientSecret": "<your Casdoor application's client secret>",

        // Optional: The callback path that the client will be redirected to
        // after the user has authenticated. default is "/casdoor/signin-callback"
        "CallbackPath": "/callback",
        // Optional: Whether require https for casdoor endpoint
        "RequireHttpsMetadata": false,
        // Optional: The scopes that the client will request
        "Scopes": [
            "openid",
            "profile",
            "email",
        ]
    }
}
```

2. Add the Casdoor authentication scheme to your web app:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCasdoor(builder.Configuration.GetSection("Casdoor"))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
```

3. Now you can use the Casdoor authentication scheme in your web app!

### Samples

- [MVC sample](https://github.com/casdoor-net/casdoor-dotnet-sdk/tree/master/samples/MvcApp): an MVC web app that uses Casdoor authentication. The default settings use the public demo Casdoor, start it with `dotnet run`, or change `appsettings.json` to use your own Casdoor.
- More examples: [WinForms](https://github.com/casdoor/casdoor-dotnet-winform-example), [MAUI](https://github.com/casdoor-net/casdoor-dotnet-maui-example), [Avalonia](https://github.com/casdoor-net/casdoor-dotnet-avalonia-example), [WPF](https://github.com/casdoor/casdoor-dotnet-desktop-example)

## 🛠 Development

The tests run against a real Casdoor server. CI starts one with Docker and the data in [.ci/casdoor/init_data.json](.ci/casdoor/init_data.json):

```bash
docker run -d --name casdoor -p 8000:8000 \
  -e driverName=sqlite \
  -e dataSourceName='file:casdoor.db?cache=shared' \
  -e initDataFile=/init_data.json \
  -v "$PWD/.ci/casdoor/init_data.json:/init_data.json:ro" \
  casbin/casdoor-all-in-one

dotnet build
dotnet test
```

Set `CASDOOR_TEST_ENDPOINT`, `CASDOOR_TEST_CLIENT_ID`, `CASDOOR_TEST_CLIENT_SECRET`, `CASDOOR_TEST_ORGANIZATION` and `CASDOOR_TEST_APPLICATION` to run the tests against another server.

Releases are published to NuGet automatically by semantic-release when commits are pushed to `master`.

## 📖 Documentation

- [Casdoor Documentation](https://casdoor.ai/docs/overview)
- [Casdoor .NET SDK Documentation](https://casdoor.ai/docs/how-to-connect/sdk)
- [Casdoor API Documentation](https://door.casdoor.com/swagger)
- [Casdoor GitHub Repository](https://github.com/casdoor/casdoor)

## 📄 License

This project is licensed under the [Apache 2.0 license](LICENSE).
