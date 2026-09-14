using E_Commerce.Domain.Contracts;

namespace E_Commerce.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication>SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope(); //for data seeding
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("catalog");

            await seeder.SeedDataAsync();

            return app;
        }
    }
}
