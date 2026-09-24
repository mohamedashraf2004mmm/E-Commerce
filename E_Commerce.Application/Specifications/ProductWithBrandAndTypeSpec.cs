using E_Commerce.Application.Common;
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
        public ProductWithBrandAndTypeSpec(ProductQueryParams queryParams) 
           : base(p => (queryParams.BrandId == null || p.BrandId == queryParams.BrandId) 
           && (queryParams.TypeId == null || p.TypeId == queryParams.TypeId)
           && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        //brandid
        //typeid
        //both
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    SetOrderBy(p => p.Name);
                    break;

                case ProductSortingOptions.NameDesc:
                    SetOrderByDescending(p => p.Name);
                    break;

                case ProductSortingOptions.PriceAsc:
                    SetOrderBy(p => p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    SetOrderByDescending(p => p.Price);
                    break;

                default:
                    SetOrderBy(p => p.id);
                    break;

            }
            ApplyPagination(queryParams.PageSize, queryParams.pageIndex);
        }
        //Get by Id 
        public ProductWithBrandAndTypeSpec(int id) : base(p => p.id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
