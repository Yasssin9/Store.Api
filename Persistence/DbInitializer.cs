using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _storeDbContext;

        public DbInitializer(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                //if(_storeDbContext.Database.GetPendingMigrations().Any())
                //    _storeDbContext.Database.Migrate();

                if (!_storeDbContext.ProductType.Any())
                {
                        var typesData = File.ReadAllText(@"..\Persistence\Data\Seeding\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types is not null && types.Any())
                    {
                        await _storeDbContext.ProductType.AddRangeAsync(types);
                        await _storeDbContext.SaveChangesAsync();
                    }

                    if (!_storeDbContext.ProductBrand.Any())
                    {
                        var brandData = File.ReadAllText(@"..\Persistence\Data\Seeding\brands.json");
                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                        if (brands is not null && brands.Any())
                        {
                            await _storeDbContext.ProductBrand.AddRangeAsync(brands);
                            await _storeDbContext.SaveChangesAsync();
                        }
                    }

                    if (!_storeDbContext.Product.Any())
                    {
                        var productData = File.ReadAllText(@"..\Persistence\Data\Seeding\products.json");
                        var products = JsonSerializer.Deserialize<List<Product>>(productData);
                        if (products is not null && products.Any())
                        {
                            await _storeDbContext.Product.AddRangeAsync(products);
                            await _storeDbContext.SaveChangesAsync();
                            
                        }
                    }
                    
                }

            }
            catch (Exception ex) 
            { 
            
            }
        }
    }
}
