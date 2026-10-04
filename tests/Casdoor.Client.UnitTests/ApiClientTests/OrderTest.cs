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

public class OrderTest : IClassFixture<ServicesFixture>
{
    private readonly ICasdoorClient _client;

    public OrderTest(ServicesFixture servicesFixture) =>
        _client = servicesFixture.ServiceProvider.GetRequiredService<ICasdoorClient>();

    private async Task<CasdoorProduct> AddProductAsync(string name)
    {
        var product = new CasdoorProduct
        {
            Owner = TestConfig.OrganizationName,
            Name = name,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = name,
            Image = "https://cdn.casbin.org/img/casdoor-logo_1185x256.png",
            Description = "Casdoor Website",
            Tag = "auto_created_product_for_plan",
            Quantity = 999,
            State = "Published",
            Providers = new[] { "provider_payment_dummy" },
            Price = 1,
            Currency = "USD"
        };
        Assert.Equal("ok", (await _client.AddProductAsync(product))?.Status);
        return product;
    }

    [Fact]
    public async Task TestOrder()
    {
        string productName = TestUtils.GetRandomName("OrderProduct");
        string orderName = TestUtils.GetRandomName("Order");
        var product = await AddProductAsync(productName);

        var order = new CasdoorOrder
        {
            Owner = TestConfig.OrganizationName,
            Name = orderName,
            CreatedTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            DisplayName = orderName,
            Products = new[] { productName },
            ProductInfos = new[]
            {
                new CasdoorProductInfo
                {
                    Owner = TestConfig.OrganizationName, Name = productName, DisplayName = productName, Price = 1, Currency = "USD", Quantity = 1
                }
            },
            User = "admin",
            Price = 1,
            Currency = "USD",
            State = "Created"
        };
        Assert.Equal("Affected", (await _client.AddOrderAsync(order))?.Data?.ToString());

        Assert.Contains(await _client.GetOrdersAsync() ?? Enumerable.Empty<CasdoorOrder>(), item => item.Name == orderName);
        Assert.True((await _client.GetPaginationOrdersAsync(1, 10)).totalCount > 0);
        Assert.Contains(await _client.GetUserOrdersAsync("admin") ?? Enumerable.Empty<CasdoorOrder>(), item => item.Name == orderName);

        var retrieved = await _client.GetOrderAsync(orderName);
        Assert.Equal(orderName, retrieved?.Name);

        retrieved!.Message = "Updated order message";
        Assert.Equal("Affected", (await _client.UpdateOrderAsync(retrieved))?.Data?.ToString());
        Assert.Equal("Updated order message", (await _client.GetOrderAsync(orderName))?.Message);

        Assert.Equal("Affected", (await _client.CancelOrderAsync(orderName))?.Data?.ToString());
        Assert.Equal("Affected", (await _client.DeleteOrderAsync(retrieved))?.Data?.ToString());
        Assert.Null(await _client.GetOrderAsync(orderName));

        await _client.DeleteProductAsync(product);
    }

    [Fact]
    public async Task TestOrderPay()
    {
        string productName = TestUtils.GetRandomName("OrderPayProduct");
        var product = await AddProductAsync(productName);

        var order = await _client.PlaceOrderAsync(new[] { new CasdoorProductInfo { Name = productName, Quantity = 1 } }, "admin");
        Assert.False(string.IsNullOrEmpty(order?.Name));

        var payment = await _client.PayOrderAsync(order!.Name!, "provider_payment_dummy");
        Assert.NotNull(payment);

        await _client.DeleteProductAsync(product);
    }
}
