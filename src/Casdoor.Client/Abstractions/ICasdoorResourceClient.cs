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

public interface ICasdoorResourceClient
{
    public Task<CasdoorResponse?> UploadResourceAsync(string user, string tag, string parent, string fullFilePath,
        Stream fileStream, string createdTime = "", string description = "", CancellationToken cancellationToken = default);

    public Task<CasdoorResponse?> DeleteResourceAsync(string name, CancellationToken cancellationToken = default);

    public Task<CasdoorResponse?> AddResourceAsync(CasdoorUserResource casdoorUserResource, CancellationToken cancellationToken = default);

    public Task<CasdoorUserResource?> GetResourceAsync(string name, CancellationToken cancellationToken = default);

    public Task<IEnumerable<CasdoorUserResource>?> GetResourcesAsync(string owner, string user,
        string field, string value, string sortField, string sortOrder, CancellationToken cancellationToken = default);

    public Task<IEnumerable<CasdoorUserResource>?> GetPaginationResourcesAsync(string owner, string user, int pageSize, int p, string field, string value, string sortField, string sortOrder, CancellationToken cancellationToken = default);
    public Task<CasdoorUserResource?> GetResourceExAsync(string owner, string name, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateResourceAsync(CasdoorUserResource resource, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteResourceWithTagAsync(CasdoorUserResource resource, string tag,
        CancellationToken cancellationToken = default);
}
