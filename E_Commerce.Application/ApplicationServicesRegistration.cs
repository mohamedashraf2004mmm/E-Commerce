using E_Commerce.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // services.AddAutoMapper(c => c.AddProfiles(new[] { new ProductProfile() }));

            services.AddAutoMapper(c => { }, typeof(ApplicationServicesRegistration).Assembly);
            return services;
        }
    }
}
