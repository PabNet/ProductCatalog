using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Services.Abstractions;


namespace ProductCatalog.Services.Data
{
    public class UserService : BaseEntityService, IEntityService<User> 
    {
        protected IEntityService<UserRole> _userRoleService { get; set; }

        public UserService(ProductCatalog.Domain.Abstractions.IUnitOfWork unitOfWork,
                           IEntityService<UserRole> userRoleService,
                           ILogger<UserService> logger) : base(unitOfWork, logger) 
        {
            this._userRoleService = userRoleService;
        }

        public async Task<bool> AddEntryAsync(User entry)
        {
            var result = default(bool);

            try
            {
                if (entry.Role == null)
                {
                    if(entry.RoleId != null)
                    {
                        var role = await this._userRoleService.GetFullEntry(new UserRole { Id = entry.RoleId.Value });
                        if (role != null)
                        {
                            entry.Role= role;
                        }
                    }
                    else
                    {
                        var roleName = default(string);

                        var users = (await GetEntries()).ToList();
                        if (users.Count > 0)
                        {
                            var existingUser = users.Where(u => u.Login == entry.Login
                                                            &&
                                                            u.Password == entry.Password)
                                                .FirstOrDefault();
                            if (existingUser != null)
                            {
                                return false;
                            }

                            roleName = "Simple User";
                        }
                        else
                        {
                            roleName = "Administrator";
                        }

                        var userRole = await this._userRoleService.GetFullEntry(new UserRole { Name = roleName });
                        if (userRole != null)
                        {
                            entry.Role = userRole;
                        }
                    }
                }

                await _unitOfWork.UserRepository.CreateAsync(entry);
                await _unitOfWork.SaveAsync();

                result = true;
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while adding User");
            }
            
            return result;
        }

        public async Task<List<User>> GetEntries()
        {
            return (await this._unitOfWork.UserRepository.GetAsync()).ToList();
        }

        public async Task<List<User>> GetEntries(Func<User, bool> predicate)
        {
            return (await this._unitOfWork.UserRepository.GetAsync(predicate)).ToList();
        }

        public async Task<User?> GetFullEntry(User entry)
        {
            return (await GetEntries(u => (entry.Login != default && u.Login == entry.Login) || (entry.Id != default && u.Id == entry.Id))).FirstOrDefault();
        }

        public async Task<bool> RemoveEntry(User entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.UserRepository.Remove(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while removing User");
                }
            }

            return result;
        }

        public async Task<bool> UpdateEntry(User entry)
        {
            var result = default(bool);

            var existingUser = await GetFullEntry(entry);

            if (existingUser != null)
            {
                try
                {
                    this._unitOfWork.UserRepository.Update(existingUser);
                    await _unitOfWork.SaveAsync();

                    result = true;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "An error occurred while updating User");
                }
            }

            return result;
        }
    }
}
