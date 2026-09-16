using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }
        //get all products
        [HttpGet]
        public async Task<ActionResult<Result<IReadOnlyList<ProductDto>>>> GetAllProducts(CancellationToken ct)
        {
            var result = await _productService.GetAllProductsAsync(ct);
            return Ok(result);

        }

        //get product by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ProductDto>>>GetProduct(int id , CancellationToken ct)
        {
            var result = await _productService.GetProductByIdAsync(id, ct);
            return Ok(result);
        }
        //get product types
        [HttpGet("types")]
        public async Task<ActionResult<Result<IReadOnlyList<TypeDto>>>> GetAllTypes(CancellationToken ct)
        {
            var result = await _productService.GetAllTypesAsync(ct);
            return Ok(result);
        }
        //get product brands
        [HttpGet("brands")]
        public async Task<ActionResult<Result<IReadOnlyList<BrandDto>>>> GetAllBrands(CancellationToken ct)
        {
            var result = await _productService.GetAllBrandsAsync(ct);
            return Ok(result);
        }
    }
}
