using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using ProductSale.Service.Basket.MassTransit.Events;
using ProductSale.Service.Basket.Services.Abstractions;
using ProductSale.Service.Basket.Services.Concretes;
using ProductSale.Service.Basket.Statics;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Shared.Services.Concretes;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(opt =>
{
    opt.AddConsumer<CourseNameChangedEventConsumer>();

    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", x =>
        {
            x.Username("guest");
            x.Password("guest");
        });

        cfg.ReceiveEndpoint("course-name-change-for-basket", x =>
        {
            x.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
        });
    });
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddSingleton<RedisService>(opt =>
{
    IOptions<RedisSettings> optionsSnapshot = opt.GetRequiredService<IOptions<RedisSettings>>();

    RedisSettings redisSettings = optionsSnapshot.Value;

    RedisService redisService = new(redisSettings.Host, redisSettings.Port);

    redisService.Connect();

    return redisService;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityServerURL"];
        opt.Audience = "basket_resource";
        opt.RequireHttpsMetadata = false;
    });


builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

builder.Services.AddRouting(opt =>
{
    opt.LowercaseQueryStrings = true;
    opt.LowercaseUrls = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
