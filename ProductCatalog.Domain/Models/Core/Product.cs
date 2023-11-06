using ProductCatalog.Domain.Models.Authentication;
using ProductCatalog.Domain.Models.Templates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Models.Core
{
    [Table("product")]
    public class Product : DescriptiveEntity
    {
        [Column("generalNote", TypeName = "NVARCHAR(255)")]
        public string? GeneralNote { get; set; }
        [Column("specialNote", TypeName = "NVARCHAR(255)")]
        public string? SpecialNote { get; set; }
        [Required, ForeignKey("categoryId")]
        public virtual ProductCategory Category { get; set; } = null!;

        [NotMapped]
        public Guid? CategoryId { get; set; }

        [NotMapped]
        private string? priceStr;

        [NotMapped]
        public string? PriceStr
        {
            get
            {
                if (!string.IsNullOrEmpty(priceStr))
                {
                    return priceStr.Replace(".", ",");
                }
                return priceStr;
            }
            set
            {
                priceStr = value;
            }
        }

        [Column("price", TypeName = "DECIMAL(10,2)")]
        public decimal Price { get; set; }
    }
}
