using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions;
using Shared.ProductDtos;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        //public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandAsync()
        {
            var brands=await unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync();
            
            var mappedBrands=mapper.Map<IEnumerable<BrandResultDto>>(brands);

            return mappedBrands;
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductAsync()
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAllAsync();

            var mappedProduct=mapper.Map<IEnumerable<ProductResultDto>>(product);

            return mappedProduct;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypeAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var mappedType = mapper.Map<IEnumerable<TypeResultDto>>(types);

            return mappedType;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);

            var mappedProduct = mapper.Map<ProductResultDto>(product);

            return mappedProduct;
        }
    }
}
