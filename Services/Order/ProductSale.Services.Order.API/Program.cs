using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using ProductSale.Services.Order.BL.Handlers;
using ProductSale.Services.Order.BL.MassTransit.Consumer;
using ProductSale.Services.Order.DAL;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Shared.Services.Concretes;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(opt =>
{
    opt.AddConsumer<CreateOrderMessageCommandConsumer>();
    opt.AddConsumer<CourseNameChangedEventConsumer>();

    opt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQUrl"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        cfg.ReceiveEndpoint("create-order", e =>
        {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
        });

        cfg.ReceiveEndpoint("course-name-change", e =>
        {
            e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
        });
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityServerURL"];
        opt.Audience = "order_resource";
        opt.RequireHttpsMetadata = false;
    });

builder.Services.AddRouting(opt =>
{
    opt.LowercaseQueryStrings = true;
    opt.LowercaseUrls = true;
});

builder.Services.AddScoped<IUserProvider,UserProvider>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssemblyContaining(typeof(CreateOrderCommandHandler));
});
builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using var scope = app.Services.CreateScope();

var orderDbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

orderDbContext.Database.Migrate();

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
