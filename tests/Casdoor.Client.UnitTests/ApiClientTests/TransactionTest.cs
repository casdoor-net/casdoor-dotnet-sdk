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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Casdoor.Client.UnitTests.Fixtures;
using Casdoor.Client.UnitTests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Casdoor.Client.UnitTests.ApiClientTests;

public class TransactionTest : IClassFixture<ServicesFixture>
{
    private readonly ICasdoorClient _client;

    public TransactionTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    [Fact]
    public async Task TestTransaction()
    {
        // a recharge of the organization doesn't need a user balance, so it can be added in CI
        var transaction = new CasdoorTransaction
        {
            Owner = TestConfig.OrganizationName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Application = "app-casbin",
            Domain = "https://casdoor.ai",
            Category = "Recharge",
            Type = "Recharge",
            Tag = "Organization",
            Amount = 100,
            Currency = "USD",
            State = "Paid"
        };

        Assert.Equal("ok", (await _client.AddTransactionWithDryRunAsync(transaction, true))?.Status);

        var response = await _client.AddTransactionAsync(transaction);
        Assert.Equal("ok", response?.Status);
        string name = response!.Data!.ToString()!;

        Assert.Contains(await _client.GetTransactionsAsync() ?? Enumerable.Empty<CasdoorTransaction>(), item => item.Name == name);
        Assert.True((await _client.GetPaginationTransactionsAsync(1, 10)).totalCount > 0);

        var retrieved = await _client.GetTransactionAsync(name);
        Assert.Equal(name, retrieved?.Name);

        retrieved!.DisplayName = "Updated Transaction";
        Assert.Equal("Affected", (await _client.UpdateTransactionAsync(retrieved))?.Data?.ToString());
        Assert.Equal("Updated Transaction", (await _client.GetTransactionAsync(name))?.DisplayName);

        Assert.Equal("Affected", (await _client.DeleteTransactionAsync(retrieved))?.Data?.ToString());
        Assert.Null(await _client.GetTransactionAsync(name));
    }
}
