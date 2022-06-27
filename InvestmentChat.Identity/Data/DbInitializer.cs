using InvestmentChat.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerAspNetIdentity.Data
{
    public static class DbInitializer
    {
        public static void DbInitialize(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
