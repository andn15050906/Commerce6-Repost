using System.ComponentModel.DataAnnotations;

using Commerce6.Web.Models.Merchant.AttributeDTOs;
using Commerce6.Web.Models.Merchant.ProductImageDTOs;

namespace Commerce6.Web.Models.Merchant.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string Id { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Product name must be less than 100 characters")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must not be negative")]
        public int? Price { get; set; }

        [StringLength(1000, ErrorMessage = "Product description must be less than 1000 characters")]
        public string? Description { get; set; }

        [Range(0.0, 1, ErrorMessage = "Discount must be between 0 and 100%")]
        public double? Discount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must not be negative")]
        public int? Stock { get; set; }



        public int[]? DeletedAttributes { get; set; }
        public AttributeRequestDTO[]? AddedAttributes { get; set; }

        public string[]? DeletedImages { get; set; }                //send fileName, not id
        public CreateProductImageDTO[]? AddedImages { get; set; }
    }
}
