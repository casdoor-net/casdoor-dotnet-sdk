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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Casdoor.Client;
public class CasdoorWebhook
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("databaseType")]
    public string? DatabaseType { get; set; }

    [JsonPropertyName("database")]
    public string? Database { get; set; }

    [JsonPropertyName("table")]
    public string? Table { get; set; }

    [JsonPropertyName("tablePrimaryKey")]
    public string? TablePrimaryKey { get; set; }

    [JsonPropertyName("tableColumns")]
    public List<TableColumnsItem>? TableColumns { get; set; }

    [JsonPropertyName("affiliationTable")]
    public string? AffiliationTable { get; set; }

    [JsonPropertyName("avatarBaseUrl")]
    public string? AvatarBaseUrl { get; set; }

    [JsonPropertyName("errorText")]
    public string? ErrorText { get; set; }

    [JsonPropertyName("syncInterval")]
    public int SyncInterval { get; set; }

    [JsonPropertyName("isReadOnly")]
    public bool IsReadOnly { get; set; }

    [JsonPropertyName("isEnabled")]
    public bool IsEnabled { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("contentType")]
    public string? ContentType { get; set; }

    [JsonPropertyName("headers")]
    public IEnumerable<object>? Headers { get; set; }

    [JsonPropertyName("events")]
    public IEnumerable<string>? Events { get; set; }

    [JsonPropertyName("tokenFields")]
    public IEnumerable<string>? TokenFields { get; set; }

    [JsonPropertyName("objectFields")]
    public IEnumerable<string>? ObjectFields { get; set; }

    [JsonPropertyName("isUserExtended")]
    public bool IsUserExtended { get; set; }

    [JsonPropertyName("singleOrgOnly")]
    public bool SingleOrgOnly { get; set; }

    [JsonPropertyName("maxRetries")]
    public int MaxRetries { get; set; }

    [JsonPropertyName("retryInterval")]
    public int RetryInterval { get; set; }

    [JsonPropertyName("useExponentialBackoff")]
    public bool UseExponentialBackoff { get; set; }
}
