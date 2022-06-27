using InvestmentChat.Web.Configurations;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})


    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = appSettings.IdentityAPIUrl;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "investor";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("investor");
        options.SaveTokens = true;
        //options.RequireHttpsMetadata = false;
        //HttpClientHandler handler = new HttpClientHandler();
        //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        //options.BackchannelHttpHandler = handler;
    });

builder.Services.AddSingleton(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
