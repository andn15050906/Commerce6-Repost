using Commerce6.Web.Models.Contact.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace Commerce6.Web.Models.AppUser.UserDTOs
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }

        [RegularExpression(@"0\d{9,10}", ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? Facebook { get; set; }

        public AddressRequestDTO? Address { get; set; }
    }
}
