// Copyright 2022 The Casdoor Authors. All Rights Reserved.
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

public interface ICasdoorEnforcerClient
{
    public Task<CasdoorResponse?> AddEnforcerAsync(CasdoorEnforcer enforcer, CancellationToken cancellationToken = default);

    public Task<CasdoorResponse?> UpdateEnforcerAsync(CasdoorEnforcer enforcer, string enforcerId, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteEnforcerAsync(CasdoorEnforcer enforcer, CancellationToken cancellationToken = default);
    public Task<CasdoorEnforcer?> GetEnforcerAsync(string name, string? owner = null, CancellationToken cancellationToken = default);

    public Task<IEnumerable<CasdoorEnforcer>?> GetEnforcersAsync(string? owner = null, CancellationToken cancellationToken = default);
    public Task<(IEnumerable<CasdoorEnforcer>? enforcers, int totalCount)> GetPaginationEnforcersAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default);
}
