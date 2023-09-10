using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Models.AppUser
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Avatar { get; set; }
        public string? Facebook { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastSeen { get; set; }
        public bool IsConfirmed { get; set; }
        public AddressResponseDTO? Address { get; set; }
    }
}
