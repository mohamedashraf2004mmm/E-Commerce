using E_Commerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    //Table
    public class Order : BaseEntity<Guid>
    {
        private Order()
        {
            
        }
        public Order(string buyerEmail, OrderAddress shippingAddress, ICollection<OrderItem> items, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; } = default!;
        public OrderAddress ShippingAddress { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public decimal SubTotal { get; set; } //order cost
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int DeliveryMethodId { get; set; } //foreign key
        //[NotMapped]
        //public decimal Total
        //{
        //    get
        //    {
        //        return SubTotal + DeliveryMethod.Cost;
        //    }
        //}

        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Cost ?? 0);


    }
}
