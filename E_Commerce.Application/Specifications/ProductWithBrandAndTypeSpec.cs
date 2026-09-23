using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithBrandAndTypeSpec : BaseSpecification<Product,int>
    {
        //Get All
        public ProductWithBrandAndTypeSpec() : base(null)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
        //Get by Id 
        public ProductWithBrandAndTypeSpec(int id) : base(p => p.id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
