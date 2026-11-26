using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<CreatedOrderDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            //items(order item)
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId, ct);
            if (basket == null)
                return Error.NotFound("Basket not found", $"Basket with id {orderDto.BasketId} is not found");

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
            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShippingAddress);

            //deliverymethod
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod == null)
                return Error.NotFound("Delivery Method not found", $"delivery method with id {orderDto.DeliveryMethodId} is not found");

            //subtotal
            var subTotal = orderItems.Sum(o => o.Quantity * o.Price);

            var order = new Order(email, orderAddress, orderItems, deliveryMethod, subTotal);
            _unitOfWork.GetRepository<Order,Guid>().Add(order);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result == 0)
            {
                return Error.Failure("Order save failed", "Can not create order");
            }
            else
            {
                await _basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);
                return _mapper.Map<CreatedOrderDto>(order);
            }
        }

        public async Task<Result<IReadOnlyList<CreatedOrderDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email) , ct);
            if (orders.Any())
            {
                return Result<IReadOnlyList<CreatedOrderDto>>.Ok(_mapper.Map<IReadOnlyList<CreatedOrderDto>>(orders));
            }
            else
            {
                return Error.NotFound("Orders not found", $"No orders found with email {email}");
            }
        }

        public async Task<Result<CreatedOrderDto>> GetOrderByIdAndEmailForUserAsync(Guid id, string email, CancellationToken ct = default)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(id,email) , ct);
            if(order == null)
            {
                return Error.NotFound("Order not found", $"Order with id {id} not found");
            }
            else
            {
                return Result<CreatedOrderDto>.Ok(_mapper.Map<CreatedOrderDto>(order));
            }
        }
    }
}
