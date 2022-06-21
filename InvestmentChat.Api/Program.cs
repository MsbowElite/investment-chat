using InvestmentChat.Api.Consumers;
using InvestmentChat.Api.Hubs;
using InvestmentChat.Domain.Services;
using InvestmentChat.Infra.CrossCutting.Utils.Settings;
using InvestmentChat.Infra.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSignalRSwaggerGen();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins("https://localhost:6001")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});
builder.Services.AddSignalR();

builder.Services.AddHostedService<BotMessageConsumer>();

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