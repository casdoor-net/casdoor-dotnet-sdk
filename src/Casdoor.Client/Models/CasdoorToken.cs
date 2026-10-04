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

public class CasdoorToken
{
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("application")]
    public string? Application { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("codeChallenge")]
    public string? CodeChallenge { get; set; }

    [JsonPropertyName("codeExpireIn")]
    public long? CodeExpireIn { get; set; }

    [JsonPropertyName("codeIsUsed")]
    public bool? CodeIsUsed { get; set; }

    [JsonPropertyName("createdTime")]
    public string? CreatedTime { get; set; }

    [JsonPropertyName("expiresIn")]
    public long? ExpiresIn { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("organization")]
    public string? Organization { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("tokenType")]
    public string? TokenType { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("idToken")]
    public string? IdToken { get; set; }

    [JsonPropertyName("accessTokenHash")]
    public string? AccessTokenHash { get; set; }

    [JsonPropertyName("refreshTokenHash")]
    public string? RefreshTokenHash { get; set; }

    [JsonPropertyName("idTokenHash")]
    public string? IdTokenHash { get; set; }

    [JsonPropertyName("grantType")]
    public string? GrantType { get; set; }

    [JsonPropertyName("resource")]
    public string? Resource { get; set; }

    [JsonPropertyName("dPoPJkt")]
    public string? DPoPJkt { get; set; }

    [JsonPropertyName("sessionId")]
    public string? SessionId { get; set; }
}
