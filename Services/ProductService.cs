using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specification;
using Shared;
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

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductAsync(ProductSpecificationParams specifications)
        {
            var specs=new ProductWithFilterSpecification(specifications);

            var product = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs);

            var countSpecs = new ProductCountSpecification(specifications);

            var totalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs);

            var mappedProduct=mapper.Map<IEnumerable<ProductResultDto>>(product);

            return new PaginatedResult<ProductResultDto>(specifications.PageIndex,
                specifications.PageSize, totalCount, mappedProduct);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypeAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();

            var mappedType = mapper.Map<IEnumerable<TypeResultDto>>(types);

            return mappedType;
        }

        public async Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            var specs= new ProductWithFilterSpecification(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(specs);

            var mappedProduct = mapper.Map<ProductResultDto>(product);

            return product is null ? throw new ProductNotFoundException(id) : mappedProduct;ؤ
        }

       
    }
}
