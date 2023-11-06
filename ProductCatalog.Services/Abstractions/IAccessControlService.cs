using ProductCatalog.Utility.Enums;

namespace ProductCatalog.Services.Abstractions
{
    public interface IAccessControlService
    {
        Task<bool> HasAccess(AccessAction action, string roleName, params object[] permissions);
    }
}
