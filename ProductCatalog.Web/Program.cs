using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Infrastructure.Data.DataBase;
using ProductCatalog.Utility.Helpers;
using System.Web;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Infrastructure.Repositories;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;
using ProductCatalog.Services.Data;
using ProductCatalog.Web.Configuration.Middleware;
using ProductCatalog.Infrastructure.Business.Common;
using ProductCatalog.Services.Models.NationalBank.Templates;
using ProductCatalog.Utility.Proxies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllersWithViews();

services.AddHttpContextAccessor();
services.AddScoped<CookieUtility>();

services.AddScoped<ProductCatalogExceptionMiddleware>();

var appsettingsName = "appsettings.json";

#if DEBUG
    appsettingsName = "appsettings.Development.json";
#endif

var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(appsettingsName)
                        .Build();

services.AddSingleton(c => configuration);
services.AddSingleton<ConfigurationUtility>();
services.AddSingleton<ProductCatalog.Utility.Helpers.HttpUtility>();
services.AddSingleton<EncryptionUtility>();
services.AddSingleton<CookieUtility>();
services.AddSingleton<DataInitializer>();

var encryptionUtility = services.BuildServiceProvider().GetService<EncryptionUtility>();
ProxyUtitlity<EncryptionUtility>.Initialize(encryptionUtility);

services.AddDbContext<ProductCatalogContext>();

services.AddScoped<IGenericRepository<User>, EntityModelRepository<User>>();
services.AddScoped<IGenericRepository<UserRole>, EntityModelRepository<UserRole>>();
services.AddScoped<IGenericRepository<Product>, EntityModelRepository<Product>>();
services.AddScoped<IGenericRepository<ProductCategory>, EntityModelRepository<ProductCategory>>();

services.AddScoped<ProductCatalog.Domain.Abstractions.IUnitOfWork, ProductCatalog.Infrastructure.Data.UnitOfWork>();

services.AddScoped<IEntityService<Product>, ProductService>();
services.AddScoped<IEntityService<ProductCategory>, ProductCategoryService>();
services.AddScoped<IEntityService<User>, UserService>();
services.AddScoped<IEntityService<UserRole>, UserRoleService>();

services.AddScoped<ProductCatalog.Services.Abstractions.IUnitOfWork, ProductCatalog.Infrastructure.Business.UnitOfWork>();

services.AddScoped<IAccessControlService, AccessControlService>();

services.AddScoped<IApiService, NationalBankApiService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseMiddleware<ProductCatalogExceptionMiddleware>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");
});

app.Run();
