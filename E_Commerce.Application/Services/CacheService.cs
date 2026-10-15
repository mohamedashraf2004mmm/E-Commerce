using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepo;

        public CacheService(ICacheRepository cacheRepo )
        {
            _cacheRepo = cacheRepo;
        }
        public async Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default)
        {
            return await _cacheRepo.GetDataAsync(cacheKey);
        }

        public async Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var jsonValue = JsonSerializer.Serialize(cacheValue , new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepo.SetAsync(cacheKey, jsonValue, timeToLive);
        }
        //there is an attribute that will use this service
    }
}
