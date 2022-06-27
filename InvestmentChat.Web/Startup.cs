using InvestmentChat.Web.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace InvestmentChat.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            var appSettings = new AppSettings();
            services.AddSingleton(appSettings);
            Configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = appSettings.IdentityAPIUrl;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.ClientId = "client";
                    options.ResponseType = "code";
                    options.MapInboundClaims = false;
                    options.SaveTokens = true;
                    //disable ssl for docker
                    //options.RequireHttpsMetadata = false;
                    //HttpClientHandler handler = new HttpClientHandler();
                    //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    //options.BackchannelHttpHandler = handler;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
