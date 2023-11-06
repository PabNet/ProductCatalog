using ProductCatalog.Domain.Models.Templates;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Authentication
{
    [Table("permission")]
    public class Permission : DescriptiveEntity 
    {
        public virtual List<UserRole>? Roles { get; set; }
    }
}
