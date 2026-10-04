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

namespace Casdoor.Client;

public interface ICasdoorMfaClient
{
    public Task<CasdoorResponse?> InitiateMfaAsync(string owner, string mfaType, string name, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> VerifyMfaAsync(string owner, string mfaType, string name, string secret, string passcode,
        CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> EnableMfaAsync(string owner, string mfaType, string name, string secret, string recoveryCode,
        CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> SetPreferredMfaAsync(string owner, string mfaType, string name, string secret = "",
        CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteMfaAsync(string owner, string name, CancellationToken cancellationToken = default);
}
