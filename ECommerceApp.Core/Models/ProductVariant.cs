using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Models
{
    public class ProductVariant
    {
        public int ProductId { get; set; }
        
        [JsonIgnore]
        public Product ?Product { get; set; }
        
        public int ProductTypeId { get; set; }
        
        public ProductType ?ProductType { get; set; }
        
        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }
        public bool Visible { get; set; } = true;

        public bool Deleted { get; set; } = false;

        [NotMapped]
        public bool Editing { get; set; } = false;

        [NotMapped]
        public bool IsNew { get; set; } = false;

    }
}
