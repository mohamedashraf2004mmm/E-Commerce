using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity;
using E_Commerce.Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext , 
            UserManager<ApplicationUser>userManager,
            RoleManager<IdentityRole>roleManager,
            ILogger<IdentityDataSeeder>logger)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._logger = logger;
        }
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync(ct);
                }

                if (!await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!await _userManager.Users.AnyAsync())
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Mohamed Ashraf",
                        Email = "MohamedAshraf13@gmail.com",
                        UserName = "MohamedAshraffff",
                        PhoneNumber = "01129207222"
                    };
                    var createUserResult = await _userManager.CreateAsync(admin, "P@ssw0rd");
                    if (createUserResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                    }
                    else
                    {
                        var errors = string.Join(';', createUserResult.Errors.Select(e => e.Description));
                        _logger.LogWarning($"Can not seed default admin : {errors}");
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Identity data seeding failed");
                return;
            }
        }
    }
}
