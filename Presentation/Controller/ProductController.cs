using System.Net;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;
using Shared.ProductDtos;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController(IServiceManager serviceManager): ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationParams specs)
        {
            var products = await serviceManager.ProductService.GetAllProductAsync(specs);

            return Ok(products);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ErrorDetails),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResultDto>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductByIdAsync(id);

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var Brands = await serviceManager.ProductService.GetAllBrandAsync();

            return Ok(Brands);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var Types = await serviceManager.ProductService.GetAllTypeAsync();

            return Ok(Types);
        }

    }
}
