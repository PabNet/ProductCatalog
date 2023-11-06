namespace ProductCatalog.Domain.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity item);
        Task CreateRangeAsync(IEnumerable<TEntity> items);
        Task CreateOrUpdateAsync(TEntity item, Func<TEntity, bool> predicate);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate);
        void Remove(TEntity item);
        void Update(TEntity item);
    }
}
