using ProductCatalog.Domain.Models.Templates;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Authentication
{
    [Table("rolePermission")]
    public class UserRolePermission : BaseEntity
    {
        [Column("roleId")]
        public Guid RoleId { get; set; }
        public virtual UserRole? Role { get; set; }

        [Column("permissionId")]
        public Guid PermissionId { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
