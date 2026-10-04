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

public interface ICasdoorTransactionClient
{
    public Task<IEnumerable<CasdoorTransaction>?> GetTransactionsAsync(string? owner = null, CancellationToken cancellationToken = default);
    public Task<(IEnumerable<CasdoorTransaction>? transactions, int totalCount)> GetPaginationTransactionsAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default);
    public Task<CasdoorTransaction?> GetTransactionAsync(string name, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> AddTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteTransactionAsync(CasdoorTransaction transaction, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorTransaction>?> GetUserTransactionsAsync(string userName, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> AddTransactionWithDryRunAsync(CasdoorTransaction transaction, bool dryRun,
        CancellationToken cancellationToken = default);
}
