using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        //create order
        Task<Result<CreatedOrderDto>> CreateOrderAsync(OrderDto order, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<CreatedOrderDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default);
        Task<Result<CreatedOrderDto>> GetOrderByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default);
    }
}
