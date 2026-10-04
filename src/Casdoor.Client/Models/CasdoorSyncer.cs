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

public class TableColumnsItem
{
    [JsonPropertyName("casdoorName")]
    public string? CasdoorName { get; set; }

    [JsonPropertyName("isHashed")]
    public bool? IsHashed { get; set; }

    [JsonPropertyName("isKey")]
    public bool? IsKey { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("values")]
    public IEnumerable<string>? Values { get; set; }
}

public class CasdoorSyncer
{
    [JsonPropertyName("affiliationTable")]
    public string? AffiliationTable { get; set; }

    [JsonPropertyName("avatarBaseUrl")]
    public string? AvatarBaseUrl { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("database")]
    public string? Database { get; set; }

    [JsonPropertyName("databaseType")]
    public string? DatabaseType { get; set; }

    [JsonPropertyName("errorText")]
    public string? ErrorText { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("isEnabled")]
    public bool? IsEnabled { get; set; }

    [JsonPropertyName("isReadOnly")]
    public bool? IsReadOnly { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonPropertyName("sslMode")]
    public string? SslMode { get; set; }

    [JsonPropertyName("syncInterval")]
    public int SyncInterval { get; set; }

    [JsonPropertyName("table")]
    public string? Table { get; set; }

    [JsonPropertyName("tableColumns")]
    public IEnumerable<TableColumnsItem>? TableColumns { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("sshType")]
    public string? SshType { get; set; }

    [JsonPropertyName("sshHost")]
    public string? SshHost { get; set; }

    [JsonPropertyName("sshPort")]
    public int SshPort { get; set; }

    [JsonPropertyName("sshUser")]
    public string? SshUser { get; set; }

    [JsonPropertyName("sshPassword")]
    public string? SshPassword { get; set; }

    [JsonPropertyName("cert")]
    public string? Cert { get; set; }
}
