namespace Commerce6.Web.Models.Merchant.ProductReviewDTOs
{
    public class CreateProductReviewDTO
    {
        public string? Content { get; set; }
        public int Rating { get; set; }
        public string ProductId { get; set; } = null!;
    }
}
