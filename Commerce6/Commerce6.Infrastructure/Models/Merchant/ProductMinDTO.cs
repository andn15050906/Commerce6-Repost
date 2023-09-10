namespace Commerce6.Infrastructure.Models.Merchant
{
    public class ProductMinDTO
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public double Discount { get; set; }
    }
}
