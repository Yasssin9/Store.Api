using System.Reflection.Metadata;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using Services.MappingProfile;
using StackExchange.Redis;
using Store.Api.Factories;
using Store.Api.MiddleWares;

namespace Store.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>
                (_ => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IServiceManager,ServiceManager>();
            builder.Services.AddAutoMapper(typeof(Services.ServiceManager).Assembly);

            builder.Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });

            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await SeedDbAsync(app);

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
            
        static async Task SeedDbAsync(WebApplication app) 
        {
            using var scope = app.Services.CreateScope();

            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbIntializer.InitializeAsync();
        }
    }
}
