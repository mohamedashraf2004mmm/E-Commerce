using AutoMapper;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, CreatedOrderDto>()
                    .ForMember(o => o.DeliveryMethod , o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                    .ForMember(o => o.DeliveryMethodCost , o => o.MapFrom(o => o.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(odto => odto.ProductName , o => o.MapFrom(oi => oi.Product.ProductName))
                    .ForMember(odto => odto.PictureUrl , o => o.MapFrom<OrderItemPictureUrlResolver>())
                    .ForMember(odto => odto.ProductId , o => o.MapFrom(oi => oi.Product.ProductId));
            CreateMap<DeliveryMethod, DeliveryMethodDto>();
              
        }
    }
}
