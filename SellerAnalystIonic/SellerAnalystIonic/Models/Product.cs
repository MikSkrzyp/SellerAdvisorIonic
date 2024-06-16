using System.ComponentModel.DataAnnotations;

namespace SellerAnalystIonic.Models
{
    public class Product
    {
        [Key]
        public string Barcode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
