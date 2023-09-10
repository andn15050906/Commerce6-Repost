namespace Commerce6.Web.Models.AppUser.UserDTOs
{
    public class LoginDTO
    {
        public string PhoneOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
