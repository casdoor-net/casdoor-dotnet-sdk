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

using System.Text.Json.Serialization;

namespace Casdoor.Client;

/// <summary>
///     CasdoorCasbinRule is a policy of an enforcer. The policies are xorm CasbinRule objects without JSON tags in Casdoor,
///     so the JSON keys are capitalized.
/// </summary>
public class CasdoorCasbinRule
{
    [JsonPropertyName("Id")] public long Id { get; set; }
    [JsonPropertyName("Ptype")] public string? Ptype { get; set; }
    [JsonPropertyName("V0")] public string? V0 { get; set; }
    [JsonPropertyName("V1")] public string? V1 { get; set; }
    [JsonPropertyName("V2")] public string? V2 { get; set; }
    [JsonPropertyName("V3")] public string? V3 { get; set; }
    [JsonPropertyName("V4")] public string? V4 { get; set; }
    [JsonPropertyName("V5")] public string? V5 { get; set; }
}

/// <summary>
///     CasdoorPolicyFilter filters the policies of an enforcer, see GetFilteredPoliciesAsync().
/// </summary>
public class CasdoorPolicyFilter
{
    [JsonPropertyName("ptype")] public string? Ptype { get; set; }
    [JsonPropertyName("fieldIndex")] public int? FieldIndex { get; set; }
    [JsonPropertyName("fieldValues")] public IEnumerable<string>? FieldValues { get; set; }
}
