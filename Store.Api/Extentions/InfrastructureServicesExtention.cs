using System.Text.Json.Serialization;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using StackExchange.Redis;

namespace Store.Api.Extentions
{
    public static class InfrastructureServicesExtention
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();          

            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>
                (_ => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

            services.ConfigureIdentity();

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequireLowercase = false;
                Options.Password.RequireUppercase = false;
                Options.Password.RequireDigit = true;
                Options.Password.RequiredLength = 8;
                Options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }
    }
}
