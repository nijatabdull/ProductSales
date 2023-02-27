using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
});

builder.Services.AddAuthentication()
    .AddJwtBearer("AuthScheme", opt =>
    {
        opt.Authority = builder.Configuration["IdentityServerURL"];
        opt.Audience = "gateway_resource";
        opt.RequireHttpsMetadata = false;
    });

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
