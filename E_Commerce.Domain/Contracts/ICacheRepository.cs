using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ICacheRepository //to get the data from redis
    {
        Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default);
        Task SetAsync(string cacheKey, string cacheValue, TimeSpan? timeToLive = default , CancellationToken ct = default);
    }
}
