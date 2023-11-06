using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Utility.Helpers;

namespace ProductCatalog.Infrastructure.Data.DataBase
{
    public sealed class ProductCatalogContext : DbContext
    {
        private ConfigurationUtility _configurationUtility { get; set; }
        private DataInitializer _dataInitializer { get; set; }

        public DbSet<Product> Products { get; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; } = null!;
        public DbSet<UserRole> Roles { get; } = null!;
        public DbSet<User> Users { get; } = null!;
        public DbSet<UserRolePermission>? RolePermissions { get; }

        public ProductCatalogContext(ConfigurationUtility configurationUtility, DataInitializer dataInitializer)
        {
            _configurationUtility = configurationUtility;
            _dataInitializer = dataInitializer;

            Database.EnsureCreated();
        }

        public void DeleteDataBase()
        {
            Database.EnsureDeleted();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                          .UseSqlServer(_configurationUtility.GetValue("ConnectionStrings:MSSQL"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _dataInitializer.InitializeData(modelBuilder);
        }
    }
}
