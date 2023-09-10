using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Models.Sale
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string PaymentMethod { get; set; }
        public string Transporter { get; set; }
        public string State { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public int Fee { get; set; }
        public int Price { get; set; }
        public double Discount { get; set; }
        public UserMinDTO Customer { get; set; }
        public AddressResponseDTO FromAddress { get; set; }
        public AddressResponseDTO ToAddress { get; set; }
        public ShopMinDTO Shop { get; set; }
        public IEnumerable<Order_ProductDTO>? Order_Products { get; set; }
    }
}
