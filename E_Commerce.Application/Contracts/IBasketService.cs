using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IBasketService
    {
        //get basket
        Task<Result<BasketDto>> GetBasketAsync(string BasketId, CancellationToken ct = default);
        //create or update basket
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto baskket, TimeSpan? ttl = null , CancellationToken ct = default);

        // delete basket
        Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default);
    }
}
