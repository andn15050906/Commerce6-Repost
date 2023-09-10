using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Web.Models.Sale.OrderDTOs
{
    public class MerchantUpdateOrderDTO
    {
        public string OrderId { get; set; }
        public int? State { get; set; }
        public double? Discount { get; set; }
        public AddressRequestDTO? FromAddress { get; set; }
        public bool? DefaultAddress { get; set; }
    }
}
