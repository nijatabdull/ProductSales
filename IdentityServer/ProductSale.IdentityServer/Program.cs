using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductSale.IdentityServer;
using ProductSale.IdentityServer.Data;
using ProductSale.IdentityServer.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();


    using var scope = app.Services.CreateScope();

    IServiceProvider serviceProvider = scope.ServiceProvider;

    var appDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

    appDbContext.Database.Migrate();

    var appUserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if(appUserManager.Users.Any() is false)
    {
        appUserManager.CreateAsync(new ApplicationUser { UserName = "nijatabdull", Email = "nicata099@gmail.com" }, "Nic@t12345")
            .GetAwaiter().GetResult();
    }

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException") // https://github.com/dotnet/runtime/issues/60600
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}