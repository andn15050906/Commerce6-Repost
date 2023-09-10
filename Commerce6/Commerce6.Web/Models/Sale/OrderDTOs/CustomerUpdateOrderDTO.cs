using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Web.Models.Sale.OrderDTOs
{
    public class CustomerUpdateOrderDTO
    {
        public string OrderId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Transporter { get; set; }
        public int? State { get; set; }
        public AddressRequestDTO? ToAddress { get; set; }
    }
}
