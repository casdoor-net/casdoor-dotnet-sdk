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

public interface ICasdoorUserClient
{
    public Task<IEnumerable<CasdoorUser>?> GetUsersAsync(string? owner = null, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorUser>?> GetSortedUsersAsync(string sorter, int limit, string? owner = null, CancellationToken cancellationToken = default);
    public Task<CasdoorUser?> GetUserAsync(string name, string? owner = null, CancellationToken cancellationToken = default);
    public Task<CasdoorUser?> GetUserByEmailAsync(string email, string? owner = null, CancellationToken cancellationToken = default);
    public Task<CasdoorUser?> GetUserByPhoneAsync(string phone, string? owner = null, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> AddUserAsync(CasdoorUser user, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateUserAsync(CasdoorUser user, IEnumerable<string> propertyNames, CancellationToken cancellationToken = default );
    public Task<CasdoorResponse?> UpdateUserByIdAsync(string userId, CasdoorUser user, CancellationToken cancellationToken);
    public Task<CasdoorResponse?> UpdateUserForColumns(CasdoorUser user, IEnumerable<string> columns, CancellationToken cancellationToken);
    public Task<CasdoorResponse?> UpdateUserForbiddenFlagAsync(CasdoorUser user, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateUserDeletedFlagAsync(CasdoorUser user, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteUserAsync(string name, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> CheckUserPasswordAsync(string name, CancellationToken cancellationToken = default);
    public Task<int?> GetUserCount(string owner, int isOnline, CancellationToken cancellationToken = default);
    public Task<(IEnumerable<CasdoorUser>? users, int totalCount)> GetPaginatedUsersAsync(int page, int pageSize, IDictionary<string, string?>? queryParams = null, string? owner = null, CancellationToken cancellationToken = default);
}
