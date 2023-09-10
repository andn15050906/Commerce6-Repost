namespace Commerce6.Infrastructure.Models.Merchant
{
    public class ProductDTO
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
        public int RatingCount { get; set; }
        public int TotalRating { get; set; }

        public int ShopId { get; set; }
        public string ThumbImage { get; set; } = null!;
    }
}
