using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Services.Abstractions;
using ProductCatalog.Utility.Enums;

namespace ProductCatalog.Infrastructure.Business.Common
{
    public class AccessControlService : IAccessControlService
    {
        private IEntityService<UserRole> _roleService { get; set; }
        public AccessControlService(IEntityService<UserRole> roleService)
        {
            this._roleService = roleService;
        }
        public async Task<bool> HasAccess(AccessAction action, string roleName, params object[] permissions)
        {
            var hasAccess = default(bool);

            var role = await this._roleService.GetFullEntry(new UserRole { Name = roleName });
            if(role != null && role.Permissions != null)
            {
                switch (action)
                {
                    case AccessAction.SpecificPermission:
                        {
                            hasAccess = role.Permissions.Any(p => p.Name == permissions.First().ToString());

                            break;
                        }
                    case AccessAction.CheckAtLeastOnePermission:
                        {
                            hasAccess = role.Permissions.Any(p => p.Name.Contains(permissions.First().ToString()));

                            break;
                        }
                }
            }

            return hasAccess;
        }
    }
}
