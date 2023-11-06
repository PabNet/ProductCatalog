using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Infrastructure.Data.DataBase;

namespace ProductCatalog.Infrastructure.Data
{
    public class UnitOfWork : ProductCatalog.Domain.Abstractions.IUnitOfWork
    {
        private readonly ProductCatalogContext _context;
        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<ProductCategory> ProductCategoryRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<UserRole> UserRoleRepository { get; }

        public UnitOfWork(ProductCatalogContext context,
            IGenericRepository<Product> productRepository,
            IGenericRepository<ProductCategory> productCategoryRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<UserRole> userRoleRepository)
        {
            _context = context;

            ProductRepository = productRepository;
            ProductCategoryRepository = productCategoryRepository;
            UserRepository = userRepository;
            UserRoleRepository = userRoleRepository;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
