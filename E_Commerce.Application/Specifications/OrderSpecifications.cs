using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class OrderSpecifications : BaseSpecification<Order , Guid>
    {
        public OrderSpecifications(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            SetOrderByDescending(o => o.OrderDate);
        }
        public OrderSpecifications(Guid id ,string email) : base(o => o.BuyerEmail == email && o.id == id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }
    }
}
