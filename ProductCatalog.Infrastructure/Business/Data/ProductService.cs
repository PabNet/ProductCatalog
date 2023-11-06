using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Services.Data
{
    public class ProductService : BaseEntityService, IEntityService<Product>
    {
        private IEntityService<ProductCategory> _categoryService { get; set; }
        public ProductService(ProductCatalog.Domain.Abstractions.IUnitOfWork unitOfWork,
                              IEntityService<ProductCategory> categoryService,
                              ILogger<ProductService> logger) : base(unitOfWork, logger) 
        {
            this._categoryService = categoryService;
        }

        public async Task<bool> AddEntryAsync(Product entry)
        {
            var result = default(bool);

            try
            {
                if(entry.Category == null && entry.CategoryId != null)
                {
                    var category = await this._categoryService.GetFullEntry(new ProductCategory { Id = entry.CategoryId.Value });
                    if(category != null)
                    {
                        entry.Category = category;
                    }
                }

                entry.Price = Convert.ToDecimal(entry.PriceStr);

                await this._unitOfWork.ProductRepository.CreateAsync(entry);
                await _unitOfWork.SaveAsync();

                result = true;
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while adding Product");
            }

            return result;
        }

        public async Task<List<Product>> GetEntries()
        {
            return (await this._unitOfWork.ProductRepository.GetAsync()).ToList();
        }

        public async Task<List<Product>> GetEntries(Func<Product, bool> predicate)
        {
            return (await this._unitOfWork.ProductRepository.GetAsync(predicate)).ToList();
        }

        public async Task<Product?> GetFullEntry(Product entry)
        {
            return (await this._unitOfWork.ProductRepository.GetAsync(u => u.Id == entry.Id)).FirstOrDefault();
        }

        public async Task<bool> RemoveEntry(Product entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.ProductRepository.Remove(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while removing Product");
                }
            }

            return result;
        }

        public async Task<bool> UpdateEntry(Product entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    existingUser.Price = Convert.ToDecimal(entry.PriceStr);
                    this._unitOfWork.ProductRepository.Update(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while updating Product");
                }
            }

            return result;
        }
    }
}
