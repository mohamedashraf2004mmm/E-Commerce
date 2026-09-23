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
        public ProductWithBrandAndTypeSpec(int? BrandId, int? TypeId) 
           : base(p => (BrandId == null || p.BrandId == BrandId) && (TypeId == null || p.TypeId == TypeId))
        //brandid
        //typeid
        //both
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
