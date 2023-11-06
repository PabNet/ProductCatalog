using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;

namespace ProductCatalog.Infrastructure.Data.DataBase
{
    public class DataInitializer
    {
        private static readonly Dictionary<string, UserRole> RoleMap = new Dictionary<string, UserRole>
        {
            ["Administrator"] = new UserRole { Id = Guid.NewGuid(), Name = "Administrator", Description = "The \"Administrator\" role provides full access and control over the functionality of the application, including the ability to manage users, change settings, and view all data and operations." },
            ["AdvancedUser"] = new UserRole { Id = Guid.NewGuid(), Name = "Advanced User", Description = "The \"Advanced User\" role has advanced rights, which allows you to see and manage all application information. These users can view, edit and delete products and categories." },
            ["SimpleUser"] = new UserRole { Id = Guid.NewGuid(), Name = "Simple User", Description = "The \"Simple User\" role has access to all information except \"Special Note\" and can perform operations to add and change products." }
        };

        private static readonly Dictionary<string, Permission> PermissionMap = new Dictionary<string, Permission>
        {
            ["ViewSpecialNoteColumn"] = new Permission { Id = Guid.NewGuid(), Name = "ViewSpecialNoteColumn", Description = "Allows you to view a special note for a product, if available." },
            ["AddProduct"] = new Permission { Id = Guid.NewGuid(), Name = "AddProduct", Description = "Gives the right to create new products in the catalog." },
            ["EditProduct"] = new Permission { Id = Guid.NewGuid(), Name = "EditProduct", Description = "Allows you to edit product information, including name, description, price and other attributes." },
            ["DeleteProduct"] = new Permission { Id = Guid.NewGuid(), Name = "DeleteProduct", Description = "Allows you to remove products from the catalog." },
            ["AddCategory"] = new Permission { Id = Guid.NewGuid(), Name = "AddCategory", Description = "Allows you to create new categories for products." },
            ["EditCategory"] = new Permission { Id = Guid.NewGuid(), Name = "EditCategory", Description = "Allows you to edit category information, including title and description." },
            ["DeleteCategory"] = new Permission { Id = Guid.NewGuid(), Name = "DeleteCategory", Description = "Allows you to delete categories and related products." },
            ["AddUser"] = new Permission { Id = Guid.NewGuid(), Name = "AddUser", Description = "Gives the right to create new users in the system." },
            ["EditUser"] = new Permission { Id = Guid.NewGuid(), Name = "EditUser", Description = "Allows you to edit user information, including password and lock status." },
            ["DeleteUser"] = new Permission { Id = Guid.NewGuid(), Name = "DeleteUser", Description = "Allows you to delete user accounts from the system." }
        };

        public void InitializeData(ModelBuilder modelBuilder)
        {
            SetUpRules(modelBuilder);

            InitializeUserRoles(modelBuilder);
            InitializePermissions(modelBuilder);
            InitializeRolePermissions(modelBuilder);
        }

        private void SetUpRules(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
            .HasMany(pc => pc.Products)
            .WithOne(p => p.Category)
            .OnDelete(DeleteBehavior.Cascade);
        }

        private void InitializeUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                RoleMap.Select(r => r.Value).ToList()
            );
        }

        private void InitializePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                PermissionMap.Select(p => p.Value).ToList()
            );
        }

        private void InitializeRolePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
            .HasMany(c => c.Permissions)
            .WithMany(s => s.Roles)
            .UsingEntity<UserRolePermission>(
                j => j.HasData(
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["ViewSpecialNoteColumn"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["AddProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["EditProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["DeleteProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["AddCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["EditCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["DeleteCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["AddUser"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["EditUser"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["Administrator"].Id, PermissionId = PermissionMap["DeleteUser"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["ViewSpecialNoteColumn"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["AddProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["EditProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["DeleteProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["AddCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["EditCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["AdvancedUser"].Id, PermissionId = PermissionMap["DeleteCategory"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["SimpleUser"].Id, PermissionId = PermissionMap["AddProduct"].Id },
                    new UserRolePermission { Id = Guid.NewGuid(), RoleId = RoleMap["SimpleUser"].Id, PermissionId = PermissionMap["EditProduct"].Id }
                )
             );
        }
    }
}
