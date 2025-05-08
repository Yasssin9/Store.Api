using Domain.Contracts;

namespace Store.Api.Extentions
{
    public static class WebApplicationExtention
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbIntializer.InitializeAsync();
            await dbIntializer.InitializeIdentityAsync();

            return app;
        }
    }
}
