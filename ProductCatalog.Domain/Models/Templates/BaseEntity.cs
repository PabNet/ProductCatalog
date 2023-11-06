using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Domain.Models.Templates
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
