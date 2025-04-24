using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Shared.ProductDtos;

namespace Services.Specification
{
    public class ProductWithFilterSpecification : Specification<Product>
    {
        public ProductWithFilterSpecification(int id) : base(Product => Product.Id == id)
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);

        }

        public ProductWithFilterSpecification(ProductSpecificationParams specs)
            : base(Product => (!specs.BrandId.HasValue || Product.BrandId == specs.BrandId) &&
                            (!specs.TypeId.HasValue || Product.TypeId == specs.TypeId) &&
                            (string.IsNullOrEmpty(specs.Search) ||
                                Product.Name.ToLower().Contains(specs.Search.ToLower().Trim()))

            )
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);

            ApplyPagination(specs.PageIndex, specs.PageIndex);

            if (specs.Sort is not null)
            {
                switch (specs.Sort)
                {
                    case "nameAsc":
                        SetOrderBy(Product => Product.Name);
                        break;

                    case "nameDesc":
                        SetOrderByDescending(Product => Product.Name);
                        break;

                    case "priceAsc":
                        SetOrderBy(Product => Product.Price);
                        break;

                    case "priceDesc":
                        SetOrderByDescending(Product => Product.Price);
                        break;

                    default:
                        SetOrderBy(Product => Product.Name);
                        break;
                }

            }
        }
    }
}