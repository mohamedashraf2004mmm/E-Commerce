using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration config)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                //so it will get the connection string from appsettings.json
            });
            return services;
        }

    }
}
