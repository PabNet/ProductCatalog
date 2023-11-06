namespace ProductCatalog.Services.Abstractions
{
    public interface IEntityService<T>
    {
        Task<bool> AddEntryAsync(T entry);
        Task<bool> UpdateEntry(T entry);
        Task<bool> RemoveEntry(T entry);
        Task<List<T>> GetEntries();
        Task<List<T>> GetEntries(Func<T, bool> predicate);
        Task<T?> GetFullEntry(T entry);
    }
}
