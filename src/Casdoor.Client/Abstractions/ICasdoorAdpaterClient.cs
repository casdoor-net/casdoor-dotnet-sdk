using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casdoor.Client;

namespace Casdoor.Client;

public interface ICasdoorAdapterClient
{
    public Task<CasdoorResponse?> AddAdapterAsync(CasdoorAdapter adapter, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateAdapterAsync(CasdoorAdapter adapter, IEnumerable<string>? propertyNames = default, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> UpdateAdapterColumnsAsync(CasdoorAdapter adapter, IEnumerable<string>? columns = default, CancellationToken cancellationToken = default);
    public Task<CasdoorResponse?> DeleteAdapterAsync(string owner, string name, CancellationToken cancellationToken = default);
    public Task<CasdoorAdapter?> GetAdapterAsync(string owner, string name, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorAdapter>?> GetAdaptersAsync(string owner, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CasdoorAdapter>?> GetPaginationAdaptersAsync(string owner, int pageSize, int p,
        List<KeyValuePair<string, string?>>? queryMap, CancellationToken cancellationToken = default);
}
