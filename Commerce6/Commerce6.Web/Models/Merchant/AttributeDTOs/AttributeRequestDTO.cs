using System.ComponentModel.DataAnnotations;

namespace Commerce6.Web.Models.Merchant.AttributeDTOs
{
    public class AttributeRequestDTO
    {
        [StringLength(45)]
        public string Name { get; set; } = null!;
        [StringLength(45)]
        public string Value { get; set; } = null!;
    }
}
