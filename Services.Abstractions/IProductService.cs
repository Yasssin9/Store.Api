using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.ProductDtos;

namespace Services.Abstractions
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResultDto>> GetAllProductAsync();

        Task<ProductResultDto> GetProductByIdAsync(int id);

        Task<IEnumerable<TypeResultDto>> GetAllTypeAsync();
        Task<IEnumerable<BrandResultDto>> GetAllBrandAsync();

        //Task<TypeResultDto> GetTypeByIdAsync(int id);
        //Task<BrandResultDto> GetBrandByIdAsync(int id);
    }
}
