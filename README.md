# Casdoor .NET SDK

[![Build](https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/build.yml/badge.svg)](https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/build.yml)
[![Release](https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/release.yml/badge.svg)](https://github.com/casdoor-net/casdoor-dotnet-sdk/actions/workflows/release.yml)
[![GitHub Release](https://img.shields.io/github/v/release/casdoor-net/casdoor-dotnet-sdk)](https://github.com/casdoor-net/casdoor-dotnet-sdk/releases/latest)
[![NuGet Version](https://img.shields.io/nuget/v/Casdoor.Client?logo=nuget)](https://www.nuget.org/packages/Casdoor.Client)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Casdoor.Client?logo=nuget)](https://www.nuget.org/packages/Casdoor.Client)
[![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg)](https://github.com/semantic-release/semantic-release)
[![License](https://img.shields.io/github/license/casdoor-net/casdoor-dotnet-sdk)](https://github.com/casdoor-net/casdoor-dotnet-sdk/blob/master/LICENSE)
[![Discord](https://img.shields.io/discord/1022748306096537660?logo=discord&label=discord&color=5865F2)](https://discord.gg/5rPsrAzK7S)

The Casdoor's SDK for .NET/ASP.NET Core, which will allow you to easily connect your application to the Casdoor authentication system without having to implement it from scratch.

## Packages

This SDK is built using the following packages for different platforms:

| Package Name         | NuGet                                                                                                               | Description          | Supported frameworks                   |
|----------------------|---------------------------------------------------------------------------------------------------------------------|----------------------|----------------------------------------|
| `Casdoor.Client`     | [![NuGet](https://img.shields.io/nuget/vpre/Casdoor.Client)](https://www.nuget.org/packages/Casdoor.Client)         | SDK for .NET         | .NET Standard 2.0/.NET 4.6.1 and newer |
| `Casdoor.AspNetCore` | [![NuGet](https://img.shields.io/nuget/vpre/Casdoor.AspNetCore)](https://www.nuget.org/packages/Casdoor.AspNetCore) | SDK for ASP.NET Core | .NET Core 3.1 and newer                |
| `Casdoor.Native`     | `wait publish`                                                                                                      | SDK for WPF or Maui  | -                                      |

## Casdoor Client

Casdoor.Client is a base package for the specific platform SDKs. It contains follow features.

- CasdoorClient: A API client implementation for the Casdoor. You can use to call the Casdoor APIs.

##### Usage

```cs
var httpClient = new HttpClient();
var client = new CasdoorClient(HttpClient, new CasdoorOptions{
    Endpoint = "https://door.casdoor.com",
    OrganizationName = "casbin", // your Casdoor organization
    ApplicationName = "app-example", // your Casdoor application
    ApplicationType = "native", // webapp, webapi or native
    ClientId = "b800a86702dd4d29ec4d", // your Casdoor application's client ID
    ClientSecret = "1219843a8db4695155699be3a67f10796f2ec1d5", // your Casdoor application's client secret
});

// Request tokens (You should enable credentials flow in your Casdoor application)
var token = await client.RequestClientCredentialsTokenAsync();
client.SetBearerToken(token);

var currentUser = client.ParseJwtToken(token.AccessToken);

// Request user info
var users = await client.GetUsersAsync();
// Request roles
var roles = await client.GetRolesAsync();
// Request Permissions
var persmissions = await client.GetPermissionsAsync();

var policy = new CasdoorPermissionRule()
{
    Id = $"{currentUser.Owner}/{persmissions.First().Name}",
    V0 = $"{currentUser.Owner}/{currentUser.Name}",
    V1 = "example-resource",
    V2 = "example-action"
};

var isPermissionAvaliable = await client.EnforceAsync(policy);

```
- CasdoorOptions (Updating)
- Extensions (Updating)

## Casdoor AspNetCore

### Getting started

1. Add the following settings about casdoor in your appsettings.json file:

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

2. Add the following code to your webapp:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCasdoor(builder.Configuration.GetSection("Casdoor"))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
```

3. Now you can use the Casdoor authentication scheme in your webapp!

### Samples

#### 1. [MVC sample](https://github.com/casdoor/casdoor-dotnet-sdk/tree/master/samples/MvcApp)

It is a MVC webapp that uses Casdoor authentication.
The default settings use the public demo Casdoor and Casnode configuration, you can directly start the webapp by running:

```bash
dotnet run
```

Or change the settings in the appsettings.json according to your deployed casdoor configuration.[README.md](README.md)

## License

This project is licensed under the [Apache 2.0 license](LICENSE).
