using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Infrastructure.Data.DataBase;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class EntityModelRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ProductCatalogContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EntityModelRepository(ProductCatalogContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item));
            }

            await _context.AddAsync(item);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> items)
        {
            if (items == null)
            {
                throw new ArgumentException(nameof(items));
            }

            await _context.AddRangeAsync(items);
        }

        public async Task CreateOrUpdateAsync(TEntity item, Func<TEntity, bool> predicate)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var dbEntity = (await GetAsync(predicate)).FirstOrDefault();
            if (dbEntity != null)
            {
                Update(item);
            }
            else
            {
                await CreateAsync(item);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentException(nameof(predicate));
            }

            return (await GetAsync()).Where(e => predicate(e)).ToList();
        }

        public void Remove(TEntity item)
        {
            if (item == default)
            {
                throw new ArgumentException(nameof(item));
            }

            _context.Remove(item);
        }

        public void Update(TEntity item)
        {
            if (item == default)
            {
                throw new ArgumentException(nameof(item));
            }

            _context.Update(item);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
