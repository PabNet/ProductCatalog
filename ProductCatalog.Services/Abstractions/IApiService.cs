namespace ProductCatalog.Services.Abstractions
{
    public interface IApiService
    {
        public Task<T> ExecuteRequest<T>(Enum command, params object[] parameters);
    }
}
