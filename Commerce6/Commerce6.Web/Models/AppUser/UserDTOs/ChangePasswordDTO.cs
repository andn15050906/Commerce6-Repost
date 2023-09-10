using System.ComponentModel.DataAnnotations;

namespace Commerce6.Web.Models.AppUser.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
        [RegularExpression(@"(?=.*[A-Z].*)(?=.*[a-z].*)(.*[0-9].*)", ErrorMessage = "Password must contain Uppercase, Lowercase and Number")]
        public string NewPassword { get; set; } = null!;
    }
}
