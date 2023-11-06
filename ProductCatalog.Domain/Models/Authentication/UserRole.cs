using ProductCatalog.Domain.Models.Core;
using ProductCatalog.Domain.Models.Templates;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Authentication
{
    [Table("userRole")]
    public class UserRole : DescriptiveEntity 
    {
        public virtual List<User>? Users { get; set; }

        public virtual List<Permission>? Permissions { get; set; }
    }
}
