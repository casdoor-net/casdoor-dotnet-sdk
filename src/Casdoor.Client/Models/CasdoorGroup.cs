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


public class CasdoorGroup
{
    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }
    [JsonPropertyName("updatedTime")]
    public string? UpdatedTime { get; set; }
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
    [JsonPropertyName("manager")]
    public string? Manager { get; set; }
    [JsonPropertyName("contactEmail")]
    public string? ContactEmail { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }
    [JsonPropertyName("isTopGroup")]
    public bool? IsTopGroup { get; set; }
    [JsonPropertyName("users")]
    public List<CasdoorUser>? Users { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("key")]
    public string? Key { get; set; }
    [JsonPropertyName("children")]
    public List<CasdoorGroup>? Children { get; set; }
    [JsonPropertyName("isEnabled")]
    public bool? IsEnabled { get; set; }

    [JsonPropertyName("parentName")]
    public string? ParentName { get; set; }

    [JsonPropertyName("haveChildren")]
    public bool HaveChildren { get; set; }

    [JsonPropertyName("gidNumber")]
    public int GidNumber { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, string>? Properties { get; set; }
}


