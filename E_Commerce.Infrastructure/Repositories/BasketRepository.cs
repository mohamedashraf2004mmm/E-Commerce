using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        //deals with redis database
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(basket);

            var expiry = !timeToLive.HasValue || timeToLive.Value <= TimeSpan.Zero
                ? TimeSpan.FromDays(7)
                : timeToLive.Value;

            var result = await _database.StringSetAsync(
                basket.Id,
                value,
                expiry);

            return result ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
           return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _database.StringGetAsync(basketId);
            if (basket.IsNullOrEmpty) return null;
            else return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
    }
}
