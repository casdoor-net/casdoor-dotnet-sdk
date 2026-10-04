// Copyright 2024 The Casdoor Authors. All Rights Reserved.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casdoor.Client;

using System.Text.Json.Serialization;

public class CasdoorPayment
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    // Payment Provider Info
    [JsonPropertyName("provider")]
    public string? Provider { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    // Product Info
    [JsonPropertyName("productName")]
    public string? ProductName { get; set; }

    [JsonPropertyName("productDisplayName")]
    public string? ProductDisplayName { get; set; }

    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("returnUrl")]
    public string? ReturnUrl { get; set; }

    // Payer Info
    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("personName")]
    public string? PersonName { get; set; }

    [JsonPropertyName("personIdCard")]
    public string? PersonIdCard { get; set; }

    [JsonPropertyName("personEmail")]
    public string? PersonEmail { get; set; }

    [JsonPropertyName("personPhone")]
    public string? PersonPhone { get; set; }

    [JsonPropertyName("invoiceType")]
    public string? InvoiceType { get; set; }

    [JsonPropertyName("invoiceTitle")]
    public string? InvoiceTitle { get; set; }

    [JsonPropertyName("invoiceTaxId")]
    public string? InvoiceTaxId { get; set; }

    [JsonPropertyName("invoiceRemark")]
    public string? InvoiceRemark { get; set; }

    [JsonPropertyName("invoiceUrl")]
    public string? InvoiceUrl { get; set; }

    [JsonPropertyName("outOrderId")]
    public string? OutOrderId { get; set; }

    [JsonPropertyName("payUrl")]
    public string? PayUrl { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("products")]
    public IEnumerable<string>? Products { get; set; }

    [JsonPropertyName("productsDisplayName")]
    public string? ProductsDisplayName { get; set; }

    [JsonPropertyName("order")]
    public string? Order { get; set; }

    [JsonPropertyName("orderObj")]
    public CasdoorOrder? OrderObj { get; set; }

    [JsonPropertyName("successUrl")]
    public string? SuccessUrl { get; set; }
}
