namespace Commerce6.Web.Models.Merchant.ShopReviewDTOs
{
    public class CreateShopReviewDTO
    {
        public string? Content { get; set; }
        public int Rating { get; set; }
        public int ShopId { get; set; }
    }
}
