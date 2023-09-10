using System.ComponentModel.DataAnnotations;

namespace Commerce6.Web.Models.AppUser.UserDTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = null!;

        [RegularExpression(@"0\d{9,10}", ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } = null!;

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
        [RegularExpression(@"(?=.*[A-Z].*)(?=.*[a-z].*)(.*[0-9].*)", ErrorMessage = "Password must contain Uppercase, Lowercase and Number")]
        public string Password { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }
    }
}
