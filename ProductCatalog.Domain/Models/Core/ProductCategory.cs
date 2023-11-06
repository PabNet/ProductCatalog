using ProductCatalog.Domain.Models.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Models.Core
{
    [Table("productCategory")]
    public class ProductCategory : DescriptiveEntity 
    {
        public virtual List<Product>? Products { get; set; }
    }
}
