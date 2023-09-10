using System.ComponentModel.DataAnnotations;

using Commerce6.Web.Models.Merchant.AttributeDTOs;
using Commerce6.Web.Models.Merchant.ProductImageDTOs;

namespace Commerce6.Web.Models.Merchant.ProductDTOs
{
    public class CreateProductDTO
    {
        [StringLength(100, ErrorMessage = "Product name must be less than 100 characters")]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Price must not be negative")]
        public int Price { get; set; }

        [StringLength(1000, ErrorMessage = "Product description must be less than 1000 characters")]
        public string? Description { get; set; }

        [Range(0.0, 1, ErrorMessage = "Discount must be between 0 and 100%")]
        public double Discount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must not be negative")]
        public int Stock { get; set; }

        public string? CountUnit { get; set; }

        public int? ShopCategoryId { get; set; }
        public AttributeRequestDTO[]? Attributes { get; set; }
        public CreateProductImageDTO[]? Images { get; set; }
        public int CategoryId { get; set; }
    }
}
