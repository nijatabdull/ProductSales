using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProductSale.Service.Catalog.Configurations.Helpers;
using ProductSale.Service.Catalog.Services.Abstractions;
using ProductSale.Service.Catalog.Services.Concretes;
using ProductSale.Service.Catalog.Statics.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(
        new RouteTokenTransformerConvention(
            new SlugifyParameterTransformer()));

    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["IdentityServerURL"];
        opt.Audience = "catalog_resource";
        opt.RequireHttpsMetadata= false;
    });

builder.Services.AddRouting(opt =>
{
    opt.LowercaseQueryStrings= true;
    opt.LowercaseUrls= true;
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();

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
