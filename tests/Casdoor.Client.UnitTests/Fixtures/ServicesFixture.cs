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

using Microsoft.Extensions.DependencyInjection;

namespace Casdoor.Client.UnitTests.Fixtures;

internal static class TestConfig
{
    public static readonly string Endpoint = GetEnv("CASDOOR_TEST_ENDPOINT", "http://localhost:8000");
    public static readonly string ClientId = GetEnv("CASDOOR_TEST_CLIENT_ID", "casdoor-dotnet-sdk-ci-client");
    public static readonly string ClientSecret = GetEnv("CASDOOR_TEST_CLIENT_SECRET", "casdoor-dotnet-sdk-ci-secret");
    public static readonly string OrganizationName = GetEnv("CASDOOR_TEST_ORGANIZATION", "casbin");
    public static readonly string ApplicationName = GetEnv("CASDOOR_TEST_APPLICATION", "app-example");

    private static string GetEnv(string key, string defaultValue)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }
}

public class ServicesFixture
{
    public ServicesFixture()
    {
        ServiceProvider = new ServiceCollection()
            .AddCasdoorClient(options =>
            {
                options.Endpoint = TestConfig.Endpoint;
                options.OrganizationName = TestConfig.OrganizationName;
                options.ApplicationName = TestConfig.ApplicationName;
                options.ClientId = TestConfig.ClientId;
                options.ClientSecret = TestConfig.ClientSecret;
                options.ApplicationType = "webapp";
            }).BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; set; }
}

public class ServicesFixtureWithoutSecret
{
    public ServicesFixtureWithoutSecret()
    {
        ServiceProvider = new ServiceCollection()
            .AddCasdoorClient(options =>
            {
                options.Endpoint = TestConfig.Endpoint;
                options.OrganizationName = TestConfig.OrganizationName;
                options.ApplicationName = TestConfig.ApplicationName;
                options.ClientId = TestConfig.ClientId;
                options.ApplicationType = "webapp";
            }).BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; set; }
}
