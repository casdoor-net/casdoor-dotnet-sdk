using System.Net.Http;
using Casdoor.Client;
using ConsoleApp;
using Microsoft.IdentityModel.Logging;

var httpClient = new HttpClient();
var options = new CasdoorOptions
{
    // Require: Basic options
    Endpoint = "https://door.casdoor.com",
    OrganizationName = "casbin",
    ApplicationName = "app-example",
    ApplicationType = "native", // webapp, webapi or native
    ClientId = "b800a86702dd4d29ec4d",
    ClientSecret = "1219843a8db4695155699be3a67f10796f2ec1d5",

    // Optional: The callback path that the client will be redirected to
    // after the user has authenticated. default is "/casdoor/signin-callback"
    CallbackPath = "/callback",
    // Optional: Whether require https for casdoor endpoint
    RequireHttpsMetadata = false,
    // Optional: The scopes that the client is requesting.
    Scope = "openid profile email"

    // More options can be found at README.md
    // https://github.com/casdoor/casdoor-dotnet-sdk/blob/master/README.md
};

var client = new CasdoorClient(httpClient, options);

// If you want look PII in logs or exception, you can set the following
IdentityModelEventSource.ShowPII = true;

var configuration = await options.GetOpenIdConnectConfigurationAsync();
ConsoleExtension.WriteLine("Auto fetching OpenIdConnectConfiguration...");
ConsoleExtension.JsonWriteLine(new
{
    configuration.Issuer,
    configuration.JwksUri,
    configuration.TokenEndpoint,
    configuration.AuthorizationEndpoint,
    configuration.UserInfoEndpoint,
}, ConsoleColor.DarkBlue);

var token = await client.RequestPasswordTokenAsync("admin", "123");
ConsoleExtension.WriteLine("Get tokens by username and password...");
if (token?.AccessToken is null)
{
    Console.WriteLine("Failed to get the token.");
    return;
}
client.SetBearerToken(token.AccessToken);
Console.WriteLine($"token : {token.AccessToken}");

var user = client.ParseJwtToken(token.AccessToken);

var apps = await client.GetApplicationsAsync("admin");
ConsoleExtension.JsonWriteLine(apps, ConsoleColor.Blue);

user = await client.GetUserAsync("admin");
if (user is null)
{
    Console.WriteLine("Failed to get the user.");
    return;
}
var res = await client.SetPasswordAsync(user, "123", "123"); // Switch to your own account and modify the password.
Console.WriteLine($"The status of password setting: {res?.Status}");

if (!token.IsError)
{
    ConsoleExtension.JsonWriteLine(new
    {
        token.AccessToken,
        token.RefreshToken,
        token.IdentityToken,
        token.Scope,
        token.ExpiresIn,
        token.TokenType,
    }, ConsoleColor.DarkGreen);
}
else
{
    ConsoleExtension.WriteLine(token.Error ?? "(error)", ConsoleColor.Red);
}
