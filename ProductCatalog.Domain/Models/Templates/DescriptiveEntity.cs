using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Templates
{
    public class DescriptiveEntity : BaseEntity
    {
        [Required, Column("name", TypeName = "NVARCHAR(400)")]
        public string Name { get; set; } = null!;
        [Column("description", TypeName = "TEXT")]
        public string? Description { get; set; } 
    }
}
