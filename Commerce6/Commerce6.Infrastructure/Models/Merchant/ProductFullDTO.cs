using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.Contact;

namespace Commerce6.Infrastructure.Models.Merchant
{
    public class ProductFullDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public string Description { get; set; } = null!;
        public double Discount { get; set; }
        public int Stock { get; set; }
        public int Sold { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ServerTag { get; set; }
        public int ViewCount { get; set; }
        public int RatingCount { get; set; }
        public int TotalRating { get; set; }

        public ShopMinDTO Shop { get; set; } = null!;
        public string ShopCategoryName { get; set; } = null!;

        public IEnumerable<AttributeDTO> Attributes { get; set; } = null!;
        public IEnumerable<CommentDTO> FirstComments { get; set; } = null!;
        public IEnumerable<ProductReviewDTO> FirstReviews { get; set; } = null!;
        public IEnumerable<ProductImageDTO> Images { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
