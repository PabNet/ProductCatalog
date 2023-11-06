using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Services.Abstractions
{
    public interface IUnitOfWork
    {
        public IEntityService<Product> ProductService { get; }
        public IEntityService<ProductCategory> ProductCategoryService { get; }
        public IEntityService<User> UserService { get; }
        public IEntityService<UserRole> UserRoleService { get; }
    }
}
