using Services.Abstractions;
using Services;
using Shared.IdentityDtos;

namespace Store.Api.Extentions
{
    public static class CoreServicesExtentions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(Services.ServiceManager).Assembly);
            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));

            return services;

        }
    }
}
