using identity_no_seed.Data;
using identity_no_seed.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace identity_no_seed;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (roleMgr.FindByNameAsync(SD.Admin).Result == null)
            {
                roleMgr.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                roleMgr.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }

            for (int i = 0; i < 10; i++)
            {
                if (userMgr.FindByNameAsync($"user{i}").Result is null)
                {

                    ApplicationUser adminUser = new()
                    {
                        UserName = $"user{i}",
                        Email = $"user{i}@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = i.ToString(),
                    };

                    var result = userMgr.CreateAsync(adminUser, $"User{i}pass*").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    userMgr.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
                    result = userMgr.AddClaimsAsync(adminUser, new Claim[] {
                        new Claim(JwtClaimTypes.Name, $"name{i}"),
                        new Claim(JwtClaimTypes.GivenName, $"givenName{i}"),
                        new Claim(JwtClaimTypes.FamilyName, $"familyName{i}"),
                        new Claim(JwtClaimTypes.Role,SD.Admin),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug($"user{i} created!");
                }
            }
        }
    }
}
