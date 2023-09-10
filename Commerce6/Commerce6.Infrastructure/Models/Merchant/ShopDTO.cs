using Commerce6.Infrastructure.Models.Contact;

namespace Commerce6.Infrastructure.Models.Merchant
{
    public class ShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? BankAccount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
        public int ProductCount { get; set; }
        public int FollowerCount { get; set; }
        public int RatingCount { get; set; }
        public int TotalRating { get; set; }
        public AddressResponseDTO? Address { get; set; }
    }
}
