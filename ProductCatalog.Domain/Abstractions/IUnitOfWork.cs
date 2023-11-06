using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Domain.Models.Authentication;

namespace ProductCatalog.Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<ProductCategory> ProductCategoryRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<UserRole> UserRoleRepository { get; }
        Task SaveAsync();
    }
}
