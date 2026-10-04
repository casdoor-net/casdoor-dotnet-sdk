// Copyright 2026 The Casdoor Authors. All Rights Reserved.
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

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class RecordTest : IClassFixture<ServicesFixture>
{
    private readonly ICasdoorClient _client;

    public RecordTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    [Fact]
    public async Task TestRecord()
    {
        string name = TestUtils.GetRandomName("Record");

        // Add a new object
        var record = new CasdoorRecord
        {
            Owner = TestConfig.OrganizationName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Organization = TestConfig.OrganizationName,
            User = "admin",
            Action = "test-record"
        };
        Assert.Equal("ok", (await _client.AddRecordAsync(record))?.Status);

        // Reading the records needs the access token of an admin user
        var token = await _client.GetOAuthTokenByPasswordAsync("admin", "123");
        var adminClient = _client.WithAccessToken(token.AccessToken!);

        // Get all objects, check if our added object is inside the list
        Assert.Contains(await adminClient.GetRecordsAsync() ?? Enumerable.Empty<CasdoorRecord>(), item => item.Name == name);

        // Get the object
        Assert.Equal(name, (await adminClient.GetRecordAsync(name))?.Name);

        // Get an object that doesn't exist
        Assert.Null(await adminClient.GetRecordAsync(name + "_missing"));
    }
}
