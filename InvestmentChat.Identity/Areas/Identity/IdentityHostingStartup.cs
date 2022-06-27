using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(InvestmentChat.Identity.Areas.Identity.IdentityHostingStartup))]
namespace InvestmentChat.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}