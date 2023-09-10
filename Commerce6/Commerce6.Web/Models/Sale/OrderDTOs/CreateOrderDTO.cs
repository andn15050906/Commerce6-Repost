using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Web.Models.Sale.OrderDTOs
{
    public class CreateOrderDTO
    {
        public string PaymentMethod { get; set; }
        public string Transporter { get; set; }
        public AddressRequestDTO? ToAddress { get; set; }           //1 of 2
        public bool? DefaultAddress { get; set; }                   //1 of 2
        public int ShopId { get; set; }
        public Order_ProductRequestDTO[] Products { get; set; }
    }
}
