using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Infrastructure.Business
{
    public class UnitOfWork : ProductCatalog.Services.Abstractions.IUnitOfWork
    {
        public IEntityService<Product> ProductService { get; set; }

        public IEntityService<ProductCategory> ProductCategoryService { get; set; }

        public IEntityService<User> UserService { get; set; }

        public IEntityService<UserRole> UserRoleService { get; set; }

        public UnitOfWork(IEntityService<Product> productService,
            IEntityService<ProductCategory> productCategoryService,
            IEntityService<User> userService,
            IEntityService<UserRole> userRoleService)
        {
            ProductService = productService;
            ProductCategoryService = productCategoryService;
            UserService = userService;
            UserRoleService = userRoleService;
        }
    }
}
