using Commerce6.Infrastructure.Models.AppUser;

namespace Commerce6.Infrastructure.Models.Contact
{
    public class ProductReviewDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public UserMinDTO Customer { get; set; } = null!;
        public string ProductId { get; set; } = null!;
    }
}
