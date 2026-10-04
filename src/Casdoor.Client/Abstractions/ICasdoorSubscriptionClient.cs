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

public interface ICasdoorSubscriptionClient
{
    public Task<CasdoorResponse?> AddSubscriptionAsync(CasdoorSubscription casdoorSubscription, CancellationToken cancellationToken = default);

    public Task<CasdoorResponse?> DeleteSubscriptionAsync(CasdoorSubscription casdoorSubscription, CancellationToken cancellationToken = default);

    public Task<CasdoorSubscription?> GetSubscriptionAsync(string owner, string name, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorSubscription>?> GetPaginationSubscriptions(string owner, int p, int pageSize,
        List<KeyValuePair<string, string?>> queryMap, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorSubscription>?> GetSubscriptionsAsync(string owner, CancellationToken cancellationToken = default);

    public Task<CasdoorResponse?> UpdateSubscriptionAsync(CasdoorSubscription casdoorSubscription, CancellationToken cancellationToken = default);
    public Task<(IEnumerable<CasdoorSubscription>? subscriptions, int totalCount)> GetPaginationSubscriptionsAsync(int p, int pageSize,
        List<KeyValuePair<string, string?>>? queryMap = null, CancellationToken cancellationToken = default);
}
