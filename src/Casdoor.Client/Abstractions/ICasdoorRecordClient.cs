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

public interface ICasdoorRecordClient
{
    public Task<CasdoorResponse?> AddRecordAsync(CasdoorRecord record, CancellationToken cancellationToken = default);
    [Obsolete("Casdoor has no update-record API, so this call always fails.")]
    public Task<CasdoorResponse?> UpdateRecordAsync(CasdoorRecord record, CancellationToken cancellationToken = default);
    [Obsolete("Casdoor has no delete-record API, so this call always fails.")]
    public Task<CasdoorResponse?> DeleteRecordAsync(CasdoorRecord record, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Gets a record by name, or null if it doesn't exist. Casdoor has no API to get a single record, so it
    ///     searches the records by name. Like the other APIs that read records, it needs the access token of an
    ///     admin user, see WithAccessToken().
    /// </summary>
    public Task<CasdoorRecord?> GetRecordAsync(string name, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorRecord>?> GetRecordsAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorRecord>?> GetPaginationRecordsAsync(int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default);
}
