using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _storeDbContext;
        private readonly StoreIdentityDbContext _storeIdentityDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbInitializer(StoreDbContext storeDbContext, StoreIdentityDbContext storeIdentityDbContext, RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            _storeDbContext = storeDbContext;
            _storeIdentityDbContext = storeIdentityDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task InitializeIdentityAsync()
        {
            if (_storeIdentityDbContext.Database.GetPendingMigrations().Any())
                 _storeIdentityDbContext.Database.Migrate();

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "Super Admin",
                    Email="SuperAdmin@gmail.com",
                    UserName="SuperAdmin",
                    PhoneNumber="11111111",
                };

                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "dmin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "2222222",
                };

                await _userManager.CreateAsync(superAdminUser,"Passw0rd");
                await _userManager.CreateAsync(adminUser, "Passw0rd");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");

            }
        }
    }
}
