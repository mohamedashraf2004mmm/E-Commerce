using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CreatedOrderDto>> CreateOrderAsync(OrderDto order, string email, CancellationToken ct = default)
        {
            //items(order item)
            var basket = await _basketRepository.GetBasketAsync(order.BasketId, ct);
            if (basket == null)
                return Error.NotFound("Basket not found", $"Basket with id {order.BasketId} is not found");

            if (basket.BasketItems.Count == 0)
                return Error.Validation("Basket is empty", $"Can not create order with basket id {basket.Id}");

            var orderItems = new List<OrderItem>(basket.BasketItems.Count);
            var productIds = basket.BasketItems.Select(x => x.Id).ToHashSet();

            var products = (await _unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductWithIdSpecifications(productIds))).ToDictionary(x => x.id);

            foreach (var item in basket.BasketItems)
            {
                if(!products.TryGetValue(item.Id , out var product))
                {
                    return Error.NotFound("Product not found", $"Product with id {item.Id} not found");
                }
                orderItems.Add(new OrderItem()
                {
                    Price = product.Price,
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        PictureUrl = product.PictureUrl,
                        ProductId = product.id,
                        ProductName = product.Name
                    }
                });
            }

            //shipping address

            //deliverymethod

            //subtotal

            //total
            //var order = new Order()
        }
    }
}
