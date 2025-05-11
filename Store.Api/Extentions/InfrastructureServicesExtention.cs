using System.Text;
using System.Text.Json.Serialization;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositories;
using Shared.IdentityDtos;
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
            services.ConfigureJwt(configuration);

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

        public static IServiceCollection ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {

            var jwtconfig = configuration.GetSection("JWTOptions").Get<JWTOptions>();

            services.AddAuthentication(option =>
            {   //jwt is default now of token
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtconfig.Audience,
                    ValidIssuer = jwtconfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtconfig.SecurityKey)),

                };
            });
                return services;

        }

    }
}
