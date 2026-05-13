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
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Casdoor.Client;

public partial class CasdoorClient
{

    public virtual Task<CasdoorResponse?> AddCertAsync(CasdoorCert cert,
        CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("add-cert");
        cert.Owner = _options.OrganizationName;
        return PostAsJsonAsync(url, cert, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> DeleteCertAsync(string name, CancellationToken cancellationToken = default)
    {
        CasdoorCert cert = new() { Owner = _options.OrganizationName, Name = name };
        var url = _options.GetActionUrl("delete-cert");
        return PostAsJsonAsync(url, cert, cancellationToken);
    }

    public virtual async Task<CasdoorCert?> GetCertAsync(string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("id", $"{_options.OrganizationName}/{name}").QueryMap;
        string url = _options.GetActionUrl("get-cert", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<CasdoorCert?>();
    }

    public virtual async Task<IEnumerable<CasdoorCert>?> GetCertsAsync(CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder()
            .Add("owner", _options.OrganizationName).QueryMap;
        string url = _options.GetActionUrl("get-certs", queryMap);
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorCert>?>();
    }

    public virtual async Task<IEnumerable<CasdoorCert>?> GetGlobalCertsAsync(CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("get-global-certs");
        var result = await _httpClient.GetFromJsonAsync<CasdoorResponse?>(url, cancellationToken: cancellationToken);
        return result.DeserializeData<IEnumerable<CasdoorCert>?>();
    }

    public virtual Task<CasdoorResponse?> UpdateCertAsync(CasdoorCert cert, CancellationToken cancellationToken = default)
        => ModifyCertAsync("update-cert", cert, null, cancellationToken: cancellationToken);


    private Task<CasdoorResponse?> ModifyCertAsync(string action, CasdoorCert cert, IEnumerable<string>? columns = default, string? owner = null, CancellationToken cancellationToken = default)
    {
        var queryMapBuilder = new QueryMapBuilder().Add("id", $"{cert.Owner}/{cert.Name}");

        string columnsValue = string.Join(",", columns ?? Array.Empty<string>());

        if (!string.IsNullOrEmpty(columnsValue))
        {
            queryMapBuilder.Add("columns", columnsValue);
        }
        cert.Owner = _options.OrganizationName;

        string url = _options.GetActionUrl(action, queryMapBuilder.QueryMap);
        return PostAsJsonAsync(url, cert, cancellationToken);
    }
}
