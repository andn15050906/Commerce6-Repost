using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Models.Sale
{
    public class Order_ProductDTO
    {
        public int Quantity { get; set; }
        public ProductMinDTO Product { get; set; }
        //OrderDTO ?
    }
}
