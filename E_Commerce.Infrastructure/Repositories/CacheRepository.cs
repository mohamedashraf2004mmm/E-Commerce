using E_Commerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(cacheKey);
            if (value.IsNullOrEmpty) return null;
            else
                return value.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
           await _database.StringSetAsync(cacheKey, cacheValue, timeToLive ?? TimeSpan.FromDays(7));
        }
    }
}
