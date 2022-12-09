using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Featured { get; set; } = false;
        public int CategoryId { get; set; }
        public Category ?Category { get; set; }
        public List<ProductVariant>? ProductVariants { get; set; } = new List<ProductVariant>(); 
    }
}
