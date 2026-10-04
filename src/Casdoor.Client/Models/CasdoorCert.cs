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
using System.Text.Json.Serialization;

namespace Casdoor.Client;

public class CasdoorCert
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("cryptoAlgorithm")]
    public string? CryptoAlgorithm { get; set; }

    [JsonPropertyName("bitSize")]
    public int BitSize { get; set; }

    [JsonPropertyName("expireInYears")]
    public int ExpireInYears { get; set; }

    [JsonPropertyName("certificate")]
    public string? Certificate { get; set; }

    [JsonPropertyName("privateKey")]
    public string? PrivateKey { get; set; }

    [JsonPropertyName("authorityPublicKey")]
    public string? AuthorityPublicKey { get; set; }

    [JsonPropertyName("authorityRootPublicKey")]
    public string? AuthorityRootPublicKey { get; set; }

    [JsonPropertyName("expireTime")]
    public string? ExpireTime { get; set; }

    [JsonPropertyName("domainExpireTime")]
    public string? DomainExpireTime { get; set; }

    [JsonPropertyName("provider")]
    public string? Provider { get; set; }

    [JsonPropertyName("account")]
    public string? Account { get; set; }

    [JsonPropertyName("accessKey")]
    public string? AccessKey { get; set; }

    [JsonPropertyName("accessSecret")]
    public string? AccessSecret { get; set; }
}
