using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Abstractions;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Services.Abstractions;

namespace ProductCatalog.Services.Data
{
    public class UserRoleService : BaseEntityService, IEntityService<UserRole>
    {
        public UserRoleService(ProductCatalog.Domain.Abstractions.IUnitOfWork unitOfWork, ILogger<UserRoleService> logger) : base(unitOfWork, logger) {}

        public async Task<bool> AddEntryAsync(UserRole entry)
        {
            var result = default(bool);

            try
            {
                await this._unitOfWork.UserRoleRepository.CreateAsync(entry);
                await _unitOfWork.SaveAsync();

                result = true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while adding Role");
            }

            return result;
        }

        public async Task<List<UserRole>> GetEntries()
        {
            return (await this._unitOfWork.UserRoleRepository.GetAsync()).ToList();
        }

        public async Task<List<UserRole>> GetEntries(Func<UserRole, bool> predicate)
        {
            return (await this._unitOfWork.UserRoleRepository.GetAsync(predicate)).ToList();
        }

        public async Task<UserRole?> GetFullEntry(UserRole entry)
        {
            return (await this._unitOfWork.UserRoleRepository.GetAsync(u => u.Name == entry.Name || u.Id == entry.Id))
                                                             .FirstOrDefault();
        }

        public async Task<bool> RemoveEntry(UserRole entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.UserRoleRepository.Remove(existingUser);
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while removing Product");
                }
            }

            return result;
        }

        public async Task<bool> UpdateEntry(UserRole entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.UserRoleRepository.Update(existingUser);
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while updating Role");
                }
            }

            return result;
        }
    }
}
