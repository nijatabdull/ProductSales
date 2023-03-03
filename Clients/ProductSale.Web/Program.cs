using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Shared.Services.Concretes;
using ProductSale.Web.Handlers;
using ProductSale.Web.Models;
using ProductSale.Web.Services.Abstractions;
using ProductSale.Web.Services.Concretes;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.Configure<ClientSetting>(builder.Configuration.GetSection("ClientSetting"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserProvider, UserProvider>();

builder.Services.AddHttpClient<IIdentityService, IdentityService>();

builder.Services.AddHttpClient<IClientTokenService, ClientTokenService>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(opt =>
{
    string url = $"{builder.Configuration["AppSetting:BaseUrl"]}{builder.Configuration["AppSetting:ServicePath:Catalog"]}";

    opt.BaseAddress = new Uri(url);
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();

builder.Services.AddHttpClient<IPhotoService, PhotoService>(opt =>
{
    string url = $"{builder.Configuration["AppSetting:BaseUrl"]}{builder.Configuration["AppSetting:ServicePath:Photo"]}";

    opt.BaseAddress = new Uri(url);
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();

builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();

builder.Services.AddHttpClient<IUserService, UserService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["AppSetting:IdentityBaseUrl"]);
}).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/Auth/SignIn";
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
        opt.Cookie.Name = "Microservice_Cookie";
    });

builder.Services.AddAccessTokenManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
