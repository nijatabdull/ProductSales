using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProductSale.Service.Discount.Services.Abstractions;
using ProductSale.Service.Discount.Services.Concretes;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Shared.Services.Concretes;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityServerURL"];
        opt.Audience = "discount_resource";
        opt.RequireHttpsMetadata = false;
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
