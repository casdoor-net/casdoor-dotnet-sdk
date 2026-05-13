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

namespace Casdoor.Client;

public interface ICasdoorSessionClient
{
    public Task<CasdoorResponse?> AddSessionAsync(CasdoorSession session, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateSessionAsync(CasdoorSession session, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateSessionForColumnsAsync(CasdoorSession session,IEnumerable<string>? columns = default, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteSessionAsync(CasdoorSession session, CancellationToken cancellationToken = default);
    public Task<CasdoorSession?> GetSessionAsync(string name, string application, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorSession>?> GetSessionsAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorSession>?> GetPaginationSessionsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default);
}
