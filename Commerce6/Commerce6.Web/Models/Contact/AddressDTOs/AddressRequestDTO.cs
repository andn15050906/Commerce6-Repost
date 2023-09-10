namespace Commerce6.Web.Models.Contact.AddressDTOs
{
    public class AddressRequestDTO
    {
        public string Province { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string StreetNumber { get; set; } = null!;
    }
}
