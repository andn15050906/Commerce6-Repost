using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Web.Models.Sale.OrderDTOs
{
    public class Order_ProductRequestDTO
    {
        public int Quantity { get; set; }
        public string ProductId { get; set; }
    }
}
