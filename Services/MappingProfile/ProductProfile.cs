using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Shared.ProductDtos;

namespace Services.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDto>()
                .ForMember(d => d.BrandName, option => option.MapFrom(src => src.ProductBrand.Name))
                .ForMember(d => d.TypeName, option => option.MapFrom(src => src.ProductType.Name));

            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();
        }
    }
}
