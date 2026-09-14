using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Common;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext context , ILogger<CatalogDataSeeder>logger) : IDataSeeder
    {
        private readonly StoreDbContext context = context;

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync(ct);
                }

                //seeding

                //path

                //AppContext.BaseDirectory => C:\Users\FIRST\source\repos\E-Commerce\E-Commerce\bin\Debug\net8.0


                //fullPath => "C:\Users\FIRST\source\repos\E-Commerce\E-Commerce\bin\Debug\net8.0\DataSeed\products.json"


                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

               await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
               await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
               await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);

                var result = await context.SaveChangesAsync();
                if(result > 0)
                {
                    logger.LogInformation($"{result} rows added");
                }
                else
                {
                    logger.LogInformation("Database seeded or has data");
                }
            }
            catch
            {

            }
        }

        private async Task SeedIfEmptyAsync<T , TKey>(string rootPath , string fileName , CancellationToken ct)where T : BaseEntity<TKey>
        {
            if (await context.Set<T>().AnyAsync())
            {
                logger.LogInformation("Table already seeded or has data");
                return;
            }

            var filePath = Path.Combine(rootPath, fileName);
            if (!File.Exists(filePath))
            {
                logger.LogWarning($"This file {filePath} was not found");
                return;
            }

            var fileStream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            var items =  await JsonSerializer.DeserializeAsync<List<T>>(fileStream , options , ct);

            if (items?.Any() ?? false)
            {
                context.Set<T>().AddRange(items);
            }

        }
    }
}
