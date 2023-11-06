using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Services.Data
{
    public class ProductCategoryService : BaseEntityService, IEntityService<ProductCategory>
    {
        public ProductCategoryService(ProductCatalog.Domain.Abstractions.IUnitOfWork unitOfWork, ILogger<ProductCategoryService> logger) : base(unitOfWork, logger) {}

        public async Task<bool> AddEntryAsync(ProductCategory entry)
        {
            var result = default(bool);

            try
            {
                await this._unitOfWork.ProductCategoryRepository.CreateAsync(entry);
                await this._unitOfWork.SaveAsync();

                result = true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while adding Category");
            }

            return result;
        }

        public async Task<List<ProductCategory>> GetEntries()
        {
            return (await this._unitOfWork.ProductCategoryRepository.GetAsync()).ToList();
        }

        public async Task<List<ProductCategory>> GetEntries(Func<ProductCategory, bool> predicate)
        {
            return (await this._unitOfWork.ProductCategoryRepository.GetAsync(predicate)).ToList();
        }

        public async Task<ProductCategory?> GetFullEntry(ProductCategory entry)
        {
            return (await this._unitOfWork.ProductCategoryRepository.GetAsync(u => u.Id == entry.Id)).FirstOrDefault();
        }

        public async Task<bool> RemoveEntry(ProductCategory entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.ProductCategoryRepository.Remove(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while removing Category");
                }
            }

            return result;
        }

        public async Task<bool> UpdateEntry(ProductCategory entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.ProductCategoryRepository.Update(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while updating Category");
                }
            }

            return result;
        }
    }
}
