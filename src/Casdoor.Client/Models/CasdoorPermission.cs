// Copyright 2023 The Casdoor Authors. All Rights Reserved.
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

using System.Text.Json.Serialization;

namespace Casdoor.Client;

/// <summary>
///     CasdoorPermission has the same definition as https://github.com/casdoor/casdoor/blob/master/object/permission.go#L24
/// </summary>

public class CasdoorPermission
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("users")]
    public IEnumerable<string>? Users { get; set; }

    [JsonPropertyName("roles")]
    public IEnumerable<string>? Roles { get; set; }

    [JsonPropertyName("domains")]
    public IEnumerable<string>? Domains { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("adapter")]
    public string? Adapter { get; set; }

    [JsonPropertyName("resourceType")]
    public string? ResourceType { get; set; }

    [JsonPropertyName("resources")]
    public IEnumerable<string>? Resources { get; set; }

    [JsonPropertyName("actions")]
    public IEnumerable<string>? Actions { get; set; }

    [JsonPropertyName("effect")]
    public string? Effect { get; set; }

    [JsonPropertyName("submitter")]
    public string? Submitter { get; set; }

    [JsonPropertyName("approver")]
    public string? Approver { get; set; }

    [JsonPropertyName("approveTime")]
    public string? ApproveTime { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("isEnabled")]
    public bool IsEnabled { get; set; }

    [JsonPropertyName("groups")]
    public IEnumerable<string>? Groups { get; set; }

    [JsonPropertyName("sourceGroups")]
    public IEnumerable<string>? SourceGroups { get; set; }

    [JsonPropertyName("sourceRoles")]
    public IEnumerable<string>? SourceRoles { get; set; }

    [JsonPropertyName("expireTime")]
    public string? ExpireTime { get; set; }
}
