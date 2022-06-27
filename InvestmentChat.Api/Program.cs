using InvestmentChat.Api.Consumers;
using InvestmentChat.Api.Hubs;
using InvestmentChat.Api.Settings;
using InvestmentChat.Domain.Services;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using InvestmentChat.Infra.Data.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSignalRSwaggerGen();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvestmentChat.API", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Bearer [token]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }

                });
});

var appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins(appSettings.InvestmentWebUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });

builder.Services.AddHostedService<BotMessageConsumer>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = appSettings.IdentityAPIUrl;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var rabbitMQSettings = new RabbitMQSettings();
builder.Configuration.GetSection("RabbitMQSettings").Bind(rabbitMQSettings);
builder.Services.AddSingleton(rabbitMQSettings);
builder.Services.AddSingleton<IRabbitMQPublisher>(x =>
    new RabbitMQPublisher(rabbitMQSettings));

builder.Services.AddSingleton<ChatHub>();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.UseHttpsRedirection();

app.Run();